using System.IO;
using System.IO.Abstractions;
using Chalk.Interop;
using Seterlund.CodeGuard;

namespace Chalk.GitImport.Interop
{
    public class CommandLineClientBasedRepository : IRepository
    {
        const string GitRepositoryDirectoryName = ".git";

        const string InitCommand = "init";
        const string ConfigCommand = "config";
        const string LocalConfigSwitchArgument = "local";
        const string AddConfigSwitchArgument = "add";

        const string UserNameConfigPath = "user.name";
        const string UserEmailConfigPath = "user.email";
        const string AutoCrLfConfigPath = "core.autocrlf";

        readonly CommandLineClient gitClient;
        readonly IFileSystem fileSystem;
        readonly string workingDirectoryPath;

        public CommandLineClientBasedRepository(CommandLineClient gitClient, IFileSystem fileSystem, string workingDirectoryPath)
        {
            this.gitClient = gitClient;
            this.fileSystem = fileSystem;
            this.workingDirectoryPath = workingDirectoryPath;
        }

        public bool Exists()
        { 
            string gitRepositoryPath = Path.Combine(workingDirectoryPath, GitRepositoryDirectoryName);
            return fileSystem.Directory.Exists(gitRepositoryPath);
        }

        public void Initialize()
        {
            gitClient.ExecuteCommand(InitCommand);
        }

        public void Configure(string userName, string userEmail)
        {
            Guard.That(userName, "userName").IsNotNullOrEmpty();
            Guard.That(userEmail, "userEmail").IsNotNullOrEmpty();

            ConfigureUserName(userName); 
            ConfigureUserEmail(userEmail); 
            ConfigureAutoCrLf();
        }

        void ConfigureAutoCrLf()
        {
            gitClient.ExecuteCommand(ConfigCommand,
                new SwitchArgument(LocalConfigSwitchArgument), new SwitchArgument(AddConfigSwitchArgument),
                new PositionalArgument(AutoCrLfConfigPath),
                new PositionalArgument(false.ToString().ToLowerInvariant()));
        }

        void ConfigureUserEmail(string userEmail)
        {
            gitClient.ExecuteCommand(ConfigCommand,
                new SwitchArgument(LocalConfigSwitchArgument), new SwitchArgument(AddConfigSwitchArgument),
                new PositionalArgument(UserEmailConfigPath),
                new PositionalArgument(userEmail));
        }

        void ConfigureUserName(string userName)
        {
            gitClient.ExecuteCommand(ConfigCommand,
                new SwitchArgument(LocalConfigSwitchArgument), new SwitchArgument(AddConfigSwitchArgument), 
                new PositionalArgument(UserNameConfigPath),
                new PositionalArgument(userName));
        }
    }
}
