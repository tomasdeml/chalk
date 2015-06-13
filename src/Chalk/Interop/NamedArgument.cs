using System.Collections.Generic;
using Seterlund.CodeGuard;

namespace Chalk.Interop
{
    public class NamedArgument : IArgument
    {
        public string Name { get; private set; } 
        public string Value { get; private set; }

        public NamedArgument(string name, string value)
        {
            Guard.That(name).IsNotNullOrEmpty();
            Name = name;
            Value = value;
        }

        public virtual IEnumerable<string> FormatCommandLine(string prefix, CommandLineArgumentStyle argumentStyle)
        {
            return argumentStyle == CommandLineArgumentStyle.SingleWithEqualsSignDelimiter
                ? FormatSingleCommandLineWithEqualsDelimiter(prefix)
                : FormatSeparatedCommandLine(prefix);
        }

        string[] FormatSeparatedCommandLine(string prefix)
        {
            return new[] {prefix + Name, Value};
        }

        string[] FormatSingleCommandLineWithEqualsDelimiter(string prefix)
        {
            return new[] {string.Format("{0}{1}={2}", prefix, Name, Value)};
        }
    }
}