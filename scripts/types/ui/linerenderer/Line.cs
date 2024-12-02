namespace SCE
{
    public readonly struct Line
    {
        private const Color DefaultFgColor = Color.White;
        private const Color DefaultBgColor = Color.Black;

        public Line(string data, Color fgColor, Color bgColor)
        {
            Data = data;
            FgColor = fgColor;
            BgColor = bgColor;
        }

        public Line(string data, Color fgColor)
            : this(data, fgColor, DefaultBgColor)
        {
        }

        public Line(string data)
            : this(data, DefaultFgColor)
        {
        }

        public string Data { get; }

        public Color FgColor { get; }

        public Color BgColor { get; }
    }
}
