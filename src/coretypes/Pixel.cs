namespace SCE
{
    /// <summary>
    /// A struct storing a <see cref="Element"/>, and a <see cref="FgColor"/> and <see cref="BgColor"/> <see cref="byte"/> color.
    /// Used in <see cref="DisplayMap"/> and derived classes to render to the <see cref="Display"/>.
    /// </summary>
    public readonly struct Pixel : IEquatable<Pixel>
    {
        public const SCEColor DEFAULT_FGCOLOR = SCEColor.Gray;

        public const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        private readonly char element = ' ';

        public Pixel(char element, SCEColor fgColor, SCEColor bgColor)
        {
            this.element = element;
            FgColor = fgColor;
            BgColor = bgColor;
        }

        public Pixel(char element, ColorSet colors)
            : this(element, colors.FgColor, colors.BgColor)
        {
        }

        public Pixel(SCEColor bgColor)
        {
            BgColor = bgColor;
        }

        public Pixel(ColorSet colors)
        {
            FgColor = colors.FgColor;
            BgColor = colors.BgColor;
        }

        public static Pixel Empty { get; } = new();

        public char Element
        {
            get => element;
            init
            {
                if (char.IsControl(element) && element != '\0')
                {
                    throw new ArgumentException("Element cannot be a control character.");
                }
                element = value;
            }
        }

        public SCEColor FgColor { get; init; } = DEFAULT_FGCOLOR;

        public SCEColor BgColor { get; init; } = DEFAULT_BGCOLOR;

        public ColorSet ColorSet() { return new(FgColor, BgColor); }

        public char RenderElement()
        {
            return Element != '\0' ? Element : ' ';
        }

        #region Equality

        public static bool operator ==(Pixel p1, Pixel p2) => Equals(p1, p2);

        public static bool operator !=(Pixel p1, Pixel p2) => !(p1 == p2);
      
        public bool Equals(Pixel pixel)
        {
            return pixel.Element == Element && pixel.FgColor == FgColor && pixel.BgColor == BgColor;
        }

        public override bool Equals(object? obj)
        {
            return obj is not null && Equals((Pixel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Element, FgColor, BgColor);
        }

        #endregion

        public override string ToString()
        {
            return $"\"{Element}\",{FgColor},{BgColor}";
        }

        public static Pixel Merge(Pixel top, Pixel bottom)
        {
            return new()
            {
                Element = top.Element != '\0' ? top.Element : bottom.Element,
                FgColor = ColorUtils.GetStackColor(top.FgColor, bottom.FgColor),
                BgColor = ColorUtils.GetStackColor(top.BgColor, bottom.BgColor),
            };
        }
    }
}
