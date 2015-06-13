using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Chalk.Interop;
using Seterlund.CodeGuard;
using YAXLib;

namespace Chalk.VaultExport.Interop
{
    public class RepositoryDeletionHistory
    {
        const string HistoryCommand = "HISTORY";

        static readonly string[] ActionsClassifiedAsDeletion = { "rename", "move", "delete" };

        readonly IVaultClient vaultClient;
        readonly string repositoryName;
        readonly string repositoryPath;

        public RepositoryDeletionHistory(IVaultClient vaultClient, string repositoryName, string repositoryPath)
        {
            Guard.That(vaultClient).IsNotNull();
            Guard.That(repositoryName, "repositoryName").IsNotNullOrEmpty();
            Guard.That(repositoryPath, "repositoryPath").IsNotNullOrEmpty();
            this.vaultClient = vaultClient;
            this.repositoryName = repositoryName;
            this.repositoryPath = repositoryPath;
        }

        public ISet<int> GetTransactionsContainingDeletions(int beginVersion, int endVersion)
        {
            IArgument[] arguments = CreateArguments(repositoryName, beginVersion, endVersion);
            var historyOutput = vaultClient.ExecuteCommand<HistoryCommandOutput>(HistoryCommand,
                new PositionalArgument(repositoryPath), arguments);

            var transactionIds = new HashSet<int>(historyOutput.HistoryItems.Select(i => i.TransactionId));
            return new HashSet<int>(transactionIds);
        }

        static IArgument[] CreateArguments(string repositoryName, int beginVersion, int endVersion)
        {
            return new IArgument[] {
                CommandLineClientArgument.RepositoryName(repositoryName),
                CommandLineClientArgument.BeginVersion(beginVersion),
                CommandLineClientArgument.EndVersion(endVersion),
                CommandLineClientArgument.IncludeActions(ActionsClassifiedAsDeletion)
            };
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        class HistoryCommandOutput
        {
            [YAXSerializeAs("history")]
            [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "item")]
            [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
            public HistoryItem[] HistoryItems { get; set; }

            public class HistoryItem
            {
                [YAXSerializeAs("txid")]
                [YAXAttributeForClass]
                public int TransactionId { get; set; }
            }
        }
    }
}