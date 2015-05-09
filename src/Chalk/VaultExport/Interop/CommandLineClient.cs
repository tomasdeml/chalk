using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Chalk.Interop;
using TwoPS.Processes;
using YAXLib;

namespace Chalk.VaultExport.Interop
{
    public class CommandLineClient : IVaultClient
    {
        const string SingleDashArgumentPrefix = "-";

        static readonly YAXSerializer CommandResultSerializer;

        readonly string vaultHost;
        readonly NetworkCredential vaultCredential;
        readonly TimeSpan commandTimeOut;
        readonly string commandLineClientPath;

        static CommandLineClient()
        {
            CommandResultSerializer = new YAXSerializer(typeof(CommandLineClientResult),
                YAXExceptionHandlingPolicies.ThrowWarningsAndErrors);
        }

        public CommandLineClient(string commandLineClientPath, string vaultHost, NetworkCredential vaultCredential, TimeSpan commandTimeOut)
        {
            this.vaultHost = vaultHost;
            this.vaultCredential = vaultCredential;
            this.commandTimeOut = commandTimeOut;
            this.commandLineClientPath = commandLineClientPath;
        }

        public void ExecuteCommand(string command, IEnumerable<PrimaryArgument> primaryArguments, params NamedArgument[] additionalArguments)
        {
            ExecuteCommandAndVerifyResult(command, primaryArguments, additionalArguments);
        }

        ProcessResult ExecuteCommandAndVerifyResult(string command, IEnumerable<PrimaryArgument> primaryArguments, NamedArgument[] additionalArguments)
        {
            var commandResult = RunClientProcess(command, primaryArguments, additionalArguments);
            VerifyProcessResult(commandResult);
            VerifyClientCommandResult(commandResult, command);

            return commandResult;
        }

        public TOutput ExecuteCommand<TOutput>(string command, PrimaryArgument primaryArguments, params NamedArgument[] additionalArguments)
        {
            ProcessResult processResult = ExecuteCommandAndVerifyResult(command, new[] {primaryArguments}, additionalArguments);

            var commandOutputSerializer = new YAXSerializer(typeof(TOutput), YAXExceptionHandlingPolicies.ThrowWarningsAndErrors);
            var commandOutput = commandOutputSerializer.Deserialize(processResult.StandardOutput);

            return (TOutput)commandOutput;
        }

        static void VerifyClientCommandResult(ProcessResult processResult, string command)
        {
            var commandResult = (CommandLineClientResult) CommandResultSerializer.Deserialize(processResult.StandardOutput);

            if (!commandResult.Success)
                throw new VaultException(string.Format("Command {0} failed because: {1}.", command, commandResult.Error));
        }

        static void VerifyProcessResult(ProcessResult processResult)
        {
            if (!processResult.Success)
                throw new ProcessRunnerException("Failed to invoke Vault Command Line Client. Process status was " +
                                         processResult.Status);

            if (processResult.ExitCode != 0)
                throw new VaultException(string.Format("Vault Client command failed, exit code {0}. All output: {1}",
                    processResult.ExitCode, processResult.AllOutput));
        }

        ProcessResult RunClientProcess(string command, IEnumerable<PrimaryArgument> primaryArguments, IEnumerable<NamedArgument> additionalArguments)
        {
            IEnumerable<string> allArguments = GetAllArguments(primaryArguments, additionalArguments);
 
            var clientProcessOptions = new ProcessOptions(commandLineClientPath) { Timeout = (int)commandTimeOut.TotalSeconds };
            clientProcessOptions.Add(command);
            clientProcessOptions.Add(allArguments);

            return Process.Run(clientProcessOptions);
        }

        IEnumerable<string> GetAllArguments(IEnumerable<PrimaryArgument> primaryArguments, IEnumerable<NamedArgument> additionalArguments)
        {
            return GetCommonArguments()
                .Concat(additionalArguments)
                .Cast<IFormattableArgument>()
                .Concat(primaryArguments)
                .SelectMany(a => a.FormatCommandLine(SingleDashArgumentPrefix, CommandLineArgumentStyle.Separated));
        }

        IEnumerable<NamedArgument> GetCommonArguments()
        {
            return new[]
            {
                CommandLineClientArgument.Host(vaultHost), CommandLineClientArgument.User(vaultCredential.UserName),
                CommandLineClientArgument.Password(vaultCredential.Password)
            };
        }
    }
}
