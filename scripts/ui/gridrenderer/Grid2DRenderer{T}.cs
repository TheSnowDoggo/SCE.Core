namespace SCE
{
    public class Grid2DRenderer<T> : UIBaseExt
    {
        private const string DEFAULT_NAME = "grid2d_renderer";

        public Grid2DRenderer(string name, Grid2D<T> grid2D, Func<Vector2Int, Pixel>? renderFunc = null)
            : base(grid2D.Dimensions)
        {
            Grid = grid2D;
            RenderFunc = renderFunc;
        }

        public Grid2DRenderer(Grid2D<T> grid2D, Func<Vector2Int, Pixel>? renderFunc = null)
            : this(DEFAULT_NAME, grid2D, renderFunc)
        {
        }

        public Grid2D<T> Grid { get; set; }

        public Func<Vector2Int, Pixel>? RenderFunc { get; set; }

        public bool RenderOnUpdate { get; set; } = false;

        public void Update()
        {
            if (RenderOnUpdate)
                Render();
        }

        private void Render()
        {
            if (RenderFunc is null)
                throw new NullReferenceException("Render function is null.");

            _dpMap.GenericCycle((pos) => _dpMap[pos] = RenderFunc.Invoke(pos));
        }

        public override DisplayMap GetMap()
        {
            if (!RenderOnUpdate)
                Render();
            return base.GetMap();
        }
    }
}
