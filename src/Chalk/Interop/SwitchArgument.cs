using System.Collections.Generic;

namespace Chalk.Interop
{
    public class SwitchArgument : NamedArgument
    {
        public SwitchArgument(string name)
            : base(name, null)
        {
        }

        public override IEnumerable<string> FormatCommandLine(string prefix, CommandLineArgumentStyle argumentStyle)
        {
            return new[] { prefix + Name };
        }
    }
}