using System;
using Chalk.Actions;
using Chalk.Logging;
using PowerArgs;

namespace Chalk
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            ILogger logger = new ConsoleLoggerWithProgressIndicator(); 

            try
            {
                var parameters = Args.Parse<Parameters>(args);
                if (parameters == null)
                    return; 
 
                var context = new ActionContext(parameters, logger);
                var action = new ActionComposer().Compose(parameters.Action, context); 
                var timedAction = new TimedAction(action);
                timedAction.Execute();

                Console.WriteLine("Completed, action took {0}", timedAction.Duration);
            }
            catch (Exception e)
            {
                logger.LogInfo("An error occured: {0}", e);
                throw;
            }
        }
    }
}
