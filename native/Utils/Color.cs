namespace SCECore.Utils
{
    /// <summary>
    /// SCEColor (<see cref="byte"/> color) managment class.
    /// </summary>
    public static class Color
    {
        public const byte Black = 0, DarkBlue = 1, DarkGreen = 2, DarkCyan = 3, DarkRed = 4, DarkMagenta = 5, DarkYellow = 6, Gray = 7, DarkGray = 8, Blue = 9, Green = 10, Cyan = 11, Red = 12, Magenta = 13, Yellow = 14, White = 15, Transparent = 16;

        /// <summary>
        /// Gets the top exclusive full range of all allowed <see cref="byte"/> color codes.
        /// </summary>
        public static Vector2Int FullRange { get; } = new(0, 17);

        /// <summary>
        /// Gets the top exclusive range of all valid <see cref="ConsoleColor"/> <see cref="byte"/> color codes.
        /// </summary>
        public static Vector2Int RealRange { get; } = new(0, 16);

        /// <summary>
        /// Gets the exception thrown when an invalid <see cref="byte"/> color code is given.
        /// </summary>
        private static Exception InvalidColorCodeException => new("Value is not a valid color code");

        /// <summary>
        /// Writes the text representation of the given type <typeparamref name="T"/> to the standard output stream with the given foreground and background colors.
        /// </summary>
        /// <remarks>
        /// <I>Note: Console colors are not reset after writing.</I>
        /// </remarks>
        /// <typeparam name="T">The generic type of <paramref name="value"/> which implements <see cref="IConvertible"/>.</typeparam>
        /// <param name="value">The value whose text representation will be written.</param>
        /// <param name="fgColor">The foreground color to write in.</param>
        /// <param name="bgColor">The background color to write in.</param>
        public static void Write<T>(T value, byte fgColor, byte bgColor)
            where T : IConvertible
        {
            Console.ForegroundColor = (ConsoleColor)fgColor;
            Console.BackgroundColor = (ConsoleColor)bgColor;
            Console.Write(value);
        }

        /// <summary>
        /// Writes the text representation of the given type <typeparamref name="T"/> to the standard output stream with the given foreground and background colors.
        /// </summary>
        /// <remarks>
        /// <I>Note: Console colors are not reset after writing.</I>
        /// </remarks>
        /// <typeparam name="T">The generic type of <paramref name="value"/> which implements <see cref="IConvertible"/>.</typeparam>
        /// <param name="value">The value whose text representation will be written.</param>
        /// <param name="colorSet">The set of colors to write in.</param>
        public static void Write<T>(T value, ColorSet colorSet)
            where T : IConvertible
        {
            Write(value, colorSet.FgColor, colorSet.BgColor);
        }

        /// <summary>
        /// Writes the text representation of the given type <typeparamref name="T"/>, followed by the current line terminator, to the output stream with the given foreground and background colors.
        /// </summary>
        /// <remarks>
        /// <I>Note: Console colors are not reset after writing.</I>
        /// </remarks>
        /// <typeparam name="T">The generic type of <paramref name="value"/> which implements <see cref="IConvertible"/>.</typeparam>
        /// <param name="value">The value whose text representation will be written.</param>
        /// <param name="fgColor">The foreground color to write in.</param>
        /// <param name="bgColor">The background color to write in.</param>
        public static void WriteLine<T>(T value, byte fgColor, byte bgColor)
            where T : IConvertible
        {
            Write($"{value}\n", fgColor, bgColor);
        }

        /// <summary>
        /// Writes the text representation of the given type <typeparamref name="T"/>, followed by the current line terminator, to the output stream with the given foreground and background colors.
        /// </summary>
        /// <remarks>
        /// <I>Note: Console colors are not reset after writing.</I>
        /// </remarks>
        /// <typeparam name="T">The generic type of <paramref name="value"/> which implements <see cref="IConvertible"/>.</typeparam>
        /// <param name="value">The value whose text representation will be written.</param>
        /// <param name="colorSet">The set of colors to write in.</param>
        public static void WriteLine<T>(T value, ColorSet colorSet)
            where T : IConvertible
        {
            WriteLine(value, colorSet.FgColor, colorSet.BgColor);
        }

        /// <summary>
        /// Determines whether the given <see cref="byte"/> <paramref name="color"/> is a valid color code before returning it if it's valid.
        /// </summary>
        /// <param name="color">The color to check and return if it's valid.</param>
        /// <returns><paramref name="color"/> if it's a valid color code; otherwise, throws an exception.</returns>
        public static byte ValidSet(byte color)
        {
            return IsColorCode(color) ? color : throw InvalidColorCodeException;
        }

        /// <summary>
        /// Gets the contrasting color of either <see cref="Black"/> or <see cref="White"/> depending on whether the given <paramref name="color"/> is light.
        /// </summary>
        /// <param name="color">The color to check.</param>
        /// <returns><see cref="Black"/> if <paramref name="color"/> is light; otherwise, <see cref="White"/>.</returns>
        public static byte GetContrast(byte color)
        {
            return IsRealColor(color) && IsLightColor(color) ? Black : White;
        }

        /// <summary>
        /// Indicates whether the given <see cref="byte"/> <paramref name="color"/> is a light color.
        /// </summary>
        /// <param name="color">The color code to check.</param>
        /// <returns><see langword="true"/> if the color is light; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException">If given <paramref name="color"/> an invalid color code.</exception>
        public static bool IsLightColor(byte color)
        {
            return IsRealColor(color) ? color is White or Gray or Yellow or Cyan : throw InvalidColorCodeException;
        }

        /// <summary>
        /// Gets the correct color for layer stacking respective of color transparency.
        /// </summary>
        /// <remarks>
        /// <i>Note: Only the <paramref name="topColor"/> is checked for transparency.</i>
        /// </remarks>
        /// <param name="topColor">The color of the higher layer.</param>
        /// <param name="bottomColor">The color of the lower layer.</param>
        /// <returns><paramref name="topColor"/> if it isn't transparent; otherwise, <paramref name="bottomColor"/>.</returns>
        public static byte GetStackColor(byte topColor, byte bottomColor)
        {
            return topColor != Transparent ? topColor : bottomColor;
        }

        /// <summary>
        /// Gets the corresponding name for the given <see cref="byte"/> <paramref name="color"/>.
        /// </summary>
        /// <param name="color">The <see cref="byte"/> color to get the name of.</param>
        /// <returns>A string of the name corresponding to given <paramref name="color"/> if it's a valid color code; otherwise, returns <value>"None"</value>.</returns>
        public static string GetName(byte color)
        {
            return IsRealColor(color) ? ((ConsoleColor)color).ToString() : (color == Transparent ? "Transparent" : "None");
        }

        /// <summary>
        /// Indicates whether the given <see cref="byte"/> <paramref name="color"/> is a valid <see cref="ConsoleColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="byte"/> color to check.</param>
        /// <returns><see langword="true"/> if <paramref name="color"/> is a valid <see cref="ConsoleColor"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsRealColor(byte color)
        {
            return RealRange.InRange(color);
        }

        /// <summary>
        /// Indicates whether the given <see cref="byte"/> <paramref name="color"/> is a valid color code.
        /// </summary>
        /// <remarks>
        /// <i>Note: A valid color code is one which can be stored in a <see cref="Pixel"/> or <see cref="ColorSet"/>.</i>
        /// </remarks>
        /// <param name="color">The <see cref="byte"/> color to check.</param>
        /// <returns><see langword="true"/> if <paramref name="color"/> is a valid color code; otherwise, <see langword="false"/>.</returns>
        public static bool IsColorCode(byte color)
        {
            return FullRange.InRange(color);
        }
    }
}
