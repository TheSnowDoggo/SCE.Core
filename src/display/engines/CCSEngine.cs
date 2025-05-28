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

        private CCSEngine()
        {
            spaceFillCache = new(SpaceFill);
        }

        public static CCSEngine Instance { get => _lazy.Value; }

        /// <summary>
        /// Gets or sets the side buffer required to prevent side jittering.
        /// </summary>
        public int DisplayBuffer { get; set; } = DEFAULT_BUFFER_SIZE;

        private static ColorSet[] GetRenderInfo(DisplayMapView dpMap)
        {
            HashSet<ColorSet> set = new();
            foreach (var pos in dpMap)
            {
                set.Add(dpMap[pos].ColorSet());
            }
            return set.ToArray();
        }

        private static void Write(string[] strArr, ColorSet[] rInfoArr)
        {
            if (strArr.Length != rInfoArr.Length)
            {
                throw new ArgumentException("Array lengths do not match.");
            }

            for (int i = 0; i < strArr.Length; i++)
            {
                try
                {
                    Console.SetCursorPosition(0, 0);
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

        private string Build(DisplayMapView dpMap, ColorSet rInfo)
        {
            StringBuilder sb = new();

            rInfo.Expose(out SCEColor fgColor, out SCEColor bgColor);

            bool different = false;
            for (int y = 0; y < dpMap.Height; ++y)
            {
                if (y != 0)
                {
                    sb.Append('\n');
                }
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    var pixel = dpMap[x, y];
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

        private string[] BuildAll(DisplayMapView dpMap, ColorSet[] renderInfoArr)
        {
            var bInfoArr = new string[renderInfoArr.Length];
            for (int i = 0; i < renderInfoArr.Length; ++i)
            {
                bInfoArr[i] = Build(dpMap, renderInfoArr[i]);
            }
            return bInfoArr;
        }

        /// <inheritdoc/>
        public override void Render(DisplayMapView dpMap)
        {
            var rInfoArr = GetRenderInfo(dpMap);
            var strArr = BuildAll(dpMap, rInfoArr);
            Write(strArr, rInfoArr);
        }

        ///<inheritdoc/>
        public override Vector2Int? GetViewportDimensions()
        {
            return Display.ConsoleWindowDimensions() - new Vector2Int(DisplayBuffer, 0);
        }
    }
}
