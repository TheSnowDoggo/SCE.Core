namespace SCE
{
    public class Line
    {
        protected static ColorSet DEFAULT_COLORS { get; } = new(Color.Gray, Color.Transparent);

        public Line(string message, ColorSet? colors = null)
        {
            Message = message;
            Colors = colors ?? DEFAULT_COLORS;
        }

        public Line(string message, Color fgColor, Color? bgColor = null)
            : this(message, new ColorSet(fgColor, bgColor ?? DEFAULT_COLORS.BgColor))
        {
        }

        public string Message { get; set; }

        public ColorSet Colors { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
