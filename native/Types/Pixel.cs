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
        public const byte DefaultColor = Color.Black;

        private readonly string element;

        private readonly byte fgColor;

        private readonly byte bgColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pixel"/> struct given the element, foreground and background color.
        /// </summary>
        /// <param name="element">The <see cref="string"/> value of the new instance.</param>
        /// <param name="fgColor">The foreground color of the new instance.</param>
        /// <param name="bgColor">The background color of the new instance.</param>
        public Pixel(string element, byte fgColor, byte bgColor)
        {
            this.element = ValidSetElement(element);
            this.fgColor = Color.ValidSet(fgColor);
            this.bgColor = Color.ValidSet(bgColor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pixel"/> struct given the background color.
        /// </summary>
        /// <param name="bgColor">The background color of the new instance.</param>
        public Pixel(byte bgColor)
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
        public string Element => element;

        /// <summary>
        /// Gets the <see cref="byte"/> foreground color of this instance.
        /// </summary>
        public byte FgColor => fgColor;

        /// <summary>
        /// Gets the <see cref="byte"/> background color of this instance.
        /// </summary>
        public byte BgColor => bgColor;

        private static Exception ElementLengthException => new("Element length is not valid");

        // Equals operators
        public static bool operator ==(Pixel p1, Pixel p2) => Equals(p1, p2);

        public static bool operator !=(Pixel p1, Pixel p2) => !(p1 == p2);

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"\"{Element}\",{FgColor},{BgColor}"; 
        }

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
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the number of <see cref="Pixel"/> the <paramref name="str"/> will occupy.
        /// </summary>
        /// <param name="str">The string to find the <see cref="Pixel"/> length of.</param>
        /// <returns>The number of <see cref="Pixel"/> the <paramref name="str"/> will occupy.</returns>
        public static int GetPixelLength(string str)
        {
            return (int)Math.Ceiling((double)str.Length / PIXELWIDTH);
        }

        private static string ValidSetElement(string element)
        {
            if (element is null or "")
            {
                element = EmptyElement;
            }
            return element.Length == PIXELWIDTH ? element : throw ElementLengthException;
        }
    }
}
