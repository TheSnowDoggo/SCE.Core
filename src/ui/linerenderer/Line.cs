namespace SCE
{
    public struct Line
    {
        private static ColorSet DefaultColors { get; } = new(SCEColor.Gray, SCEColor.Transparent);

        public Line(string message, ColorSet? colors = null)
        {
            Message = message;
            Colors = colors ?? DefaultColors;
        }

        public Line(string message, SCEColor fgColor, SCEColor? bgColor = null)
            : this(message, new ColorSet(fgColor, bgColor ?? DefaultColors.BgColor))
        {
        }

        public string Message { get; set; }

        public ColorSet Colors { get; set; }

        public HorizontalAnchor Anchor { get; set; } = HorizontalAnchor.Left;

        public override string ToString()
        {
            return Message;
        }
    }
}
