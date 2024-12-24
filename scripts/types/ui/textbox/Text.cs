namespace SCE
{
    public class Text : IEquatable<Text>, ICloneable
    {
        public Text(string data)
        {
            Data = data;
        }

        public Text()
            : this(string.Empty)
        {
        }

        public string Data { get; set; }

        public Color FgColor { get; set; } = Color.White;

        public Color BgColor { get; set; } = Color.Transparent;

        public Anchor Anchor { get; set; }

        /// <summary>
        /// Renders a new line when the text overflows its container's width.
        /// This does not modify the text data
        /// </summary>
        public bool NewLineOverflow { get; set; } = false;

        // Equality
        public static bool operator ==(Text? t1, Text? t2) => t1 is not null && t1.Equals(t2);

        public static bool operator !=(Text? t1, Text? t2) => !(t1 == t2);

        /// <inheritdoc/>
        public object Clone()
        {
            Text clone = new(Data)
            {
                FgColor = FgColor,
                BgColor = BgColor,
                Anchor = Anchor,
                NewLineOverflow = NewLineOverflow,
            };

            return clone;
        }

        /// <summary>
        /// Determines whether the specified text is equal to the current text.
        /// </summary>
        /// <param name="other">The text to compare with the current text.</param>
        /// <returns><see langword="true"/> if the specified text is equal to the current text; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Text? other)
        {
            return other is not null && other.Data == Data && other.FgColor == FgColor && other.BgColor == BgColor && other.Anchor == Anchor && other.NewLineOverflow == NewLineOverflow;
        }

        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            return other is Text text && Equals(text);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Data, FgColor, BgColor, Anchor, NewLineOverflow);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Data;
        }

        public int GetStartMapX(int stringLength, int width)
        {
            switch (Anchor)
            {
                case Anchor.BottomLeft:
                case Anchor.MiddleLeft:
                case Anchor.TopLeft: 
                    return 0;
                case Anchor.BottomCenter:
                case Anchor.MiddleCenter:
                case Anchor.TopCenter:
                    return (width - Pixel.GetPixelLength(stringLength)) / 2;
                case Anchor.BottomRight:
                case Anchor.MiddleRight:
                case Anchor.TopRight:
                    return width - Pixel.GetPixelLength(stringLength);
            }
            throw new NotImplementedException("Unknown alignment.");
        }

        public int GetStartMapY(int rows, int height)
        {
            switch (Anchor)
            {
                case Anchor.BottomLeft:
                case Anchor.BottomCenter:
                case Anchor.BottomRight:
                    return height - rows;              
                case Anchor.MiddleLeft:
                case Anchor.MiddleCenter:
                case Anchor.MiddleRight:
                    return (height - rows) / 2;
                case Anchor.TopLeft:
                case Anchor.TopCenter:
                case Anchor.TopRight:
                    return 0;
            }
            throw new NotImplementedException("Unknown alignment.");
        }

        public string GetFormattedBody(string str)
        {
            switch (Anchor)
            {
                case Anchor.BottomLeft:
                case Anchor.MiddleLeft:
                case Anchor.TopLeft:
                    return str;
                case Anchor.BottomCenter:
                case Anchor.MiddleCenter:
                case Anchor.TopCenter:
                    return StringUtils.PadBeforeToEven(str);
                case Anchor.BottomRight:
                case Anchor.MiddleRight:
                case Anchor.TopRight:
                    return StringUtils.PadBeforeToEven(str);
            }
            throw new NotImplementedException("Unknown alignment.");
        }
    }
}
