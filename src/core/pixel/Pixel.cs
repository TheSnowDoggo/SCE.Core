namespace SCE
{
    /// <summary>
    /// A struct storing a <see cref="Element"/>, and a <see cref="FgColor"/> and <see cref="BgColor"/> <see cref="byte"/> color.
    /// Used in <see cref="DisplayMap"/> and derived classes to render to the <see cref="Display"/>.
    /// </summary>
    public readonly struct Pixel : IEquatable<Pixel>
    {
        public const SCEColor DEFAULT_FGCOLOR = SCEColor.Black;

        public Pixel(char element, SCEColor fgColor, SCEColor bgColor)
        {
            if (char.IsControl(element) && element != '\0')
                throw new ArgumentException("Element cannot be a control character.");

            Element = element;
            FgColor = fgColor;
            BgColor = bgColor;
        }

        public Pixel(char element, ColorSet colors)
            : this(element, colors.FgColor, colors.BgColor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pixel"/> struct given the background color.
        /// </summary>
        /// <param name="bgColor">The background color of the new instance.</param>
        public Pixel(SCEColor bgColor)
            : this(' ', DEFAULT_FGCOLOR, bgColor)
        {
        }

        public Pixel(ColorSet colors)
            : this(' ', colors.FgColor, colors.BgColor)
        {
        }

        public static Pixel Empty { get; } = new();

        public char Element { get; }

        public SCEColor FgColor { get; }

        public SCEColor BgColor { get; }

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

        public static Pixel Merge(Pixel topPixel, Pixel bottomPixel)
        {
            return new(topPixel.Element != '\0' ? topPixel.Element : bottomPixel.Element, 
                ColorUtils.GetStackColor(topPixel.FgColor, bottomPixel.FgColor), 
                ColorUtils.GetStackColor(topPixel.BgColor, bottomPixel.BgColor));
        }
    }
}
