namespace SCE
{
    public class Viewport : UIBaseExtR
    {
        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        public Viewport(int width, int height, SCEColor? bgColor = null)
            : base(width, height, bgColor)
        {
            BasePixel = new(bgColor ?? DEFAULT_BGCOLOR);
        }

        public Viewport(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Pixel BasePixel { get; set; }

        public bool ClearOnRender { get; set; } = true;

        public bool CropOutOfBounds { get; set; } = true;

        public bool Transparency { get; set; } = true;

        public AliasHash<IRenderable> Renderables { get; } = new();

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
                }
            }

            return (DisplayMapView)_dpMap;
        }
    }
}
