using System.Diagnostics;
using System.Text;

namespace SCE
{
    public sealed class Display : SceneBase
    {
        public enum RenderType
        {
            CCS,
            Compatibility,
            Debug,
        }

        private const string DEFAULT_NAME = "display";

        private const int DEFAULT_BUFFER_SIZE = 7;

        private Display(string name)
            : base(name)
        {
        }

        #region Singleton

        private static readonly Lazy<Display> _lazy = new(() => new(DEFAULT_NAME));

        public static Display Instance { get => _lazy.Value; }

        #endregion

        #region Renderables

        private readonly Renderer _renderer = new(Console.WindowWidth, Console.WindowHeight);

        public SearchHash<IRenderable> Renderables { get => _renderer.Renderables; }

        #endregion

        #region Properties

        public Vector2Int Dimensions { get => _renderer.Dimensions; }

        public int Width { get => _renderer.Width; }

        public int Height { get => _renderer.Height; }

        public Action? OnDisplayResize { get; set; }

        #endregion

        #region Settings

        private RenderType renderMode;

        public RenderType RenderMode
        {
            get => renderMode;
            set
            {
                renderMode = value;
                if (renderMode == RenderType.Debug)
                    Console.ResetColor();
            }
        }

        public IUpdateLimit? UpdateLimiter { get; set; }

        public bool CheckForResize { get; set; } = true;

        public int DisplayBuffer { get; set; } = DEFAULT_BUFFER_SIZE;

        #endregion

        #region Scene

        /// <inheritdoc/>
        public override void Start()
        {
            Console.CursorVisible = false;
            TryResize();
        }

        /// <inheritdoc/>
        public override void Update()
        {
            if (!UpdateLimiter?.OnUpdate() ?? false)
                return;
            if (CheckForResize && TryResize())
                return;

            var dpMap = _renderer.GetMap();
            switch (RenderMode)
            {
                case RenderType.CCS:
                    CCSRender(dpMap);
                    break;
                case RenderType.Compatibility:
                    CompatibilityRender(dpMap);
                    break;
                case RenderType.Debug:
                    DebugRender(dpMap);
                    break;
            }
        }

        #endregion

        #region Resizing

        private bool TryResize()
        {
            var winDimensions = GetAdjustedWindowDimensions();
            if (winDimensions == Dimensions)
                return false;
            Console.ResetColor();
            Console.Clear();
            _renderer.Resize(winDimensions);
            OnDisplayResize?.Invoke();
            return true;
        }

        public static Vector2Int GetWindowDimensions()
        {
            return new(Console.WindowWidth, Console.WindowHeight);
        }

        public Vector2Int GetAdjustedWindowDimensions()
        {
            return RenderMode switch
            {
                RenderType.CCS => GetWindowDimensions() - new Vector2Int(DisplayBuffer, 0),
                RenderType.Debug or RenderType.Compatibility => GetWindowDimensions(),
                _ => throw new NotImplementedException()
            };
        }

        #endregion

        #region CCSRender

        private static void CCSRender(DisplayMap dpMap)
        {
            var writeInfoArr = CCSGetRenderInfo(dpMap);
            var buildArray = CCSBuildAll(dpMap, writeInfoArr);
            CCSWrite(buildArray, writeInfoArr);
        }

        private static StringBuilder[] CCSBuildAll(DisplayMap dpMap, ColorSet[] renderInfoArr)
        {
            var buildInfoArr = new StringBuilder[renderInfoArr.Length];
            for (int i = 0; i < renderInfoArr.Length; ++i)
                buildInfoArr[i] = CCSBuild(dpMap, renderInfoArr[i]);
            return buildInfoArr;
        }

        private static readonly CopyCache tabCache = new('\t');

        private static readonly CopyCache backspaceCache = new('\b');

        private static readonly Memoizer<int, string> spaceFillCache = new(SpaceFill);

        private static readonly StringBuilder spaceFillBuilder = new();

        public static StringBuilder CCSBuild(DisplayMap dpMap, ColorSet renderInfo)
        {
            StringBuilder strBuilder = new();

            renderInfo.Expose(out SCEColor fgColor, out SCEColor bgColor);

            bool differentColor = false;
            for (int y = dpMap.Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    var pixel = dpMap[x, y];
                    if (pixel.BgColor == bgColor && (pixel.Element is ' ' or '\0' || pixel.FgColor == fgColor))
                    {
                        if (differentColor)
                        {
                            strBuilder.Append(spaceFillCache.CachedCall(x));
                            differentColor = false;
                        }
                        strBuilder.Append(pixel.Element);
                    }
                    else
                    {
                        differentColor = true;
                    }
                }
                if (y != 0)
                    strBuilder.Append('\n');
            }
            return strBuilder;
        }

        private static string SpaceFill(int x)
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

        private static ColorSet[] CCSGetRenderInfo(DisplayMap dpMap)
        {
            HashSet<Pixel> pixelSet = new();
            HashSet<ColorSet> colorsSet = new();
            for (int y = dpMap.Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    var pixel = dpMap[x, y];
                    if (pixelSet.Contains(pixel))
                        continue;
                    colorsSet.Add(new ColorSet(pixel.FgColor, pixel.BgColor));
                }
            }
            return colorsSet.ToArray();
        }

        private static void CCSWrite(StringBuilder[] strBuilderArr, ColorSet[] renderInfoArr)
        {
            if (strBuilderArr.Length != renderInfoArr.Length)
                throw new ArgumentException("Array lengths do not match.");

            for (int i = 0; i < strBuilderArr.Length; i++)
            {
                var renderInfo = renderInfoArr[i];
                try
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = (ConsoleColor)renderInfo.FgColor;
                    Console.BackgroundColor = (ConsoleColor)renderInfo.BgColor;
                    Console.Write(strBuilderArr[i]);
                }
                catch
                {
                    Debug.WriteLine("SCE[DisplayManager] Error: Console buffer exceeded.");
                }
            }

            Console.ResetColor();
        }

        #endregion

        #region CompatibiliyRender

        private static void CompatibilityRender(DisplayMap dpMap)
        {
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new();
            SCEColor lastFgColor = dpMap[0,0].FgColor;
            SCEColor lastBgColor = dpMap[0, 0].BgColor;
            for (int y = dpMap.Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    var pixel = dpMap[x, y];
                    if (lastFgColor == pixel.FgColor && lastBgColor == pixel.BgColor)
                        sb.Append(pixel.Element);
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
                if (y != 0)
                    sb.Append('\n');
            }
            Console.ForegroundColor = (ConsoleColor)lastFgColor;
            Console.BackgroundColor = (ConsoleColor)lastBgColor;
            Console.Write(sb);
        }

        #endregion

        #region DebugRender

        private static void DebugRender(DisplayMap dpMap)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(DebugBuild(dpMap));
        }

        private static string DebugBuild(DisplayMap dpMap)
        {
            StringBuilder sb = new(dpMap.Size() + dpMap.Height);
            for (int y = dpMap.Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < dpMap.Width; ++x)
                    sb.Append(DebugGetChar(dpMap[x, y]));
                if (y != 0)
                    sb.Append('\n');
            }
            return sb.ToString();
        }

        private static char DebugGetChar(Pixel pixel)
        {
            if ((pixel.Element is ' ' or '\0') && pixel.BgColor != SCEColor.Black)
                return SIFUtils.sifMap.GetKey1(pixel.BgColor);
            return pixel.Element;
        }

        #endregion
    }
}
