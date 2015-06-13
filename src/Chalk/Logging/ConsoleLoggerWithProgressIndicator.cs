using System;

namespace Chalk.Logging
{
    class ConsoleLoggerWithProgressIndicator : ILogger
    {
        public void LogDebug(string message, params object[] messageArgs)
        {
            // NOP
        }

        public void LogInfo(string message, params object[] messageArgs)
        {
            LogInfo(null, message, messageArgs);
        }

        public void LogInfo(Progress? progress, string message, params object[] messageArgs)
        {
            WriteProgressIndicator(progress); 
            WriteMessage(message, messageArgs, ConsoleColor.White);
        }

        public void LogWarning(string message, params object[] messageArgs)
        {
            WriteMessage(message, messageArgs, ConsoleColor.Yellow); 
        }

        static void WriteProgressIndicator(Progress? progress)
        {
            if (progress == null)
                return;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("({0}) ", progress.Value);
        }

        static void WriteMessage(string message, object[] messageArgs, ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message, messageArgs); 
            Console.ResetColor();
        }
    }
}