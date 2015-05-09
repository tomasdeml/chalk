using System.Collections.Generic;

namespace Chalk.Interop
{
    public class PrimaryArgument : IFormattableArgument
    {
        public string Value { get; private set; }

        public PrimaryArgument(string value)
        {
            Value = value;
        }

        public IEnumerable<string> FormatCommandLine(string prefix, CommandLineArgumentStyle argumentStyle)
        {
            return new[] {Value};
        }
    }
}