using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Net;
using Chalk.GitImport;
using Chalk.VaultExport;
using Chalk.VaultExport.Interop;
using Seterlund.CodeGuard;

namespace Chalk.Actions
{
    public class ActionComposer
    {
        static readonly IDictionary<ActionKind, Func<ActionContext, Action>> OuterActionConstructors =
            new Dictionary<ActionKind, Func<ActionContext, Action>>
            {
                {ActionKind.WorkspaceInit, NewWorkspaceInitAction},
                {ActionKind.VaultExport, NewVaultExportAction}
            };

        public Action Compose(ActionKind actionKind, ActionContext context)
        {
            Guard.That(context).IsNotNull();

            var actionConstructor = GetConstructorFor(actionKind);
            var action = actionConstructor(context);

            var prepareWorkspaceAction = NewPrepareWorkspaceAction(context);
            return (Action) Delegate.Combine(prepareWorkspaceAction, action); 
        }

        static Func<ActionContext, Action> GetConstructorFor(ActionKind actionKind)
        {
            Func<ActionContext, Action> actionConstructor;
            if (!OuterActionConstructors.TryGetValue(actionKind, out actionConstructor))
                throw new ArgumentOutOfRangeException("actionKind");

            return actionConstructor;
        }

        static Action NewWorkspaceInitAction(ActionContext context)
        {
            return (Action) Delegate.Combine(
                NewGitInitializeRepositoryAction(context),
                NewPrepareEmptyVersionMarkerAction(context));
        }

        static Action NewPrepareEmptyVersionMarkerAction(ActionContext context)
        {
            var versionMarker = new FilePersistedLastVersionMarker(context.Parameters.LocalWorkspacePath, new FileSystem());
            return new WriteNoVersionMarkerAction(versionMarker).Execute;
        }

        static Action NewPrepareWorkspaceAction(ActionContext context)
        {
            return new PrepareWorkspaceAction(context, new FileSystem()).Execute;
        }

        static Action NewGitInitializeRepositoryAction(ActionContext context)
        { 
            var gitCommandLineClient = new GitImport.Interop.CommandLineClient(context.Parameters.LocalWorkspacePath);
            var gitFacade = new GitImport.Interop.CommandLineClientFacade(gitCommandLineClient, new FileSystem());

            return new GitInitializeLocalRepositoryAction(context, gitFacade).Execute;
        }

        static Action NewVaultExportAction(ActionContext context)
        { 
            var vaultCredential = new NetworkCredential(context.Parameters.VaultUserName,
                context.Parameters.VaultPassword);
            var vaultClient = new VaultExport.Interop.CommandLineClient(context.Parameters.VaultCommandLineClientPath,
                context.Parameters.VaultHost, vaultCredential,
                TimeSpan.FromSeconds(context.Parameters.VaultServerTimeOutInSeconds));
            var vaultFactory = new VaultExport.Interop.VaultFacade(vaultClient);

            var gitClient = new GitImport.Interop.CommandLineClient(context.Parameters.LocalWorkspacePath);
            var versionMarker = new FilePersistedLastVersionMarker(context.Parameters.LocalWorkspacePath, new FileSystem());

            Action<VersionHistoryItem> versionDownloadedAction =
                new GitCommitFilesAction(context, gitClient).Execute;

            return
                new VaultExportAction(context, vaultFactory, versionMarker, new DirectoryCleaner(new FileSystem()), 
                    versionDownloadedAction).Execute;
        }
    }
}