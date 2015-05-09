namespace Chalk.VaultExport
{
    public class WriteNoVersionMarkerAction
    {
        readonly ILastVersionMarker lastVersionMarker;

        public WriteNoVersionMarkerAction(ILastVersionMarker lastVersionMarker)
        {
            this.lastVersionMarker = lastVersionMarker;
        }

        public void Execute()
        {
            if (!lastVersionMarker.Exists())
                lastVersionMarker.MarkNone(); 
        }
    }
}
