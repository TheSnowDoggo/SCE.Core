using System.Text;

namespace SCE
{
    public sealed class DebugEngine : RenderEngine
    {
        private static readonly Lazy<DebugEngine> _lazy = new(() => new());

        private DebugEngine()
        {
        }

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static DebugEngine Instance { get => _lazy.Value; }

        public SCEColor FgColor { get; set; } = SCEColor.Gray;

        public SCEColor BgColor { get; set; } = SCEColor.Black;

        private static char DebugGetChar(Pixel pixel)
        {
            if ((pixel.Element is ' ' or '\0') && pixel.BgColor != SCEColor.Black)
            {
                return SIFUtils._sifMap.GetT(pixel.BgColor);
            }
            return pixel.Element;
        }

        private static string DebugBuild(DisplayMap dpMap)
        {
            StringBuilder sb = new(dpMap.Size() + dpMap.Height);
            for (int y = 0; y < dpMap.Height; ++y)
            {
                if (y != 0)
                {
                    sb.Append('\n');
                }
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    sb.Append(DebugGetChar(dpMap[x, y]));
                }
            }
            return sb.ToString();
        }

        /// <inheritdoc/>
        public override void Render(DisplayMap dpMap)
        {
            ColorUtils.SetConsoleColor(FgColor, BgColor);
            Console.SetCursorPosition(0, 0);
            Console.Write(DebugBuild(dpMap));
            Console.ResetColor();
        }
    }
}
