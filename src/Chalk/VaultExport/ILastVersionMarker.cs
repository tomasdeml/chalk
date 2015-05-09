namespace Chalk.VaultExport
{
    public interface ILastVersionMarker
    {
        int? GetNext();

        void Mark(int version);
        void MarkNone();

        bool Exists();
    }
}