using System;
using System.Globalization;
using Chalk.Interop;
using Seterlund.CodeGuard;

namespace Chalk.VaultExport.Interop
{
    internal static class CommandLineClientArgument
    {
        public static NamedArgument EndVersion(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value");

            return new NamedArgument("endversion", value.ToString(CultureInfo.InvariantCulture));
        }

        public static NamedArgument MergeStrategy(string value)
        {
            Guard.That(value, "value").IsNotNullOrEmpty();
            return new NamedArgument("merge", value);
        }

        public static NamedArgument FileDeletionStrategy(string value)
        {
            Guard.That(value, "value").IsNotNullOrEmpty();
            return new NamedArgument("performdeletions", value);
        }

        public static NamedArgument SetFileTime(string value)
        {
            Guard.That(value, "value").IsNotNullOrEmpty();
            return new NamedArgument("setfiletime", value);
        }

        public static SwitchArgument UseWorkingFolder()
        {
            return new SwitchArgument("useworkingfolder");
        }

        public static SwitchArgument VerboseLogging()
        {
            return new SwitchArgument("verbose");
        }

        public static NamedArgument RepositoryName(string value)
        {
            Guard.That(value, "value").IsNotNullOrEmpty();
            return new NamedArgument("repository", value);
        }

        public static NamedArgument RowLimit(int value)
        {
            Guard.That(value).IsGreaterThan(0);
            return new NamedArgument("rowlimit", value.ToString());
        }

        public static NamedArgument IncludeActions(params string[] values)
        {
            Guard.That(values, "values").IsNotNull().IsNotEmpty();
            return new NamedArgument("includeactions", string.Join(",", values));
        }

        public static NamedArgument BackupBeforeOverwriting(bool value)
        {
            return new NamedArgument("backup", value ? "yes" : "no");
        }

        public static SwitchArgument MakeFilesWritable()
        {
            return new SwitchArgument("makewritable");
        }

        public static NamedArgument BeginVersion(int value)
        {
            Guard.That(value, "value").IsGreaterThan(-1); 
            return new NamedArgument("beginversion", value.ToString(CultureInfo.InvariantCulture));
        }

        public static NamedArgument Password(string value)
        {
            Guard.That(value, "value").IsNotNullOrEmpty();
            return new NamedArgument("password", value);
        }

        public static NamedArgument User(string value)
        {
            Guard.That(value, "value").IsNotNullOrEmpty();
            return new NamedArgument("user", value);
        }

        public static NamedArgument Host(string value)
        {
            Guard.That(value, "value").IsNotNullOrEmpty();
            return new NamedArgument("host", value);
        }
    }
}