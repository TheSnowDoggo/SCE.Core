namespace SCE
{
    internal class FlowTable : UIBaseExt
    {
        public FlowTable(int width, int height)
            : base(width, height)
        {
        }

        public FlowTable(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        public Pixel BasePixel { get; set; }

        public bool ClearOnRender { get; set; } = true;

        public bool Transparency { get; set; } = false;

        public FlowType FlowMode { get; set; } = FlowType.TopDown;

        public List<IRenderable> Renderables { get; } = new();

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

            int i = 0;
            foreach (var r in Renderables)
            {
                if (!r.IsActive)
                {
                    continue;
                }

                var mapView = r.GetMapView();

                Vector2Int off = FlowMode switch
                {
                    FlowType.TopDown => new(0, i),
                    FlowType.BottomTop => new(0, Height - i - mapView.Height),
                    FlowType.LeftRight => new(i, 0),
                    FlowType.RightLeft => new(Width - i - mapView.Width, 0),
                    _ => throw new NotImplementedException()
                };

                var h = FlowMode is FlowType.TopDown or FlowType.BottomTop;

                Vector2Int aRange = h ? new(Width, mapView.Height) : new(mapView.Width, Height);

                var pos = AnchorUtils.AnchorTo(r.Anchor, aRange, mapView.Dimensions) + r.Offset + off;

                if (Transparency)
                {
                    _dpMap.PMapTo(mapView, pos);
                }
                else
                {
                    _dpMap.MapTo(mapView, pos);
                }

                int nextI = FlowMode switch
                {
                    FlowType.TopDown => pos.Y + mapView.Height,
                    FlowType.BottomTop => Height - pos.Y,
                    FlowType.LeftRight => pos.X + mapView.Width,
                    FlowType.RightLeft => Width - pos.X,
                    _ => throw new NotImplementedException()
                };
                if (nextI > i)
                {
                    i = nextI;
                }
            }

            return _dpMap;
        }
    }
}
