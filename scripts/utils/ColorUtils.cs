namespace SCE
{
    /// <summary>
    /// SCEColor managment class.
    /// </summary>
    public static class ColorUtils
    {
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
        public static void Write<T>(T value, Color fgColor, Color bgColor)
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
        public static void WriteLine<T>(T value, Color fgColor, Color bgColor)
            where T : IConvertible
        {
            Write(value.ToString() + '\n', fgColor, bgColor);
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
        /// Gets the contrasting color of either <see cref="Black"/> or <see cref="White"/> depending on whether the given <paramref name="color"/> is light.
        /// </summary>
        /// <param name="color">The color to check.</param>
        /// <returns><see cref="Black"/> if <paramref name="color"/> is light; otherwise, <see cref="White"/>.</returns>
        public static Color GetContrast(Color color)
        {
            return IsRealColor(color) && IsLightColor(color) ? Color.Black : Color.White;
        }

        /// <summary>
        /// Indicates whether the specified real color is a light color.
        /// </summary>
        /// <param name="color">The color code to check.</param>
        /// <returns><see langword="true"/> if the color is light; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException">If the specified <paramref name="color"/> transparent.</exception>
        public static bool IsLightColor(Color color)
        {
            return IsRealColor(color) ? color is Color.White or Color.Gray or Color.Yellow or Color.Cyan : throw new ArgumentException("Color cannot be transparent.");
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
        public static Color GetStackColor(Color topColor, Color bottomColor)
        {
            return topColor != Color.Transparent ? topColor : bottomColor;
        }

        /// <summary>
        /// Gets the corresponding name for the given <see cref="byte"/> <paramref name="color"/>.
        /// </summary>
        /// <param name="color">The <see cref="byte"/> color to get the name of.</param>
        /// <returns>A string of the name corresponding to given <paramref name="color"/> if it's a valid color code; otherwise, returns <value>"None"</value>.</returns>
        public static string GetName(Color color)
        {
            return IsRealColor(color) ? ((ConsoleColor)color).ToString() : (color == Color.Transparent ? "Transparent" : "None");
        }

        /// <summary>
        /// Indicates whether the given <see cref="byte"/> <paramref name="color"/> is a valid <see cref="ConsoleColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="byte"/> color to check.</param>
        /// <returns><see langword="true"/> if <paramref name="color"/> is a valid <see cref="ConsoleColor"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsRealColor(Color color)
        {
            return color != Color.Transparent;
        }
    }
}
