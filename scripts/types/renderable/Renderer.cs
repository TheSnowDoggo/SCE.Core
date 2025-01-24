namespace SCE
{
    public class Renderer : UIBaseExt
    {
        private const string DEFAULT_NAME = "renderer";

        private const Color DEFAULT_BGCOLOR = Color.Black;

        private readonly List<IRenderable> _renderList = new();

        public Renderer(string name, int width, int height, Color? bgColor = null)
            : base(name, width, height, bgColor)
        {
            BgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public Renderer(string name, Vector2Int dimensions, Color? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Renderer(int width, int height, Color? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public Renderer(Vector2Int dimensions, Color? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        public SearchHash<IRenderable> Renderables { get; } = new();

        public Color BgColor { get; set; }

        public bool ClearOnRender { get; set; } = true;

        public bool IgnoreOutOfBoundRenderables { get; set; } = true;

        private void FillBackground()
        {
            _dpMap.BgColorFill(BgColor);
        }

        #region Render
        protected override void Render()
        {
            if (ClearOnRender)
                FillBackground();
            UpdateRenderList();
        }

        private void UpdateRenderList()
        {
            _renderList.Clear();
            PopulateRenderList();
            SortRenderList();
            MapRenderList();
        }

        private void PopulateRenderList()
        {
            foreach (var renderable in Renderables)
            {
                if (renderable.IsActive)
                    _renderList.Add(renderable);
            }
        }

        private void SortRenderList()
        {
            _renderList.Sort((a, b) => a.Layer - b.Layer);
        }

        private void MapRenderList()
        {
            foreach (var renderable in _renderList)
            {
                var dpMap = renderable.GetMap();
                Vector2Int pos = AnchorUtils.AnchorTo(renderable.Anchor, _dpMap.Dimensions, dpMap.Dimensions) + renderable.Position;

                if (!IgnoreOutOfBoundRenderables || Area2DInt.Overlaps(dpMap.GridArea + pos, _dpMap.GridArea))
                    _dpMap.MapTo(dpMap, pos, true);
            }
        }
        #endregion

        #region Resize
        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
        }

        public void Resize(Vector2Int dimensions)
        {
            _dpMap.CleanResize(dimensions);
        }
        #endregion
    }
}
