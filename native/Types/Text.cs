namespace SCECore.Types
{
    public class Text : IEquatable<Text>, ICloneable
    {
        private const bool DefaultNewLineOverflow = false;

        private const AlignLock DefaultAlignment = AlignLock.TopLeft;

        private const Color DefaultFgColor = Color.White;
        private const Color DefaultBgColor = Color.Transparent;

        public Text(string data, Color fgColor, Color bgColor, AlignLock alignment = DefaultAlignment, bool newLineOverflow = DefaultNewLineOverflow)
        {
            Data = data;
            FgColor = fgColor;
            BgColor = bgColor;

            Alignment = alignment;

            NewLineOverflow = newLineOverflow;
        }

        public Text(string data, Color fgColor, AlignLock alignment = DefaultAlignment, bool newLineOverflow = DefaultNewLineOverflow)
            : this(data, fgColor, DefaultBgColor, alignment, newLineOverflow)
        {
        }

        public Text(string data, AlignLock alignment = DefaultAlignment, bool newLineOverflow = DefaultNewLineOverflow)
            : this(data, DefaultFgColor, alignment, newLineOverflow)
        {
        }

        public Text(Color fgColor, Color bgColor, AlignLock alignment = DefaultAlignment, bool newLineOverflow = DefaultNewLineOverflow)
            : this(string.Empty, fgColor, bgColor, alignment, newLineOverflow)
        {
        }

        public Text(Color fgColor, AlignLock alignment = DefaultAlignment, bool newLineOverflow = DefaultNewLineOverflow)
            : this(fgColor, DefaultBgColor, alignment, newLineOverflow)
        {
        }

        public Text(AlignLock alignment = DefaultAlignment, bool newLineOverflow = false)
            : this(DefaultFgColor, alignment, newLineOverflow)
        {
        }

        // AlignLock
        public enum AlignLock : byte
        {
            TopLeft,
            TopCenter,
            TopRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }

        public string Data { get; set; }

        public Color FgColor { get; set; }

        public Color BgColor { get; set; }

        public AlignLock Alignment { get; set; }

        /// <summary>
        /// Renders a new line when the text overflows its container's width.
        /// This does not modify the text data
        /// </summary>
        public bool NewLineOverflow { get; set; } = false;

        private byte HorizontalAlign
        {
            get
            {
                byte byteAlign = (byte)Alignment;

                if (byteAlign % 3 == 0)
                {
                    return 0;
                }

                if ((byteAlign - 1) % 3 == 0)
                {
                    return 1;
                }

                if ((byteAlign - 2) % 3 == 0)
                {
                    return 2;
                }

                throw new NotImplementedException();
            }
        }

        private byte VerticalAlign
        {
            get
            {
                byte byteAlign = (byte)Alignment;

                if (byteAlign is >= 0 and < 3)
                {
                    return 0;
                }

                if (byteAlign is >= 3 and < 6)
                {
                    return 1;
                }

                if (byteAlign is >= 6 and < 9)
                {
                    return 2;
                }

                throw new NotImplementedException();
            }
        }

        // Equality
        public static bool operator ==(Text? t1, Text? t2) => t1 is not null && t1.Equals(t2);

        public static bool operator !=(Text? t1, Text? t2) => !(t1 == t2);

        /// <inheritdoc/>
        public object Clone()
        {
            return new Text(Data, FgColor, BgColor, Alignment, NewLineOverflow);
        }

        /// <summary>
        /// Determines whether the specified text is equal to the current text.
        /// </summary>
        /// <param name="other">The text to compare with the current text.</param>
        /// <returns><see langword="true"/> if the specified text is equal to the current text; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Text? other)
        {
            return other is not null && other.Data == Data && other.FgColor == FgColor && other.BgColor == BgColor && other.Alignment == Alignment && other.NewLineOverflow == NewLineOverflow;
        }

        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            return other is Text text && Equals(text);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Data, FgColor, BgColor, Alignment, NewLineOverflow);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Data;
        }

        public int GetStartMapX(int stringLength, int width)
        {
            return HorizontalAlign switch
            {
                0 => 0,
                1 => (width - Pixel.GetPixelLength(stringLength)) / 2,
                2 => width - Pixel.GetPixelLength(stringLength),
                _ => throw new NotImplementedException()
            };
        }

        public int GetStartMapY(int rows, int height)
        {
            return VerticalAlign switch
            {
                0 => 0,
                1 => (height - rows) / 2,
                2 => height - rows,
                _ => throw new NotImplementedException()
            };
        }

        public string GetFormattedBody(string str)
        {
            return HorizontalAlign switch
            {
                0 => str,
                1 => str,
                2 => SCEString.PadBeforeToEven(str),
                _ => throw new NotImplementedException()
            };
        }
    }
}
