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

        private string DebugBuild(MapView<Pixel> mapView)
        {
            StringBuilder sb = new(mapView.Size() + mapView.Height);
            for (int y = 0; y < mapView.Height; ++y)
            {
                if (y != 0)
                {
                    sb.AppendLine();
                }
                for (int x = 0; x < mapView.Width; ++x)
                {
                    sb.Append(DebugGetChar(mapView[x, y]));
                }
            }
            return sb.ToString();
        }

        /// <inheritdoc/>
        public override void Render(MapView<Pixel> mapView, Vector2Int start)
        {
            ColorUtils.SetConsoleColor(FgColor, BgColor);

            Console.SetCursorPosition(start.X, start.Y);

            Console.Write(DebugBuild(mapView));

            Console.ResetColor();
        }
    }
}
