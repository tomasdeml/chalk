using System.Collections.Generic;

namespace Chalk.Interop
{
    public class PositionalArgument : IArgument
    {
        public string Value { get; private set; }

        public PositionalArgument(string value)
        {
            Value = value;
        }

        public IEnumerable<string> FormatCommandLine(string prefix, CommandLineArgumentStyle argumentStyle)
        {
            return new[] {Value};
        }
    }
}