using System;

namespace Chalk.Logging
{
    class ConsoleLoggerWithProgressIndicator : ILogger
    {
        public void LogInfo(string message, params object[] messageArgs)
        {
            LogInfo(null, message, messageArgs);
        }

        public void LogInfo(Progress? progress, string message, params object[] messageArgs)
        {
            WriteProgressIndicator(progress); 
            WriteMessage(message, messageArgs);

            Console.ResetColor();
        } 

        static void WriteProgressIndicator(Progress? progress)
        {
            if (progress == null)
                return;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("({0:0}%) ", progress.Value.Percentage);
        }

        static void WriteMessage(string message, object[] messageArgs)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message, messageArgs);
        }
    }
}