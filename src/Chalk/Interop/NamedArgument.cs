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
                ? new[] {string.Format("{0}{1}={2}", prefix, Name, Value)}
                : new[] {prefix + Name, Value};
        }
    }
}