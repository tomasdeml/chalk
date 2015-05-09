namespace Chalk.Logging
{
    public interface ILogger
    {
        void LogInfo(string message, params object[] messageArgs); 
        void LogInfo(Progress? progress, string message, params object[] messageArgs);
    }
}