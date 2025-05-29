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
        public override DisplayMapView GetMapView()
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

                var dpMap = r.GetMapView();

                Vector2Int off = FlowMode switch
                {
                    FlowType.TopDown => new(0, i),
                    FlowType.BottomTop => new(0, Height - i - dpMap.Height),
                    FlowType.LeftRight => new(i, 0),
                    FlowType.RightLeft => new(Width - i - dpMap.Width, 0),
                    _ => throw new NotImplementedException()
                };

                var h = FlowMode is FlowType.TopDown or FlowType.BottomTop;

                Vector2Int aRange = h ? new(Width, dpMap.Height) : new(dpMap.Width, Height);

                var pos = AnchorUtils.AnchorTo(r.Anchor, aRange, dpMap.Dimensions) + r.Offset + off;

                if (Transparency)
                {
                    _dpMap.PMapTo(dpMap, pos);
                }
                else
                {
                    _dpMap.MapTo(dpMap, pos);
                }

                int nextI = FlowMode switch
                {
                    FlowType.TopDown => pos.Y + dpMap.Height,
                    FlowType.BottomTop => Height - pos.Y,
                    FlowType.LeftRight => pos.X + dpMap.Width,
                    FlowType.RightLeft => Width - pos.X,
                    _ => throw new NotImplementedException()
                };
                if (nextI > i)
                {
                    i = nextI;
                }
            }

            return (DisplayMapView)_dpMap;
        }
    }
}
