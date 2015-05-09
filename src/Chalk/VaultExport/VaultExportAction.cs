using System;
using System.Collections.Generic;
using System.Linq;
using Chalk.Actions;
using Chalk.Logging;
using Chalk.VaultExport.Interop;

namespace Chalk.VaultExport
{
    public class VaultExportAction
    {
        readonly ActionContext context;
        readonly VaultFacade vaultFacade;
        readonly Action<VersionHistoryItem> versionDownloadedSubAction;
        readonly ILastVersionMarker lastVersionMarker;
        readonly DirectoryCleaner directoryCleaner;

        public VaultExportAction(ActionContext context, VaultFacade vaultFacade,
            ILastVersionMarker lastVersionMarker, DirectoryCleaner directoryCleaner,
            Action<VersionHistoryItem> versionDownloadedSubAction)
        {
            this.context = context;
            this.vaultFacade = vaultFacade;
            this.versionDownloadedSubAction = versionDownloadedSubAction;
            this.lastVersionMarker = lastVersionMarker;
            this.directoryCleaner = directoryCleaner;
        }

        public void Execute()
        {
            VersionHistoryItem[] historyItems = GetVersionHistory();
            if (historyItems.Length == 0)
            {
                context.Logger.LogInfo(Progress.Finished, "Up-to-date, no newer version found");
                return;
            }

            int beginVersion = historyItems.First().Version;
            int endVersion = historyItems.Last().Version;
            context.Logger.LogInfo("Found {0} newer versions ({1}..{2})", historyItems.Length, beginVersion, endVersion);

            foreach (var exportStep in GetExportSteps(historyItems, beginVersion, endVersion))
            {
                var startTime = DateTime.Now;
                context.Logger.LogInfo(exportStep.Progress, "Processing version {0}...",
                    exportStep.HistoryItem.Version.ToString());

                if (exportStep.NeedsCleanWorkspace)
                    CleanWorkspace();

                DownloadFilesAtVersion(exportStep.HistoryItem.Version);
                versionDownloadedSubAction(exportStep.HistoryItem);
                lastVersionMarker.Mark(exportStep.HistoryItem.Version);

                context.Logger.LogInfo("Done, took {0}s", Math.Ceiling((DateTime.Now - startTime).TotalSeconds));
            }
        }

        IEnumerable<VersionExportStep> GetExportSteps(VersionHistoryItem[] historyItems, int beginVersion, int endVersion)
        {
            var transactionsWithDeletion = GetTransactionsWithDeletion(beginVersion, endVersion);

            return
                historyItems.Select(
                    (item, idx) =>
                        new VersionExportStep(item,
                            idx, historyItems.Length, transactionsWithDeletion.Contains(item.TransactionId)));
        }

        VersionHistoryItem[] GetVersionHistory()
        {
            int? nextVersionToGet = lastVersionMarker.GetNext();
            context.Logger.LogInfo(Progress.Starting, "Fetching history from Vault (begin version {1})...",
                context.Parameters.VaultRepositoryPath,
                nextVersionToGet != null ? nextVersionToGet.ToString() : "LATEST");

            var versionHistory = vaultFacade.NewVersionHistory(context.Parameters.VaultRepositoryName,
                context.Parameters.VaultRepositoryPath);
            return versionHistory.Fetch(nextVersionToGet).ToArray();
        }

        ISet<int> GetTransactionsWithDeletion(int beginVersion, int endVersion)
        {
            if (context.Parameters.DisableRepositoryDeletionDetection)
            {
                return new HashSet<int>();
            }
            {
                context.Logger.LogInfo("Fetching repository path deletion history...");

                var deletionHistory = vaultFacade.NewRepositoryDeletionHistory(context.Parameters.VaultRepositoryName,
                    context.Parameters.VaultRepositoryPath); 
                return deletionHistory.GetTransactionsContainingDeletions(beginVersion, endVersion);
            }
        }

        void CleanWorkspace()
        {
            context.Logger.LogInfo("Version contains renames/moves/deletions, cleaning workspace...");
            directoryCleaner.DeleteContents(context.Parameters.LocalWorkspacePath);
        }

        void DownloadFilesAtVersion(int version)
        {
            context.Logger.LogInfo("Getting files...");

            var versionSnapshot = vaultFacade.NewVersionSnapshot(context.Parameters.VaultRepositoryName,
                context.Parameters.VaultRepositoryPath);
            versionSnapshot.Get(version, context.Parameters.LocalWorkspacePath); 
        }

        struct VersionExportStep
        {
            public VersionHistoryItem HistoryItem { get; private set; }
            public bool NeedsCleanWorkspace { get; private set; }
            public Progress Progress { get; private set; }

            public VersionExportStep(VersionHistoryItem historyItem, int stepIndex, int totalNumberOfSteps, bool needsCleanWorkspace)
                : this()
            {
                HistoryItem = historyItem;
                NeedsCleanWorkspace = needsCleanWorkspace;
                Progress = Progress.ForStep(stepIndex, totalNumberOfSteps);
            }
        }
    }
}