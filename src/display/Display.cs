namespace SCE
{
    public sealed class Display : SceneBase
    {
        /// <summary>
        /// Represents the different resizing modes.
        /// </summary>
        public enum ResizeType
        {
            Auto,
            Custom,
            RenderEngine,
        }

        private static readonly Lazy<Display> _lazy = new(() => new());

        private readonly Viewport viewport = new(ConsoleWindowDimensions());

        private Display()
        {
        }

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static Display Instance { get => _lazy.Value; }

        public AliasHash<IRenderable> Renderables { get => viewport.Renderables; }

        public IUpdateLimit? UpdateLimiter { get; set; }

        /// <summary>
        /// Gets or sets the 
        /// </summary>
        public RenderEngine? RenderEngine { get; set; } = CCSEngine.Instance;

        /// <summary>
        /// Gets or sets the resizing mode.
        /// </summary>
        public ResizeType ResizeMode { get; set; } = ResizeType.RenderEngine;

        /// <summary>
        /// Gets or sets the custom viewport dimensions.
        /// </summary>
        public Vector2Int CustomDimensions { get; set; } = ConsoleWindowDimensions();

        #region Properties

        /// <summary>
        /// Gets the width of the viewport.
        /// </summary>
        public int Width { get => viewport.Width; }

        /// <summary>
        /// Gets the height of the viewport.
        /// </summary>
        public int Height { get => viewport.Height; }

        /// <summary>
        /// Gets the dimensions of the viewport.
        /// </summary>
        public Vector2Int Dimensions { get => viewport.Dimensions; }

        /// <summary>
        /// Gets the base pixel used to clear the viewport.
        /// </summary>
        public Pixel BasePixel
        {
            get => viewport.BasePixel;
            set => viewport.BasePixel = value;
        }

        /// <summary>
        /// Gets or sets a value representing whether the viewport be cleared every frame.
        /// </summary>
        public bool ClearOnRender
        {
            get => viewport.ClearOnRender;
            set => viewport.ClearOnRender = value;
        }

        /// <summary>
        /// Gets or sets a value representing whether out of bounds renderables should be ignored.
        /// </summary>
        public bool CropOutOfBounds
        {
            get => viewport.CropOutOfBounds;
            set => viewport.CropOutOfBounds = value;
        }

        /// <summary>
        /// Action called whenever the display viewport changes.
        /// </summary>
        public Action? OnDisplayResize;

        #endregion

        #region Console

        /// <summary>
        /// Returns the current Console window dimensions.
        /// </summary>
        public static Vector2Int ConsoleWindowDimensions()
        {
            return new(Console.WindowWidth, Console.WindowHeight);
        }

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
            {
                return;
            }

            RenderEngine?.Render(viewport.GetMap());

            TryResize();
        }

        private bool TryResize()
        {
            var newSize = ResizeMode switch
            {
                ResizeType.Auto => ConsoleWindowDimensions(),
                ResizeType.Custom => CustomDimensions,
                ResizeType.RenderEngine => RenderEngine?.GetViewportDimensions() ?? ConsoleWindowDimensions(),
                _ => throw new NotImplementedException()
            };

            if (newSize == Dimensions)
            {
                return false;
            }

            Console.ResetColor();
            Console.Clear();

            viewport.CleanResize(newSize);

            OnDisplayResize?.Invoke();

            return true;
        }
    }
}
