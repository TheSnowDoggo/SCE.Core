using System.Text;
namespace SCE
{
    public sealed class CompatibilityEngine : RenderEngine
    {
        private static readonly Lazy<CompatibilityEngine> _lazy = new(() => new());

        public CompatibilityEngine()
        {
        }

        /// <summary>
        /// Gets the lazy instance of this class.
        /// </summary>
        public static CompatibilityEngine Instance { get => _lazy.Value; }

        /// <inheritdoc/>
        public override void Render(MapView<Pixel> mapView, Vector2Int start)
        {
            Console.SetCursorPosition(start.X, start.Y);

            StringBuilder sb = new();

            var lastSet = ColorSet.Zero;
            bool first = true;

            for (int y = 0; y < mapView.Height; ++y)
            {
                if (y != 0)
                {
                    sb.Append('\n');
                }
                for (int x = 0; x < mapView.Width; ++x)
                {
                    if (first)
                    {
                        lastSet = mapView[x, y].ColorSet();
                        first = false;
                    }
                    var set = mapView[x, y].ColorSet();
                    if (lastSet == set)
                    {
                        sb.Append(mapView[x, y].Element);
                    }
                    else
                    {
                        ColorUtils.SetConsoleColor(lastSet);
                        Console.Write(sb);
                        sb.Clear(); 

                        sb.Append(mapView[x, y].Element);

                        lastSet = set;
                    }
                }
            }

            ColorUtils.SetConsoleColor(lastSet);

            Console.Write(sb);

            Console.ResetColor();
        }
    }
}
