namespace SCECore.Types
{
    using SCECore.Utils;

    /// <summary>
    /// A struct storing a <see cref="Element"/>, and a <see cref="FgColor"/> and <see cref="BgColor"/> <see cref="byte"/> color.
    /// Used in <see cref="DisplayMap"/> and derived classes to render to the <see cref="Display"/>.
    /// </summary>
    public readonly struct Pixel : IEquatable<Pixel>
    {
        /// <summary>
        /// The global allowed string length of each <see cref="Pixel.Element"/>. Default is 2.
        /// </summary>
        public const int PIXELWIDTH = 2;

        /// <summary>
        /// The constant default <see cref="byte"/> color used in construction.
        /// </summary>
        public const Color DefaultColor = Color.Black;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pixel"/> struct given the element, foreground and background color.
        /// </summary>
        /// <param name="element">The <see cref="string"/> value of the new instance.</param>
        /// <param name="fgColor">The foreground color of the new instance.</param>
        /// <param name="bgColor">The background color of the new instance.</param>
        public Pixel(string element, Color fgColor, Color bgColor)
        {
            if (element is null or "")
            {
                element = EmptyElement;
            }

            if (element.Length != PIXELWIDTH)
            {
                throw new ArgumentException("Element is invalid.");
            }

            Element = element;
            FgColor = fgColor;
            BgColor = bgColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pixel"/> struct given the background color.
        /// </summary>
        /// <param name="bgColor">The background color of the new instance.</param>
        public Pixel(Color bgColor)
            : this(string.Empty, DefaultColor, bgColor)
        {
        }

        /// <summary>
        /// Gets the empty <see cref="string"/> element determined by the <see cref="PIXELWIDTH"/>.
        /// </summary>
        public static string EmptyElement { get; } = SCEString.Copy(' ', PIXELWIDTH);

        /// <summary>
        /// Gets the <see cref="string"/> value of this instance.
        /// </summary>
        public string Element { get; }

        /// <summary>
        /// Gets the foreground color of this instance.
        /// </summary>
        public Color FgColor { get; }

        /// <summary>
        /// Gets the <see cref="byte"/> background color of this instance.
        /// </summary>
        public Color BgColor { get; }

        // Equality operators
        public static bool operator ==(Pixel p1, Pixel p2) => Equals(p1, p2);

        public static bool operator !=(Pixel p1, Pixel p2) => !(p1 == p2);

        /// <inheritdoc/>
        public bool Equals(Pixel pixel)
        {
            return pixel.Element == Element && pixel.FgColor == FgColor && pixel.BgColor == BgColor;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is not null && Equals((Pixel)obj);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"\"{Element}\",{FgColor},{BgColor}"; 
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the minimum number of <see cref="Pixel"/> required to store the specified number of characters.
        /// </summary>
        /// <param name="characters">The number of characters.</param>
        /// <returns>The minimum number of <see cref="Pixel"/> required to store the specified number of <paramref name="characters"/>.</returns>
        public static int GetPixelLength(int characters)
        {
            return (int)Math.Ceiling((double)characters / PIXELWIDTH);
        }

        /// <summary>
        /// Returns the minimum number of <see cref="Pixel"/> required to store the specified string.
        /// </summary>
        /// <param name="str">The string to get from.</param>
        /// <returns>The minimum number of <see cref="Pixel"/> required to store the specified string.</returns>
        public static int GetPixelLength(string str)
        {
            return GetPixelLength(str.Length);
        }
    }
}
