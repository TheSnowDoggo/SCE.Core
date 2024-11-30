namespace SCECore
{
    using System.Diagnostics;
    using Seconds = Double;

    internal class Timer
    {
        private readonly Stopwatch _stopwatch = new();

        public Timer(Seconds length)
        {
            Length = length;
        }

        public Seconds Length { get; set; }

        public Seconds Remaining { get => Length - _stopwatch.Elapsed.TotalSeconds; }

        public event EventHandler? OnEndEvent;

        public static Timer GetNew(Seconds length)
        {
            Timer timer = new(length);
            timer.Start();
            return timer;
        }

        public void Start() { _stopwatch.Start(); }

        public void Stop() { _stopwatch.Stop(); }

        public void Restart() { _stopwatch.Restart(); }

        public void Update()
        {
            if (Remaining <= 0)
                OnEndEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
