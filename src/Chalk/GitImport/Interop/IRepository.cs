namespace Chalk.GitImport.Interop
{
    public interface IRepository
    {
        bool Exists();
        void Initialize();
        void Configure(string userName, string userEmail);
    }
}