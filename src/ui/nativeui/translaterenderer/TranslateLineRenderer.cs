namespace SCE
{
    public class TranslateLineRenderer : UIBaseExtR
    {
        public TranslateLineRenderer(int width, int height, Func<Vector2Int, Pixel[]> renderFunc)
            : base(width, height)
        {
            RenderFunc = renderFunc;
        }

        public TranslateLineRenderer(Vector2Int dimensions, Func<Vector2Int, Pixel[]> renderFunc)
            : this(dimensions.X, dimensions.Y, renderFunc)
        {
        }

        public TranslateLineRenderer(int width, int height, int renderWidth, Func<Vector2Int, Pixel[]> renderFunc)
            : this(width, height, renderFunc)
        {
            RenderWidth = renderWidth;
        }

        public TranslateLineRenderer(Vector2Int dimensions, int renderWidth, Func<Vector2Int, Pixel[]> renderFunc)
            : this(dimensions.X, dimensions.Y, renderWidth, renderFunc)
        {
        }

        /// <summary>
        /// Gets or sets the translation function.
        /// </summary>
        /// <remarks>
        /// The position represents the translated position given the render width.
        /// </remarks>
        public Func<Vector2Int, Pixel[]> RenderFunc { get; set; }

        #region Settings

        private int renderWidth = 1;

        /// <summary>
        /// Gets or sets the expected number of pixels per translation.
        /// </summary>
        public int RenderWidth
        {
            get => renderWidth;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Render width must be greater than 0.");
                renderWidth = value;
            }
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
        public override DisplayMapView GetMapView()
        {
            if (RenderOnUpdate)
                Render();
            return _dpMap;
        }
    }
}
