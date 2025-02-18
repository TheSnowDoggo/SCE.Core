namespace SCE
{
    public class Viewport : UIBaseExt
    {
        private const string DEFAULT_NAME = "renderer";

        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        private readonly List<IRenderable> _renderList = new();

        #region Constructors

        public Viewport(string name, int width, int height, SCEColor? bgColor = null)
            : base(name, width, height, bgColor)
        {
            BgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public Viewport(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Viewport(int width, int height, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public Viewport(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        #endregion

        public SearchHash<IRenderable> Renderables { get; } = new();

        #region Settings

        public SCEColor BgColor { get; set; }

        public bool ClearOnRender { get; set; } = true;

        public bool IgnoreOutOfBoundRenderables { get; set; } = true;

        #endregion

        #region Render

        protected void Render()
        {
            if (ClearOnRender)
                _dpMap.Data.Fill(new Pixel(BgColor));
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
            _renderList.Sort((left, right) => left.Layer - right.Layer);
        }

        private void MapRenderList()
        {
            foreach (var renderable in _renderList)
            {
                var dpMap = renderable.GetMap();

                Vector2Int pos = AnchorUtils.AnchorTo(renderable.Anchor, Dimensions, dpMap.Dimensions) + renderable.Offset;

                if (!IgnoreOutOfBoundRenderables || _dpMap.GridArea.Overlaps(pos, dpMap.Dimensions + pos))
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

        /// <inheritdoc/>
        public override DisplayMap GetMap()
        {
            Render();
            return base.GetMap();
        }
    }
}
