using System.Globalization;
using Chalk.Interop;

namespace Chalk.VaultExport.Interop
{
    public class VersionSnapshot
    {
        const string GetVersionCommand = "GETVERSION";

        const string OverwriteMergeStrategyValue = "overwrite";
        const string RemoveWorkingCopyStrategyValue = "removeworkingcopy";
        const string CheckinFileDateValue = "checkin";

        readonly IVaultClient vaultClient;
        readonly string repositoryName;
        readonly string repositoryPath;

        public VersionSnapshot(IVaultClient vaultClient, string repositoryName, string repositoryPath)
        {
            this.vaultClient = vaultClient;
            this.repositoryName = repositoryName;
            this.repositoryPath = repositoryPath;
        }

        public void Get(int version, string workspacePath)
        {
            vaultClient.ExecuteCommand(GetVersionCommand,
                new[]
                {
                    new PositionalArgument(version.ToString(CultureInfo.InvariantCulture)),
                    new PositionalArgument(repositoryPath), 
                    new PositionalArgument(workspacePath)
                },
                CommandLineClientArgument.RepositoryName(repositoryName),
                CommandLineClientArgument.BackupBeforeOverwriting(false),
                CommandLineClientArgument.MakeFilesWritable(),
                CommandLineClientArgument.MergeStrategy(OverwriteMergeStrategyValue),
                CommandLineClientArgument.FileDeletionStrategy(RemoveWorkingCopyStrategyValue),
                CommandLineClientArgument.SetFileTime(CheckinFileDateValue),
                CommandLineClientArgument.UseWorkingFolder(),
                CommandLineClientArgument.VerboseLogging());
        }
    }
}
