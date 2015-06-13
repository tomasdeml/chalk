using System;
using Chalk.Logging;
using Seterlund.CodeGuard;
using TwoPS.Processes;

namespace Chalk.Interop
{
    static class ProcessExtensions
    {
        const int SuccessExitCode = 0;

        public static void ExecuteAndVerifyResult(this Process process, ILogger logger, ResultExpectation resultExpectation = ResultExpectation.MustNotFail)
        {
            Guard.That(process).IsNotNull();
            Guard.That(resultExpectation).IsValidValue();
            Guard.That(logger).IsNotNull();

            logger.LogDebug("Running process \"{0}\"", process.Options.CommandLine);
            ProcessResult result = process.Run();

            VerifyProcessRan(process, result); 
            VerifyExitCode(process, result, resultExpectation, logger);
        }

        static void VerifyExitCode(this Process command, ProcessResult result, ResultExpectation resultExpectation, ILogger logger)
        {
            if (result.ExitCode == SuccessExitCode)
                return;

            string message = string.Format("Process \"{0}\" failed, exit code {1}. All output: {2}",
                command.Options.CommandLine, result.ExitCode, result.AllOutput);

            if (resultExpectation == ResultExpectation.MustNotFail)
                throw new Exception(message);

            logger.LogWarning(message);
        }

        static void VerifyProcessRan(this Process command, ProcessResult result)
        {
            if (!result.Success)
                throw new ProcessRunnerException(
                    string.Format("Failed to execute process \"{0}\". Process status was {1}.",
                        command.Options.CommandLine, result.Status));
        }
    }
}
