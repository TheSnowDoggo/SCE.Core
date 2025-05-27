namespace SCE
{
    public class RateLimiter : IUpdateLimit
    {
        public enum RateType
        {
            FramesPerSecond,
            SecondsPerFrame,
        }

        private double timePassed;

        public RateLimiter(double rate, bool updateOnFirstFrame = true)
        {
            Rate = rate;
            timePassed = updateOnFirstFrame ? 0.0 : TimePerUpdate;
        }

        public double Rate { get; set; }

        public RateType RateMode { get; set; } = RateType.FramesPerSecond;

        private double TimePerUpdate { get => RateMode == RateType.FramesPerSecond ? 1.0 / Rate : Rate; }

        public bool OnUpdate()
        {
            timePassed -= GameHandler.DeltaTime;
            bool update = timePassed <= 0;
            if (update)
            {
                timePassed = TimePerUpdate;
            }
            return update;
        }
    }
}
