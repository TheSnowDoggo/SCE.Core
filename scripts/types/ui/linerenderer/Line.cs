namespace SCE
{
    public readonly struct Line
    {
        private const Color DEFAULT_FG_COLOR = Color.White;
        private const Color DEFAULT_BG_COLOR = Color.Transparent;

        public Line(string data, Color fgColor = DEFAULT_FG_COLOR, Color bgColor = DEFAULT_BG_COLOR)
        {
            Data = data;
            FgColor = fgColor;
            BgColor = bgColor;
        }

        public string Data { get; }

        public Color FgColor { get; }

        public Color BgColor { get; }
    }
}
