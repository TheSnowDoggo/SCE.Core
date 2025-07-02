namespace SCE
{
    public class CustomDisplay : SceneBase
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

        private const ResizeType DEF_RESIZE_TYPE = ResizeType.RenderEngine;

        private const bool DEF_RERENDER_CULLING = true;

        private static readonly RenderEngine _defaultEngine = BMEngine.Instance;

        private readonly Viewport _viewport = new(Vector2Int.Zero)
        {
            Transparency = true,
        };

        private RenderEngine? renderEngine = _defaultEngine;

        private DisplayMap? last;

        private bool skipCull = false;

        private Vector2Int startPosition;

        private Vector2Int lastWindowDimensions;

        public CustomDisplay()
        {
        }

        public CustomDisplay(Func<IEnumerable<IRenderable>> onRender)
        {
            OnRender = onRender;
        }

        protected Func<IEnumerable<IRenderable>>? OnRender
        {
            get => _viewport.OnRender;
            set => _viewport.OnRender = value;
        }

        public IUpdateLimit? UpdateLimiter { get; set; }

        /// <summary>
        /// Gets or sets the 
        /// </summary>
        public RenderEngine? RenderEngine
        {
            get => renderEngine;
            set => MiscUtils.QueueSet(ref renderEngine, value, ref skipCull);
        }

        /// <summary>
        /// Gets or sets the resizing mode.
        /// </summary>
        public ResizeType ResizeMode { get; set; } = DEF_RESIZE_TYPE;

        /// <summary>
        /// Gets or sets the custom viewport dimensions.
        /// </summary>
        public Vector2Int CustomDimensions { get; set; }

        /// <summary>
        /// Gets or sets a value representing whether the Display should only update when the rendered viewport has changed.
        /// </summary>
        public bool RerenderCulling { get; set; } = DEF_RERENDER_CULLING;

        /// <summary>
        /// Gets or sets the prefered start position to render at.
        /// </summary>
        public Vector2Int StartPosition
        {
            get => startPosition;
            set
            {
                if (value.OrLess(Vector2Int.Zero))
                {
                    throw new ArgumentException("Start position cannot be less than zero.");
                }
                MiscUtils.QueueSet(ref startPosition, value, ref skipCull);
            }
        }

        public Action<Vector2Int>? OnWindowResize;

        #region Properties

        /// <summary>
        /// Gets the width of the viewport.
        /// </summary>
        public int Width { get => _viewport.Width; }

        /// <summary>
        /// Gets the height of the viewport.
        /// </summary>
        public int Height { get => _viewport.Height; }

        /// <summary>
        /// Gets the dimensions of the viewport.
        /// </summary>
        public Vector2Int Dimensions { get => _viewport.Dimensions; }

        /// <summary>
        /// Gets the base pixel used to clear the viewport.
        /// </summary>
        public Pixel BasePixel
        {
            get => _viewport.BasePixel;
            set => _viewport.BasePixel = value;
        }

        /// <summary>
        /// Gets or sets a value representing whether the viewport be cleared every frame.
        /// </summary>
        public bool ClearOnRender
        {
            get => _viewport.ClearOnRender;
            set => _viewport.ClearOnRender = value;
        }

        /// <summary>
        /// Action called whenever the display viewport changes.
        /// </summary>
        public Action? OnDisplayResize;

        #endregion

        /// <summary>
        /// Returns the current Console window dimensions.
        /// </summary>
        public static Vector2Int WindowDimensions()
        {
            return new(Console.WindowWidth, Console.WindowHeight);
        }

        public static Vector2Int CursorPos()
        {
            return new(Console.CursorLeft, Console.CursorTop);
        }

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

            TryResize();

            var mapView = _viewport.GetMapView();

            UpdateCull(mapView);

            RenderEngine?.Render(mapView, StartPosition);
        }

        private void UpdateCull(MapView<Pixel> mapView)
        {
            if (!RerenderCulling)
            {
                return;
            }

            if (!skipCull)
            {
                if (last != null && Grid2D<Pixel>.ValueEquals(mapView, last))
                {
                    return;
                }
                else
                {
                    last = new(mapView);
                }
            }

            skipCull = false;
        }

        public void SkipCull()
        {
            skipCull = true;
        }

        public bool TryResize()
        {
            var windowDimensions = WindowDimensions();

            if (windowDimensions != lastWindowDimensions)
            {
                OnWindowResize?.Invoke(windowDimensions);
                lastWindowDimensions = windowDimensions;
            }

            var newSize = ResizeMode switch
            {
                ResizeType.Auto => windowDimensions,
                ResizeType.Custom => CustomDimensions,
                ResizeType.RenderEngine => RenderEngine?.GetViewportDimensions() ?? windowDimensions,
                _ => throw new NotImplementedException()
            };

            if (newSize == Dimensions)
            {
                return false;
            }

            _viewport.Resize(newSize);

            OnDisplayResize?.Invoke();

            return true;
        }

        public void SetupHere(Vector2Int dimensions)
        {
            StartPosition = CursorPos();
            ResizeMode = ResizeType.Custom;
            CustomDimensions = dimensions;
        }

        public void SetupHere(int width, int height)
        {
            SetupHere(new Vector2Int(width, height));
        }
    }
}
