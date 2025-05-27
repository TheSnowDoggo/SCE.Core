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


        private static readonly Lazy<Display> _lazy = new(() => new());

        private readonly Viewport viewport = new(Console.WindowWidth, Console.WindowHeight);

        private Display()
        {
        }

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static Display Instance { get => _lazy.Value; }

        public AliasHash<IRenderable> Renderables { get => viewport.Renderables; }

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

        

        #endregion

        #region CompatibiliyRender

        

        #endregion
    }
}
