using System.IO.Abstractions;
using Chalk.Actions;
using Seterlund.CodeGuard;

namespace Chalk.VaultExport
{
    public class PrepareWorkspaceAction
    {
        readonly ActionContext context;
        readonly IFileSystem fileSystem;

        public PrepareWorkspaceAction(ActionContext context, IFileSystem fileSystem)
        {
            Guard.That(context).IsNotNull();
            Guard.That(fileSystem).IsNotNull();
            this.context = context;
            this.fileSystem = fileSystem;
        }

        public void Execute()
        {
            if (fileSystem.Directory.Exists(context.Parameters.LocalWorkspacePath))
                return;

            context.Logger.LogInfo("Creating workspace {0}...", context.Parameters.LocalWorkspacePath);
            fileSystem.Directory.CreateDirectory(context.Parameters.LocalWorkspacePath);
        } 
    }
}
