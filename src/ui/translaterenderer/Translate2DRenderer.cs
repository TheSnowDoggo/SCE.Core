﻿namespace SCE
{
    public class Translate2DRenderer : UIBaseExtR
    {
        private const string DEFAULT_NAME = "translate2d_renderer";

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Translate2DRenderer"/> class.
        /// </summary>
        public Translate2DRenderer(string name, int width, int height, Func<Vector2Int, Grid2D<Pixel>> renderFunc, Vector2Int? renderDimensions = null)
            : base(name, width, height)
        {
            RenderFunc = renderFunc;
            if (renderDimensions is Vector2Int vec)
                RenderDimensions = vec;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translate2DRenderer"/> class.
        /// </summary>
        public Translate2DRenderer(int width, int height, Func<Vector2Int, Grid2D<Pixel>> renderFunc, Vector2Int? renderDimensions = null)
            : this(DEFAULT_NAME, width, height, renderFunc, renderDimensions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translate2DRenderer"/> class.
        /// </summary>
        public Translate2DRenderer(string name, Vector2Int dimensions, Func<Vector2Int, Grid2D<Pixel>> renderFunc, Vector2Int? renderDimensions = null)
            : this(name, dimensions.X, dimensions.Y, renderFunc, renderDimensions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translate2DRenderer"/> class.
        /// </summary>
        public Translate2DRenderer(Vector2Int dimensions, Func<Vector2Int, Grid2D<Pixel>> renderFunc, Vector2Int? renderDimensions = null)
            : this(DEFAULT_NAME, dimensions.X, dimensions.Y, renderFunc, renderDimensions)
        {
        }

        #endregion

        /// <summary>
        /// Gets or sets the translation function.
        /// </summary>
        /// <remarks>
        /// The position represents the translated position given the render width.
        /// </remarks>
        public Func<Vector2Int, Grid2D<Pixel>> RenderFunc { get; set; }

        #region Settings

        /// <summary>
        /// Gets or sets the expected number of pixels per translation.
        /// </summary>
        public Vector2Int RenderDimensions
        {
            get => renderDimensions;
            set => SetRenderDimensions(value);
        }

        private Vector2Int renderDimensions = new(1,1);

        private void SetRenderDimensions(Vector2Int value)
        {
            if (value.X <= 0 || value.Y <= 0)
                throw new ArgumentException("Render dimensions must be greater than 0.");
            renderDimensions = value;
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
                    _dpMap.MapTo(rFunc(rPos), rPos * RenderDimensions);
                }
            }
        }

        private Func<Vector2Int, Grid2D<Pixel>> GetRenderFunc()
        {
            return RenderFunc ?? throw new NullReferenceException("Render function is null.");
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
