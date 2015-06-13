using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chalk.Interop;
using Chalk.Logging;
using TwoPS.Processes;

namespace Chalk.GitImport.Interop
{
    public class CommandLineClient
    {
        const string GitExecutableFileName = "git.exe";
        const string DoubleDashArgumentPrefix = "--";

        readonly string workingDirectory;
        readonly ILogger logger;

        public CommandLineClient(string workingDirectory, ILogger logger)
        {
            this.workingDirectory = workingDirectory;
            this.logger = logger;
        }

        public void ExecuteCommand(string command, params IArgument[] arguments)
        {
            ExecuteCommand(command, null, arguments);
        }

        public void ExecuteCommand(string command, string textForStandardInput, params IArgument[] arguments)
        {
            IEnumerable<string> commandLineArguments = GetCommandLineArguments(command, arguments); 
            Process gitProcess = NewGitProcess(commandLineArguments);

            if (!string.IsNullOrWhiteSpace(textForStandardInput))
                gitProcess.Options.StandardInputAppend(textForStandardInput);

            gitProcess.ExecuteAndVerifyResult(logger);
        }

        static IEnumerable<string> GetCommandLineArguments(string command, IArgument[] arguments)
        {
            var commandLineArguments = new List<string> {command};
            commandLineArguments.AddRange(
                arguments.SelectMany(
                    a => a.FormatCommandLine(DoubleDashArgumentPrefix, CommandLineArgumentStyle.SingleWithEqualsSignDelimiter)));

            return commandLineArguments;
        }

        Process NewGitProcess(IEnumerable<string> arguments)
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