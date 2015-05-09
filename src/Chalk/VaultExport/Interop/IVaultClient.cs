using System.Collections.Generic;
using Chalk.Interop;

namespace Chalk.VaultExport.Interop
{
    public interface IVaultClient
    {
        void ExecuteCommand(string command, IEnumerable<PrimaryArgument> primaryArguments, params NamedArgument[] additionalArguments);

        TOutput ExecuteCommand<TOutput>(string command, PrimaryArgument primaryArgument, params NamedArgument[] additionalArguments); 
    }
}
