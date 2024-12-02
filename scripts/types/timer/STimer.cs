namespace SCE
{
    using System.Diagnostics;
    using Seconds = Double;

    public class STimer
    {
        private readonly Stopwatch _stopwatch = new();

        public STimer(Seconds length)
        {
            Length = length;
        }

        public Seconds Length { get; set; }

        public Seconds Remaining { get => Length - _stopwatch.Elapsed.TotalSeconds; }

        public event EventHandler? OnEndEvent;

        public static STimer GetNew(Seconds length)
        {
            STimer timer = new(length);
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
