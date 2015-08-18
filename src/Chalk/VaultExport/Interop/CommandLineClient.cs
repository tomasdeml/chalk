using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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

        public void ExecuteCommand(string command, IEnumerable<PositionalArgument> primaryArguments, params IArgument[] additionalArguments)
        {
            ExecuteCommandAndVerifyResult(command, primaryArguments, additionalArguments);
        }

        ProcessResult ExecuteCommandAndVerifyResult(string command, IEnumerable<PositionalArgument> primaryArguments, IArgument[] additionalArguments)
        {
            var commandResult = RunClientProcess(command, primaryArguments, additionalArguments);
            VerifyProcessResult(commandResult);
            VerifyClientCommandResult(commandResult, command);

            return commandResult;
        }

        public TOutput ExecuteCommand<TOutput>(string command, PositionalArgument primaryArguments, params IArgument[] additionalArguments)
        {
            ProcessResult processResult = ExecuteCommandAndVerifyResult(command, new[] {primaryArguments}, additionalArguments);

            var commandOutputSerializer = new YAXSerializer(typeof(TOutput), YAXExceptionHandlingPolicies.ThrowWarningsAndErrors);
            var validStandardOuputText = RemoveUnsupportedCharacters(processResult.StandardOutput);
            var commandOutput = commandOutputSerializer.Deserialize(validStandardOuputText);

            return (TOutput)commandOutput;
        }

        static void VerifyClientCommandResult(ProcessResult processResult, string command)
        {
            var validStandardOuputText = RemoveUnsupportedCharacters(processResult.StandardOutput);

            var commandResult = (CommandLineClientResult) CommandResultSerializer.Deserialize(validStandardOuputText);

            if (!commandResult.Success)
                throw new VaultException(string.Format("Command {0} failed because: {1}.", command, commandResult.Error));
        }

        /// <summary>
        /// Removes unsupported characters, such as quotation marks.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        static string RemoveUnsupportedCharacters(string text)
        {
            var resultBuilder = new StringBuilder(text.Length);

            var regex = new Regex("comment=\"(.*)\" objverid=");
            var matches = regex.Matches(text);

            int lastProcessedIndex = 0;

            foreach (Match match in matches)
            {
                var matchGroup1 = match.Groups[1];

                resultBuilder.Append(text.Substring(lastProcessedIndex, matchGroup1.Index - lastProcessedIndex));
                resultBuilder.Append(matchGroup1.Value.Replace("\"", "''"));
                lastProcessedIndex = matchGroup1.Index + matchGroup1.Length;
            }

            resultBuilder.Append(text.Substring(lastProcessedIndex));

            return resultBuilder.ToString();
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

        ProcessResult RunClientProcess(string command, IEnumerable<PositionalArgument> primaryArguments, IArgument[] additionalArguments)
        {
            IEnumerable<string> allArguments = GetAllArguments(primaryArguments, additionalArguments);
 
            var clientProcessOptions = new ProcessOptions(commandLineClientPath) { Timeout = (int)commandTimeOut.TotalSeconds };
            clientProcessOptions.Add(command);
            clientProcessOptions.Add(allArguments);

            return Process.Run(clientProcessOptions);
        }

        IEnumerable<string> GetAllArguments(IEnumerable<PositionalArgument> primaryArguments, IEnumerable<IArgument> additionalArguments)
        {
            return GetCommonArguments()
                .Concat(additionalArguments)
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
