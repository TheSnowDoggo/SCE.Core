namespace SCE
{
    public struct MsgLine
    {
        private static ColorSet DefaultColors { get; } = new(SCEColor.Gray, SCEColor.Transparent);

        public MsgLine(string message, ColorSet? colors = null)
        {
            Message = message;
            Colors = colors ?? DefaultColors;
        }

        public MsgLine(string message, SCEColor fgColor, SCEColor? bgColor = null)
            : this(message, new ColorSet(fgColor, bgColor ?? DefaultColors.BgColor))
        {
        }

        public string Message { get; set; }

        public ColorSet Colors { get; set; }

        public Anchor Anchor { get; set; } = Anchor.None;

        /// <inheritdoc/>
        public override string ToString()
        {
            return Message;
        }
    }
}
