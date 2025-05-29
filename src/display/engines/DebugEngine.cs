using System.Text;

namespace SCE
{
    public sealed class DebugEngine : RenderEngine
    {
        private static readonly Lazy<DebugEngine> _lazy = new(() => new());

        public DebugEngine()
        {
        }

        /// <summary>
        /// Gets the lazy instance of this class.
        /// </summary>
        public static DebugEngine Instance { get => _lazy.Value; }

        public SCEColor FgColor { get; set; } = SCEColor.Gray;

        public SCEColor BgColor { get; set; } = SCEColor.Black;

        public bool ShowSIFCodes { get; set; } = true;

        private  char DebugGetChar(Pixel pixel)
        {
            var c = pixel.RenderElement();
            if (ShowSIFCodes && c == ' ' && pixel.BgColor != SCEColor.Black)
            {
                return SIFUtils.ToSIFCode(pixel.BgColor);
            }
            return c;
        }

        private string DebugBuild(DisplayMapView dpMap)
        {
            StringBuilder sb = new(dpMap.Size() + dpMap.Height);
            for (int y = 0; y < dpMap.Height; ++y)
            {
                if (y != 0)
                {
                    sb.AppendLine();
                }
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    sb.Append(DebugGetChar(dpMap[x, y]));
                }
            }
            return sb.ToString();
        }

        /// <inheritdoc/>
        public override void Render(DisplayMapView dpMap, Vector2Int start)
        {
            ColorUtils.SetConsoleColor(FgColor, BgColor);

            Console.SetCursorPosition(start.X, start.Y);

            Console.Write(DebugBuild(dpMap));

            Console.ResetColor();
        }
    }
}
