using System;
using System.Diagnostics.CodeAnalysis;
using PowerArgs;

namespace Chalk.Actions
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Parameters
    {
        [ArgShortcut("a")]
        [ArgRequired]
        public ActionKind Action { get; set; }

        [ArgShortcut("wp")]
        [ArgDefaultValue(@".\")]
        public string LocalWorkspacePath { get; set; }

        [ArgShortcut("vcp")]
        [DefaultValue("Vault.exe")]
        public string VaultCommandLineClientPath { get; set; }

        [ArgShortcut("vst")]
        [ArgDefaultValue(0)]
        [ArgRange(0, Int32.MaxValue)]
        public int VaultServerTimeOutInSeconds { get; set; }

        [ArgShortcut("vh")]
        public string VaultHost { get; set; }

        [ArgShortcut("vu")]
        public string VaultUserName { get; set; }

        [ArgShortcut("vp")]
        public string VaultPassword { get; set; }

        [ArgShortcut("vrn")]
        public string VaultRepositoryName { get; set; }

        [ArgShortcut("vrp")]
        public string VaultRepositoryPath { get; set; }

        [ArgShortcut("ddd")]
        public bool DisableRepositoryDeletionDetection { get; set; }

        [ArgShortcut("aed")]
        public string CommitAuthorEmailDomain { get; set; }
    }
}
