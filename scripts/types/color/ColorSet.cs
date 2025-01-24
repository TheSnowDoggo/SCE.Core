namespace SCE
{
    /// <summary>
    /// A struct storing a foreground and background color.
    /// </summary>
    public readonly struct ColorSet : IEquatable<ColorSet>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSet"/> struct.
        /// </summary>
        /// <param name="fgColor">The foreground <see cref="byte"/> color.</param>
        /// <param name="bgColor">The background <see cref="byte"/> color.</param>
        public ColorSet(Color fgColor, Color bgColor)
        {
            FgColor = fgColor;
            BgColor = bgColor;
        }

        public Color FgColor { get; }

        public Color BgColor { get; }

        // Conversion operators
        public static implicit operator ColorSet(Pixel pixel) => new(pixel.FgColor, pixel.BgColor);

        // Equality operators
        public static bool operator ==(ColorSet left, ColorSet right) => left.Equals(right);

        public static bool operator !=(ColorSet left, ColorSet right) => !(left == right);

        /// <summary>
        /// Exposes the foreground and background color properties in this instance.
        /// </summary>
        /// <param name="fgColor">The foregrorund <see cref="byte"/> color.</param>
        /// <param name="bgColor">The backgrround <see cref="byte"/> color.</param>
        public void Expose(out Color fgColor, out Color bgColor)
        {
            fgColor = FgColor;
            bgColor = BgColor;
        }
 
        public bool Equals(ColorSet colorSet)
        {
            return colorSet.FgColor == FgColor && colorSet.BgColor == BgColor;
        }

        public override bool Equals(object? obj)
        {
            return obj is not null && Equals((ColorSet)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FgColor, BgColor);
        }

        public override string ToString()
        {
            return $"ColorSet({FgColor},{BgColor})";
        }
    }
}
