namespace SCE
{
    internal class FlowTable : UIBaseExtR
    {
        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        public FlowTable(int width, int height, SCEColor? bgColor = null)
            : base(width, height, bgColor)
        {
            BasePixel = new(bgColor ?? DEFAULT_BGCOLOR);
        }

        public FlowTable(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Pixel BasePixel { get; set; }

        public bool ClearOnRender { get; set; } = true;

        public bool CropOutOfBounds { get; set; } = true;

        public bool Transparency { get; set; } = true;

        public FlowType FlowMode { get; set; } = FlowType.TopDown;

        public List<IRenderable> Renderables { get; } = new();

        protected virtual void Render()
        {
            
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
                    FlowType.BottomUp => new(0, Height - i - dpMap.Height),
                    FlowType.LeftRight => new(i, 0),
                    FlowType.RightLeft => new(Width - i - dpMap.Width, 0),
                    _ => throw new NotImplementedException()
                };

                Vector2Int aRange = FlowMode is FlowType.TopDown or FlowType.BottomUp ? new(Width, dpMap.Height) :
                    new(dpMap.Width, Height);

                var pos = AnchorUtils.AnchorTo(r.Anchor, aRange, dpMap.Dimensions) + r.Offset + off;

                if (!CropOutOfBounds || _dpMap.GridArea().Overlaps(pos, dpMap.Dimensions + pos))
                {
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
                        FlowType.BottomUp => Height - pos.Y,
                        FlowType.LeftRight => pos.X + dpMap.Width,
                        FlowType.RightLeft => Width - pos.X,
                        _ => throw new NotImplementedException()
                    };
                    if (nextI > i)
                    {
                        i = nextI;
                    }
                }
            }

            return _dpMap;
        }
    }
}
