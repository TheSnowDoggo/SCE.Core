using CSUtils;
using System.Diagnostics;
using System.Text;

namespace SCE
{
    public sealed class CCSEngine : RenderEngine
    {
        /// <summary>
        /// Represents the default buffer size.
        /// </summary>
        public const int DEFAULT_BUFFER_SIZE = 7;

        private static readonly Lazy<CCSEngine> _lazy = new(() => new());

        private readonly Memoizer<int, string> tabCache = new(x => Utils.Copy('\t', x));

        private readonly Memoizer<int, string> backspaceCache = new(x => Utils.Copy('\b', x));

        private readonly Memoizer<int, string> spaceFillCache;

        public CCSEngine()
        {
            spaceFillCache = new(SpaceFill);
        }

        /// <summary>
        /// Gets the lazy instance of this class.
        /// </summary>
        public static CCSEngine Instance { get => _lazy.Value; }

        /// <summary>
        /// Gets or sets the side buffer required to prevent side jittering.
        /// </summary>
        public int DisplayBuffer { get; set; } = DEFAULT_BUFFER_SIZE;

        private static ColorSet[] GetRenderInfo(MapView<Pixel> mapView)
        {
            HashSet<ColorSet> set = new();
            foreach (var pos in mapView)
            {
                set.Add(mapView[pos].ColorSet());
            }
            return set.ToArray();
        }

        private static void Write(string[] strArr, ColorSet[] rInfoArr, Vector2Int start)
        {
            if (strArr.Length != rInfoArr.Length)
            {
                throw new ArgumentException("Array lengths do not match.");
            }

            for (int i = 0; i < strArr.Length; i++)
            {
                try
                {
                    Console.SetCursorPosition(start.X, start.Y);
                    ColorUtils.SetConsoleColor(rInfoArr[i]);
                    Console.Write(strArr[i]);
                }
                catch
                {
                    Debug.WriteLine("SCE[DisplayManager] Error: Console buffer exceeded.");
                }
            }

            Console.ResetColor();
        }

        private string SpaceFill(int x)
        {
            int tabLength = Utils.ClosestHigherMultiple(x, 8);
            return string.Join("", "\r", tabCache.Invoke(tabLength / 8), backspaceCache.Invoke(tabLength - x));
        }

        private string Build(MapView<Pixel> mapView, ColorSet rInfo)
        {
            StringBuilder sb = new();

            rInfo.Expose(out SCEColor fgColor, out SCEColor bgColor);

            bool different = false;
            for (int y = 0; y < mapView.Height; ++y)
            {
                if (y != 0)
                {
                    sb.Append('\n');
                }
                for (int x = 0; x < mapView.Width; ++x)
                {
                    var pixel = mapView[x, y];
                    if (pixel.BgColor == bgColor && (pixel.Element is ' ' or '\0' || pixel.FgColor == fgColor))
                    {
                        if (different)
                        {
                            sb.Append(spaceFillCache.Invoke(x));
                            different = false;
                        }
                        sb.Append(pixel.Element);
                    }
                    else
                    {
                        different = true;
                    }
                }
            }
            return sb.ToString();
        }

        private string[] BuildAll(MapView<Pixel> mapView, ColorSet[] renderInfoArr)
        {
            var bInfoArr = new string[renderInfoArr.Length];
            for (int i = 0; i < renderInfoArr.Length; ++i)
            {
                bInfoArr[i] = Build(mapView, renderInfoArr[i]);
            }
            return bInfoArr;
        }

        /// <inheritdoc/>
        public override void Render(MapView<Pixel> mapView, Vector2Int start)
        {
            var rInfoArr = GetRenderInfo(mapView);
            var strArr = BuildAll(mapView, rInfoArr);
            Write(strArr, rInfoArr, start);
        }

        ///<inheritdoc/>
        public override Vector2Int? GetViewportDimensions()
        {
            return Display.WindowDimensions() - new Vector2Int(DisplayBuffer, 0);
        }
    }
}
