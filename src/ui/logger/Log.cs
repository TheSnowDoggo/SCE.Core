namespace SCE
{
    public struct Log
    {
        private static ColorSet DefaultColors { get; } = new(SCEColor.Gray, SCEColor.Transparent);

        public Log(string message, ColorSet? colorSet = null, DateTime? logTime = null)
        {
            Message = message;
            Colors = colorSet ?? DefaultColors;
            LogTime = logTime ?? DateTime.Now;
        }

        public Log(string message, DateTime logTime)
            : this(message, null, logTime)
        {
        }

        public string Message { get; set; }

        public ColorSet Colors { get; set; }

        public DateTime LogTime { get; set; }

        public override string ToString()
        {
            return $"[{LogTime:T}] {Message}";
        }
    }
}
