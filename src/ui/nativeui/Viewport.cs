namespace SCE
{
    public class Viewport : UIBaseExt
    {
        private Pixel basePixel = new(SCEColor.Black);

        public Viewport(int width, int height)
            : base(width, height)
        {
        }

        public Viewport(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        public Pixel BasePixel { get; set; }

        public bool ClearOnRender { get; set; } = true;

        public bool Transparency { get; set; } = false;

        public AliasHash<IRenderable> Renderables { get; } = new();

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        /// <inheritdoc/>
        public override DisplayMapView GetMapView()
        {
            if (ClearOnRender)
            {
                _dpMap.Fill(BasePixel);
            }

            List<IRenderable> list = new();

            foreach (var r in Renderables)
            {
                if (r.IsActive)
                {
                    list.Add(r);
                }
            }

            list.Sort((left, right) => left.Layer - right.Layer);

            foreach (var r in list)
            {
                var dpMap = r.GetMapView();

                var pos = AnchorUtils.AnchorTo(r.Anchor, Dimensions, dpMap.Dimensions) + r.Offset;

                var area = dpMap.GridArea();

                if (Transparency)
                {
                    _dpMap.PMapTo(dpMap, area, pos);
                }
                else
                {
                    _dpMap.MapTo(dpMap, area, pos);
                }
            }

            return (DisplayMapView)_dpMap;
        }
    }
}
