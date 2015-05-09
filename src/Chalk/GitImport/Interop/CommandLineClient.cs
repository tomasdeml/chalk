using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chalk.Interop;
using TwoPS.Processes;

namespace Chalk.GitImport.Interop
{
    public class CommandLineClient
    {
        const string GitExecutableFileName = "git.exe";
        const string DoubleDashArgumentPrefix = "--";

        readonly string workingDirectory;

        public CommandLineClient(string workingDirectory)
        {
            this.workingDirectory = workingDirectory;
        }

        public void ExecuteCommand(string command, params IFormattableArgument[] arguments)
        {
            ExecuteCommand(command, null, arguments);
        }

        public void ExecuteCommand(string command, string textForStandardInput, params IFormattableArgument[] arguments)
        {
            var commandLineArguments = new List<string> {command};
            commandLineArguments.AddRange(
                arguments.SelectMany(
                    a => a.FormatCommandLine(DoubleDashArgumentPrefix, CommandLineArgumentStyle.SingleWithEqualsSignDelimiter)));

            Process process = NewProcess(commandLineArguments);

            if (!string.IsNullOrWhiteSpace(textForStandardInput))
                process.Options.StandardInputAppend(textForStandardInput);

            process.ExecuteAndVerifyResult();
        }

        Process NewProcess(IEnumerable<string> arguments)
        {
            return new Process(new ProcessOptions(GitExecutableFileName, arguments)
            {
                WorkingDirectory = workingDirectory,
                StandardInputEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            });
        }
    }
}