using Seterlund.CodeGuard;

namespace Chalk.Logging
{
    public struct Progress
    {
        public double Percentage { get; private set; }

        public Progress(double percentage)
            : this()
        {
            Guard.That(percentage, "percentage").IsInRange(0, 100);
            Percentage = percentage;
        }

        public static Progress ForStep(int stepIndex, int totalNumberOfSteps)
        {
            return new Progress(100D / totalNumberOfSteps*(stepIndex + 1));
        }

        public static Progress Starting
        {
            get { return new Progress(0); }
        }

        public static Progress Halfway
        {
            get { return new Progress(50); }
        }

        public static Progress AlmostDone
        {
            get { return new Progress(99); }
        }

        public static Progress Finished
        {
            get { return new Progress(100); }
        }
    }
}