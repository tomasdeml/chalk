namespace Chalk.Logging
{
    public interface ILogger
    {
        void LogDebug(string message, params object[] messageArgs);

        void LogInfo(string message, params object[] messageArgs); 
        void LogInfo(Progress? progress, string message, params object[] messageArgs);

        void LogWarning(string message, params object[] messageArgs);
    }
}