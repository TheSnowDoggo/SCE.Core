namespace SCECore.Objects
{
    /// <summary>
    /// A struct storing a <see cref="FgColor"/> and <see cref="BgColor"/> <see cref="byte"/> color.
    /// Used by <see cref="Display"/> when CCS rendering.
    /// </summary>
    public readonly struct ColorSet : IEquatable<ColorSet>
    {
        private readonly byte fgColor;

        private readonly byte bgColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSet"/> struct.
        /// </summary>
        /// <param name="fgColor">The foreground <see cref="byte"/> color.</param>
        /// <param name="bgColor">The background <see cref="byte"/> color.</param>
        public ColorSet(byte fgColor, byte bgColor)
        {
            this.fgColor = Color.ValidSet(fgColor);
            this.bgColor = Color.ValidSet(bgColor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSet"/> struct.
        /// </summary>
        /// <remarks>
        /// <i>Note: The <paramref name="pixel"/> <see cref="Pixel.Element"/> property is not used.</i>
        /// </remarks>
        /// <param name="pixel">The pixel to grab the <see cref="Pixel.FgColor"/> and <see cref="BgColor"/> color from.</param>
        public ColorSet(Pixel pixel)
        {
            fgColor = Color.ValidSet(pixel.FgColor);
            bgColor = Color.ValidSet(pixel.BgColor);
        }

        /// <summary>
        /// Gets the foreground <see cref="byte"/> color.
        /// </summary>
        public byte FgColor => fgColor;

        /// <summary>
        /// Gets the background <see cref="byte"/> color.
        /// </summary>
        public byte BgColor => bgColor;

        /// <summary>
        /// Exposes the foreground and background color properties in this instance.
        /// </summary>
        /// <param name="fgColor">The foregrorund <see cref="byte"/> color.</param>
        /// <param name="bgColor">The backgrround <see cref="byte"/> color.</param>
        public void Expose(out byte fgColor, out byte bgColor)
        {
            fgColor = FgColor;
            bgColor = BgColor;
        }
 
        /// <inheritdoc/>
        public bool Equals(ColorSet colorSet) => colorSet.FgColor == FgColor && colorSet.BgColor == BgColor;

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals((ColorSet?)obj);

        /// <inheritdoc/>
        public override int GetHashCode() => base.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => $"{FgColor},{BgColor}";
    }
}
