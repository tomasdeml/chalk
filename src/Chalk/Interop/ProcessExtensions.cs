using System;
using Seterlund.CodeGuard;
using TwoPS.Processes;
using Process = TwoPS.Processes.Process;

namespace Chalk.Interop
{
    static class ProcessExtensions
    {
        public static void ExecuteAndVerifyResult(this Process command)
        {
            Guard.That(command).IsNotNull();

            ProcessResult result = command.Run();

            if (!result.Success)
                throw new ProcessRunnerException(
                    string.Format("Failed to execute process \"{0}\". Process status was {1}.",
                        command.Options.CommandLine, result.Status));

            if (result.ExitCode != 0)
                throw new Exception(string.Format("Process \"{0}\" failed, exit code {1}. All output: {2}",
                    command.Options.CommandLine, result.ExitCode, result.AllOutput));
        } 
    }
}
