using System.Diagnostics;
using System.Text;

namespace SCE
{
    public sealed class CCSEngine : IRenderEngine
    {
        private static readonly Lazy<CCSEngine> _lazy = new(() => new());

        private readonly CopyCache tabCache = new('\t');

        private readonly CopyCache backspaceCache = new('\b');

        private readonly Memoizer<int, string> spaceFillCache;

        private readonly StringBuilder spaceFillBuilder = new();

        private CCSEngine()
        {
            spaceFillCache = new(SpaceFill);
        }

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static CCSEngine Instance { get => _lazy.Value; }

        private string SpaceFill(int x)
        {
            int tabLength = MathUtils.ClosestHigherMultiple(x, 8);
            int tabs = tabLength / 8;
            int backspaces = tabLength - x;

            spaceFillBuilder.Clear();
            spaceFillBuilder.Append('\r');
            spaceFillBuilder.Append(tabCache.CachedCall(tabs));
            spaceFillBuilder.Append(backspaceCache.CachedCall(backspaces));
            return spaceFillBuilder.ToString();
        }

        private ColorSet[] CCSGetRenderInfo(DisplayMap dpMap)
        {
            HashSet<Pixel> pixelSet = new();
            HashSet<ColorSet> colorsSet = new();
            for (int y = 0; y < dpMap.Height; ++y)
            {
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    var pixel = dpMap[x, y];
                    if (!pixelSet.Contains(pixel))
                    {
                        colorsSet.Add(new ColorSet(pixel.FgColor, pixel.BgColor));
                    }
                }
            }
            return colorsSet.ToArray();
        }

        public StringBuilder CCSBuild(DisplayMap dpMap, ColorSet rInfo)
        {
            StringBuilder sb = new();

            rInfo.Expose(out SCEColor fgColor, out SCEColor bgColor);

            bool different = false;
            for (int y = 0; y < dpMap.Height; ++y)
            {
                if (y != 0)
                    sb.Append('\n');
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    var pixel = dpMap[x, y];
                    if (pixel.BgColor == bgColor && (pixel.Element is ' ' or '\0' || pixel.FgColor == fgColor))
                    {
                        if (different)
                        {
                            sb.Append(spaceFillCache.CachedCall(x));
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
            return sb;
        }

        private StringBuilder[] CCSBuildAll(DisplayMap dpMap, ColorSet[] renderInfoArr)
        {
            var bInfoArr = new StringBuilder[renderInfoArr.Length];
            for (int i = 0; i < renderInfoArr.Length; ++i)
                bInfoArr[i] = CCSBuild(dpMap, renderInfoArr[i]);
            return bInfoArr;
        }

        private void CCSWrite(StringBuilder[] sbArr, ColorSet[] rInfoArr)
        {
            if (sbArr.Length != rInfoArr.Length)
                throw new ArgumentException("Array lengths do not match.");

            for (int i = 0; i < sbArr.Length; i++)
            {
                var rInfo = rInfoArr[i];
                try
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = (ConsoleColor)rInfo.FgColor;
                    Console.BackgroundColor = (ConsoleColor)rInfo.BgColor;
                    Console.Write(sbArr[i]);
                }
                catch
                {
                    Debug.WriteLine("SCE[DisplayManager] Error: Console buffer exceeded.");
                }
            }

            Console.ResetColor();
        }

        /// <inheritdoc/>
        public void Render(DisplayMap dpMap)
        {
            var writeInfoArr = CCSGetRenderInfo(dpMap);
            var buildArray = CCSBuildAll(dpMap, writeInfoArr);
            CCSWrite(buildArray, writeInfoArr);
        }
    }
}
