namespace Chalk.GitImport.Interop
{
    public interface IGitFacade
    {
        IRepository NewRepository(string workingDirectoryPath);
    }
}