using System.Diagnostics;
using System.Text;

namespace SCE
{
    public sealed class Display : SceneBase
    {
        /// <summary>
        /// Represents the different rendering modes.
        /// </summary>
        public enum RenderType
        {
            /// <summary>
            /// Highly efficient colored rendering.
            /// </summary>
            /// <remarks>
            /// Note: Requires a small buffer (at least 7 characters) on the right side of the screen to avoid jittering.
            /// </remarks>
            CCS,
            /// <summary>
            /// Slower coloured rendering that is more basic and doesn't require a buffer.
            /// </summary>
            /// <remarks>
            /// Note: Screen tearing can be visible when moving, especially when there are many colors.
            /// </remarks>
            Compatibility,
            /// <summary>
            /// Super efficient non-colored rendering.
            /// </summary>
            /// <remarks>
            /// Note: Background colors without text are represented by characters.
            /// </remarks>
            Debug,
        }

        private const int DEFAULT_BUFFER_SIZE = 7;

        #region Singleton

        private static readonly Lazy<Display> _lazy = new(() => new());

        public static Display Instance { get => _lazy.Value; }

        #endregion

        #region Renderables

        private readonly Viewport viewport = new(Console.WindowWidth, Console.WindowHeight);

        public AliasHash<IRenderable> Renderables { get => viewport.Renderables; }

        #endregion

        #region Properties

        public Vector2Int Dimensions { get => viewport.Dimensions; }

        public int Width { get => viewport.Width; }

        public int Height { get => viewport.Height; }

        public SCEColor BgColor
        {
            get => viewport.BgColor;
            set => viewport.BgColor = value;
        }

        public bool ClearOnRender
        {
            get => viewport.ClearOnRender;
            set => viewport.ClearOnRender = value;
        }

        public bool CropOutOfBounds
        {
            get => viewport.CropOutOfBounds;
            set => viewport.CropOutOfBounds = value;
        }

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
            if (CheckForResize)
                TryResize();

            var dpMap = viewport.GetMap();
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
            viewport.CleanResize(winDimensions);
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

        private static readonly CopyCache tabCache = new('\t');

        private static readonly CopyCache backspaceCache = new('\b');

        private static readonly Memoizer<int, string> spaceFillCache = new(SpaceFill);

        private static readonly StringBuilder spaceFillBuilder = new();

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

        public static StringBuilder CCSBuild(DisplayMap dpMap, ColorSet rInfo)
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

        private static StringBuilder[] CCSBuildAll(DisplayMap dpMap, ColorSet[] renderInfoArr)
        {
            var bInfoArr = new StringBuilder[renderInfoArr.Length];
            for (int i = 0; i < renderInfoArr.Length; ++i)
                bInfoArr[i] = CCSBuild(dpMap, renderInfoArr[i]);
            return bInfoArr;
        }

        private static void CCSWrite(StringBuilder[] sbArr, ColorSet[] rInfoArr)
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

        private static void CCSRender(DisplayMap dpMap)
        {
            var writeInfoArr = CCSGetRenderInfo(dpMap);
            var buildArray = CCSBuildAll(dpMap, writeInfoArr);
            CCSWrite(buildArray, writeInfoArr);
        }

        #endregion

        #region CompatibiliyRender

        private static void CompatibilityRender(DisplayMap dpMap)
        {
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new();
            var lastFgColor = SCEColor.Black;
            var lastBgColor = SCEColor.Black;
            bool first = true;
            for (int y = 0; y < dpMap.Height; ++y)
            {
                if (y != 0)
                    sb.Append('\n');
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
        }

        #endregion

        #region DebugRender

        private static char DebugGetChar(Pixel pixel)
        {
            if ((pixel.Element is ' ' or '\0') && pixel.BgColor != SCEColor.Black)
                return SIFUtils._sifMap.GetT(pixel.BgColor);
            return pixel.Element;
        }

        private static string DebugBuild(DisplayMap dpMap)
        {
            StringBuilder sb = new(dpMap.Size() + dpMap.Height);
            for (int y = 0; y < dpMap.Height; ++y)
            {
                if (y != 0)
                    sb.Append('\n');
                for (int x = 0; x < dpMap.Width; ++x)
                    sb.Append(DebugGetChar(dpMap[x, y]));
            }
            return sb.ToString();
        }

        private static void DebugRender(DisplayMap dpMap)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(DebugBuild(dpMap));
        }

        #endregion
    }
}
