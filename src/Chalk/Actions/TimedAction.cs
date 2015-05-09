using System;
using Seterlund.CodeGuard;

namespace Chalk.Actions
{
    class TimedAction
    {
        readonly Action actionToTime;

        public TimeSpan Duration { get; private set; }

        public TimedAction(Action actionToTime)
        {
            Guard.That(actionToTime).IsNotNull();
            this.actionToTime = actionToTime;
        }

        public void Execute()
        {
            var startTime = DateTime.Now;
            actionToTime();
            Duration = (DateTime.Now - startTime);
        } 
    }
}
