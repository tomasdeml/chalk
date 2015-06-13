using Chalk.Logging;
using Seterlund.CodeGuard;

namespace Chalk.Actions
{
    public class ActionContext 
    {
        public ILogger Logger { get; private set; }

        public Parameters Parameters { get; private set; } 

        public ActionContext(Parameters parameters, ILogger logger)
        {
            Guard.That(parameters).IsNotNull();
            Guard.That(logger).IsNotNull(); 

            parameters.Validate();

            Parameters = parameters;
            Logger = logger; 
        } 
    }
}