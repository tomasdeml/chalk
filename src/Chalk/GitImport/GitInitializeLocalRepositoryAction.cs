using Chalk.Actions;
using Chalk.GitImport.Interop;
using Chalk.Logging;
using Seterlund.CodeGuard;

namespace Chalk.GitImport
{
    public class GitInitializeLocalRepositoryAction 
    {
        const string ExporterUserName = "Vault Exporter";

        readonly ActionContext context;
        readonly IGitFacade gitFacade;

        public GitInitializeLocalRepositoryAction(ActionContext context, IGitFacade gitFacade)
        {
            Guard.That(context).IsNotNull();
            Guard.That(gitFacade).IsNotNull();
            this.context = context;
            this.gitFacade = gitFacade;
        }

        public void Execute()
        {
            var repository = gitFacade.NewRepository(context.Parameters.LocalWorkspacePath);

            if (repository.Exists())
            {
                context.Logger.LogInfo("Existing git repository found, no repository will be created");
                return;
            }
 
            context.Logger.LogInfo(Progress.Halfway, "Initializing Git repository..."); 
            repository.Initialize();

            context.Logger.LogInfo(Progress.AlmostDone, "Configuring Git repository...");
            repository.Configure(ExporterUserName, ExporterEmail);
        }

        string ExporterEmail
        {
            get { return string.Format("vault-exporter@{0}", context.Parameters.CommitAuthorEmailDomain); }
        }
    }
}