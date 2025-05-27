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
        public ColorSet(SCEColor fgColor, SCEColor bgColor)
        {
            FgColor = fgColor;
            BgColor = bgColor;
        }

        public static ColorSet Zero { get; } = new(SCEColor.Black, SCEColor.Black);

        public SCEColor FgColor { get; }

        public SCEColor BgColor { get; }

        public static implicit operator ColorSet(Pixel pixel) => new(pixel.FgColor, pixel.BgColor);

        public static bool operator ==(ColorSet left, ColorSet right) => left.Equals(right);

        public static bool operator !=(ColorSet left, ColorSet right) => !(left == right);

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
            return $"ColorSet({FgColor}, {BgColor})";
        }

        /// <summary>
        /// Exposes the foreground and background color properties in this instance.
        /// </summary>
        /// <param name="fgColor">The foregrorund <see cref="byte"/> color.</param>
        /// <param name="bgColor">The backgrround <see cref="byte"/> color.</param>
        public void Expose(out SCEColor fgColor, out SCEColor bgColor)
        {
            fgColor = FgColor;
            bgColor = BgColor;
        }
    }
}
