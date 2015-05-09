using System.IO.Abstractions;

namespace Chalk.GitImport.Interop
{
    public class CommandLineClientFacade : IGitFacade
    {
        readonly CommandLineClient gitCommandLineClient;
        readonly IFileSystem fileSystem;

        public CommandLineClientFacade(CommandLineClient gitCommandLineClient, IFileSystem fileSystem)
        {
            this.gitCommandLineClient = gitCommandLineClient;
            this.fileSystem = fileSystem;
        }

        public IRepository NewRepository(string workingDirectoryPath)
        {
            return new CommandLineClientBasedRepository(gitCommandLineClient, fileSystem, workingDirectoryPath);
        }
    }
}
