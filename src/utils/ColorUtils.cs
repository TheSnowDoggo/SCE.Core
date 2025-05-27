namespace SCE
{
    /// <summary>
    /// SCEColor managment class.
    /// </summary>
    public static class ColorUtils
    {
        /// <summary>
        /// Gets the contrasting color of either <see cref="SCEColor.Black"/> or <see cref="SCEColor.White"/> depending on whether the given <paramref name="color"/> is light.
        /// </summary>
        /// <param name="color">The color to check.</param>
        /// <returns><see cref="SCEColor.Black"/> if <paramref name="color"/> is light; otherwise, <see cref="SCEColor.White"/>.</returns>
        public static SCEColor GetContrast(SCEColor color)
        {
            return IsRealColor(color) && IsLightColor(color) ? SCEColor.Black : SCEColor.White;
        }

        /// <summary>
        /// Indicates whether the specified real color is a light color.
        /// </summary>
        /// <param name="color">The color code to check.</param>
        /// <returns><see langword="true"/> if the color is light; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException">If the specified <paramref name="color"/> transparent.</exception>
        public static bool IsLightColor(SCEColor color)
        {
            return IsRealColor(color) ? color is SCEColor.White or SCEColor.Gray or SCEColor.Yellow or SCEColor.Cyan : throw new ArgumentException("Color cannot be transparent.");
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
        public static SCEColor GetStackColor(SCEColor topColor, SCEColor bottomColor)
        {
            return topColor is not SCEColor.Transparent ? topColor : bottomColor;
        }

        /// <summary>
        /// Gets the corresponding name for the given <see cref="byte"/> <paramref name="color"/>.
        /// </summary>
        /// <param name="color">The <see cref="byte"/> color to get the name of.</param>
        /// <returns>A string of the name corresponding to given <paramref name="color"/> if it's a valid color code; otherwise, returns <value>"None"</value>.</returns>
        public static string GetName(SCEColor color)
        {
            return IsRealColor(color) ? ((ConsoleColor)color).ToString() : (color == SCEColor.Transparent ? "Transparent" : "None");
        }

        /// <summary>
        /// Indicates whether the given <see cref="byte"/> <paramref name="color"/> is a valid <see cref="ConsoleColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="byte"/> color to check.</param>
        /// <returns><see langword="true"/> if <paramref name="color"/> is a valid <see cref="ConsoleColor"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsRealColor(SCEColor color)
        {
            return color != SCEColor.Transparent;
        }

        /// <summary>
        /// Converts SCEColor to ConsoleColor.
        /// </summary>
        public static ConsoleColor ToConsoleColor(SCEColor color)
        {
            return color != SCEColor.Transparent ? (ConsoleColor)color : ConsoleColor.Black;
        }

        /// <summary>
        /// Sets the Console Foreground and Background color.
        /// </summary>
        public static void SetConsoleColor(SCEColor fgColor, SCEColor bgColor)
        {
            Console.ForegroundColor = ToConsoleColor(fgColor);
            Console.BackgroundColor = ToConsoleColor(bgColor);
        }

        /// <summary>
        /// Sets the Console Foreground and Background color.
        /// </summary>
        public static void SetConsoleColor(ColorSet colorSet)
        {
            SetConsoleColor(colorSet.FgColor, colorSet.BgColor);
        }
    }
}
