using System.Collections.Generic;

namespace Chalk.Interop
{
    public interface IArgument
    {
        IEnumerable<string> FormatCommandLine(string prefix, CommandLineArgumentStyle argumentStyle);
    }
}