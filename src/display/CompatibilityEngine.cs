using System.Text;

namespace SCE
{
    public sealed class CompatibilityEngine : IRenderEngine
    {
        private static readonly Lazy<CompatibilityEngine> _lazy = new(() => new());

        private CompatibilityEngine()
        {
        }

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static CompatibilityEngine Instance { get => _lazy.Value; }

        /// <inheritdoc/>
        public void Render(DisplayMap dpMap)
        {
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new();
            var lastFgColor = SCEColor.Black;
            var lastBgColor = SCEColor.Black;
            bool first = true;
            for (int y = 0; y < dpMap.Height; ++y)
            {
                if (y != 0)
                {
                    sb.Append('\n');
                }
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    if (first)
                    {
                        lastFgColor = dpMap[x, y].FgColor;
                        lastBgColor = dpMap[x, y].BgColor;
                        first = false;
                    }
                    var pixel = dpMap[x, y];
                    if (lastFgColor == pixel.FgColor && lastBgColor == pixel.BgColor)
                    {
                        sb.Append(pixel.Element);
                    }
                    else
                    {
                        Console.ForegroundColor = (ConsoleColor)lastFgColor;
                        Console.BackgroundColor = (ConsoleColor)lastBgColor;
                        Console.Write(sb);
                        sb.Clear();

                        lastFgColor = pixel.FgColor;
                        lastBgColor = pixel.BgColor;
                        sb.Append(pixel.Element);
                    }
                }
            }
            Console.ForegroundColor = (ConsoleColor)lastFgColor;
            Console.BackgroundColor = (ConsoleColor)lastBgColor;
            Console.Write(sb);
            Console.ResetColor();
        }
    }
}
