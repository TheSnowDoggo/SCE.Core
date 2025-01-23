namespace SCE
{
    public class Renderer : UIBaseExt
    {
        private readonly List<IRenderable> _renderList = new();

        public Renderer(int width, int height)
            : base(width, height)
        {
        }

        public Renderer(Vector2Int dimensions)
            : base(dimensions)
        {
        }

        public Color BgColor { get; set; }

        public bool ClearOnRender { get; set; } = true;

        public bool IgnoreOutOfBoundRenderables { get; set; } = true;

        public SearchList<IRenderable> Renderables { get; } = new();

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
        }

        public void Resize(Vector2Int dimensions)
        {
            _dpMap.CleanResize(dimensions);
        }

        protected override void Render()
        {
            if (ClearOnRender)
                FillBackground();
            UpdateRenderList();
        }

        private void FillBackground()
        {
            _dpMap.BgColorFill(BgColor);
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
            foreach (IRenderable renderable in Renderables)
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
            foreach (IRenderable renderable in _renderList)
            {
                var dpMap = renderable.GetMap();
                Vector2Int pos = AnchorUtils.AnchorTo(renderable.Anchor, _dpMap.Dimensions, dpMap.Dimensions) + renderable.Position;
 
               
                if (!IgnoreOutOfBoundRenderables || Area2DInt.Overlaps(dpMap.GridArea + pos, _dpMap.GridArea))
                    _dpMap.MapTo(dpMap, pos, true);
            }
        }
    }
}
