using System.Collections.Generic;

namespace Chalk.Interop
{
    public interface IFormattableArgument
    {
        IEnumerable<string> FormatCommandLine(string prefix, CommandLineArgumentStyle argumentStyle);
    }
}