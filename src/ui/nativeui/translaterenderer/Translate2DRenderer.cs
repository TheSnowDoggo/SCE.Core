namespace SCE
{
    public class Translate2DRenderer : UIBaseExtR
    {
        public Translate2DRenderer(int width, int height, Func<Vector2Int, Grid2D<Pixel>> renderFunc)
            : base(width, height)
        {
            RenderFunc = renderFunc;
        }

        public Translate2DRenderer(Vector2Int dimensions, Func<Vector2Int, Grid2D<Pixel>> renderFunc)
            : this(dimensions.X, dimensions.Y, renderFunc)
        {
        }

        public Translate2DRenderer(int width, int height, Vector2Int renderDimensions, Func<Vector2Int, Grid2D<Pixel>> renderFunc)
            : this(width, height, renderFunc)
        {
            RenderDimensions = renderDimensions;
        }

        public Translate2DRenderer(Vector2Int dimensions, Vector2Int renderDimensions, Func<Vector2Int, Grid2D<Pixel>> renderFunc)
            : this(dimensions.X, dimensions.Y, renderDimensions, renderFunc)
        {
        }

        /// <summary>
        /// Gets or sets the translation function.
        /// </summary>
        /// <remarks>
        /// The position represents the translated position given the render width.
        /// </remarks>
        public Func<Vector2Int, Grid2D<Pixel>> RenderFunc { get; set; }

        #region Settings

        private Vector2Int renderDimensions = new(1, 1);

        /// <summary>
        /// Gets or sets the expected number of pixels per translation.
        /// </summary>
        public Vector2Int RenderDimensions
        {
            get => renderDimensions;
            set
            {
                if (value.X <= 0 || value.Y <= 0)
                    throw new ArgumentException("Render dimensions must be greater than 0.");
                renderDimensions = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether rendering should be called automatically by the renderer.
        /// </summary>
        public bool RenderOnUpdate { get; set; } = true;

        #endregion

        /// <summary>
        /// Renders the translate renderer.
        /// </summary>
        public void Render()
        {
            var rFunc = GetRenderFunc();
            for (int x = 0; x < Width / RenderDimensions.X; ++x)
            {
                for (int y = 0; y < Height / RenderDimensions.Y; ++y)
                {
                    var rPos = new Vector2Int(x, y);
                    _dpMap.PMapTo(rFunc.Invoke(rPos), rPos * RenderDimensions);
                }
            }
        }

        private Func<Vector2Int, Grid2D<Pixel>> GetRenderFunc()
        {
            return RenderFunc ?? throw new NullReferenceException("Render function is null.");
        }

        /// <inheritdoc/>
        public override DisplayMapView GetMapView()
        {
            if (RenderOnUpdate)
            {
                Render();
            }
            return base.GetMapView();
        }
    }
}
