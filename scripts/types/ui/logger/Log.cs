namespace SCE
{
    public class Log : Line
    {
        public Log(string message, ColorSet? colorSet = null, DateTime? logTime = null)
            : base(message, colorSet)
        {
            LogTime = logTime ?? DateTime.Now;
        }

        public Log(string message, DateTime logTime)
            : this(message, null, logTime)
        {
        }

        public DateTime LogTime { get; set; }

        public override string ToString()
        {
            return $"[{LogTime:T}] {Message}";
        }
    }
}
