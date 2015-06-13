using System.Collections.Generic;

namespace Chalk.Interop
{
    public class SwitchArgument : IArgument
    {
        public string Name { get; private set; }

        public SwitchArgument(string name)
        {
            Name = name;
        }

        public IEnumerable<string> FormatCommandLine(string prefix, CommandLineArgumentStyle argumentStyle)
        {
            return new[] { prefix + Name };
        }

        public override string ToString()
        {
            return "/" + Name;
        }
    }
}