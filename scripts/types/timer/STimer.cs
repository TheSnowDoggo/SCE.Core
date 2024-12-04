namespace SCE
{
    using System.Diagnostics;

    public class STimer
    {
        private readonly Stopwatch _stopwatch = new();

        public STimer(double lengthSeconds)
        {
            Length = lengthSeconds;
        }

        public STimer(long lengthMilliseconds)
        {
            Length = (long)(lengthMilliseconds / 1000);
        }

        public double Length { get; set; }

        public TimeSpan Elapsed { get => _stopwatch.Elapsed; }

        public long ElapsedMilliseconds { get => _stopwatch.ElapsedMilliseconds; }

        public long RemainingMilliseconds { get => (long)Length - _stopwatch.ElapsedMilliseconds; }

        public double RemainingSeconds { get => Length - _stopwatch.Elapsed.TotalSeconds; }

        public bool IsRunning { get => _stopwatch.IsRunning; }

        public event EventHandler? OnEndEvent;

        public static STimer GetNew(double length)
        {
            STimer timer = new(length);
            timer.Start();
            return timer;
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public void Reset()
        {
            _stopwatch.Reset();
        }

        public void Restart()
        {
            _stopwatch.Restart();
        }

        public void Update()
        {
            if (RemainingSeconds <= 0)
                OnEndEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
