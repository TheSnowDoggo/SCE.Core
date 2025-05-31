namespace SCE
{
    public class Viewport : UIBaseExt
    {
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

        public AliasHash<IRenderable> Renderables { get; set; } = new();

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        /// <inheritdoc/>
        public override MapView<Pixel> GetMapView()
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

            list.Sort((left, right) => right.Layer - left.Layer);

            List<Rect2DInt> mapList = new();

            foreach (var r in list)
            {
                var mapView = r.GetMapView();

                var pos = AnchorUtils.AnchorTo(r.Anchor, Dimensions, mapView.Dimensions) + r.Offset;

                if (Transparency)
                {
                    _dpMap.PMapTo(mapView, pos);
                }
                else
                {
                    _dpMap.MapTo(mapView, pos);
                }
            }

            return _dpMap;
        }
    }
}
