using System.Collections.Generic;
using Chalk.Interop;

namespace Chalk.VaultExport.Interop
{
    public interface IVaultClient
    {
        void ExecuteCommand(string command, IEnumerable<PositionalArgument> primaryArguments, params IArgument[] additionalArguments);

        TOutput ExecuteCommand<TOutput>(string command, PositionalArgument primaryArgument, params IArgument[] additionalArguments); 
    }
}
