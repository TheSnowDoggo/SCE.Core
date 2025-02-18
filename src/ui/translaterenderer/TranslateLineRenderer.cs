namespace SCE
{
    public class TranslateLineRenderer : UIBaseExtR
    {
        private const string DEFAULT_NAME = "translate_renderer";

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateLineRenderer"/> class.
        /// </summary>
        public TranslateLineRenderer(string name, int width, int height, Func<Vector2Int, Pixel[]> renderFunc, int renderWidth = 1)
            : base(name, width, height)
        {
            RenderFunc = renderFunc;
            RenderWidth = renderWidth;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateLineRenderer"/> class.
        /// </summary>
        public TranslateLineRenderer(int width, int height, Func<Vector2Int, Pixel[]> renderFunc, int renderWidth = 1)
            : this(DEFAULT_NAME, width, height, renderFunc, renderWidth)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateLineRenderer"/> class.
        /// </summary>
        public TranslateLineRenderer(string name, Vector2Int dimensions, Func<Vector2Int, Pixel[]> renderFunc, int renderWidth = 1)
            : this(name, dimensions.X, dimensions.Y, renderFunc, renderWidth)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateLineRenderer"/> class.
        /// </summary>
        public TranslateLineRenderer(Vector2Int dimensions, Func<Vector2Int, Pixel[]> renderFunc, int renderWidth = 1)
            : this(DEFAULT_NAME, dimensions.X, dimensions.Y, renderFunc, renderWidth)
        {
        }

        #endregion

        /// <summary>
        /// Gets or sets the translation function.
        /// </summary>
        /// <remarks>
        /// The position represents the translated position given the render width.
        /// </remarks>
        public Func<Vector2Int, Pixel[]> RenderFunc { get; set; }

        #region Settings

        /// <summary>
        /// Gets or sets the expected number of pixels per translation.
        /// </summary>
        public int RenderWidth
        {
            get => renderWidth;
            set => SetRenderWidth(value);
        }

        private int renderWidth = 1;

        private void SetRenderWidth(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Render width must be greater than 0.");
            renderWidth = value;
        }

        public bool RenderOnUpdate { get; set; } = true;

        #endregion

        /// <summary>
        /// Renders the translate renderer.
        /// </summary>
        public void Render()
        {
            var rFunc = GetRenderFunc();
            for (int x = 0; x < Width / RenderWidth; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    var pixels = rFunc(new Vector2Int(x, y));
                    MapPixels(new Vector2Int(x * RenderWidth, y), pixels);
                }
            }
        }

        private Func<Vector2Int, Pixel[]> GetRenderFunc()
        {
            return RenderFunc ?? throw new NullReferenceException("Render function is null.");
        }

        private void MapPixels(Vector2Int start, Pixel[] pixels)
        {
            for (int i = 0; i < pixels.Length; ++i)
                _dpMap[start.X + i, start.Y] = pixels[i];
        }

        /// <inheritdoc/>
        public override DisplayMap GetMap()
        {
            if (RenderOnUpdate)
                Render();
            return base.GetMap();
        }
    }
}
