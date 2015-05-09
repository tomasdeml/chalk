namespace Chalk.VaultExport.Interop
{
    public class VaultFacade
    {
        readonly IVaultClient vaultClient;

        public VaultFacade(IVaultClient vaultClient)
        {
            this.vaultClient = vaultClient;
        }

        public VersionHistory NewVersionHistory(string repositoryName, string repositoryPath)
        {
           return new VersionHistory(vaultClient, repositoryName, repositoryPath); 
        }

        public VersionSnapshot NewVersionSnapshot(string repositoryName, string repositoryPath)
        {
            return new VersionSnapshot(vaultClient, repositoryName, repositoryPath); 
        }

        public RepositoryDeletionHistory NewRepositoryDeletionHistory(string repositoryName, string repositoryPath)
        {
            return new RepositoryDeletionHistory(vaultClient, repositoryName, repositoryPath);
        }
    }
}
