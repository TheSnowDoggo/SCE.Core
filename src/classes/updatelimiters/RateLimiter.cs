namespace SCE
{
    public class RateLimiter : IUpdateLimit
    {
        public enum RateType
        {
            FramesPerSecond,
            SecondsPerFrame,
        }

        private const RateType DEFAULT_RATEMODE = RateType.FramesPerSecond;

        private double timePassed;

        public RateLimiter(double rate, RateType rateType = DEFAULT_RATEMODE, bool updateOnFirstFrame = true)
        {
            Rate = rate;
            RateMode = rateType;
            timePassed = updateOnFirstFrame ? 0.0 : TimePerUpdate;
        }

        public RateLimiter(double rate, bool updateOnFirstFrame)
            : this(rate, DEFAULT_RATEMODE, updateOnFirstFrame)
        {
        }

        public double Rate { get; set; } 

        public RateType RateMode { get; set; }

        private double TimePerUpdate { get => RateMode == RateType.FramesPerSecond ? 1.0 / Rate : Rate; }

        public bool OnUpdate()
        {
            timePassed -= GameHandler.DeltaTime;
            bool update = timePassed <= 0;
            if (update)
                timePassed = TimePerUpdate;
            return update;
        }
    }
}
