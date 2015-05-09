using YAXLib;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Chalk.VaultExport.Interop
{
    sealed class CommandLineClientResult
    {
        [YAXElementFor("result")]
        [YAXSerializeAs("success")]
        public bool Success { get; set; }

        [YAXSerializeAs("error")]
        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public string Error { get; set; } 
    }
}