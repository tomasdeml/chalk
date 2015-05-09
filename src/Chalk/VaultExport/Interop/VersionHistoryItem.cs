using System;
using Seterlund.CodeGuard;

namespace Chalk.VaultExport.Interop
{
    public class VersionHistoryItem
    {
        public int Version { get; private set; }
        public int TransactionId { get; private set; }

        public DateTime Date { get; private set; }

        public string Comment { get; private set; }
        public string Author { get; private set; }

        public VersionHistoryItem(int version, int transactionId, DateTime date, string comment, string author)
        {
            Guard.That(version, "version").IsGreaterThan(-1);
            Guard.That(transactionId, "transactionId").IsGreaterThan(-1);
            Guard.That(author, "author").IsNotNullOrEmpty();
            Version = version;
            TransactionId = transactionId;
            Date = date;
            Comment = comment;
            Author = author;
        }
    }
}