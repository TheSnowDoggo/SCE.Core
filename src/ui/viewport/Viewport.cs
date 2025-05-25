namespace SCE
{
    public class Viewport : UIBaseExtR
    {
        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        public Viewport(int width, int height, SCEColor? bgColor = null)
            : base(width, height, bgColor)
        {
            BgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public Viewport(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        public SCEColor BgColor { get; set; }

        public bool ClearOnRender { get; set; } = true;

        public bool CropOutOfBounds { get; set; } = true;

        public bool Transparency { get; set; } = true;

        public AliasHash<IRenderable> Renderables { get; } = new();

        protected virtual void Render()
        {
            if (ClearOnRender)
                _dpMap.Data.Fill(new Pixel(BgColor));

            List<IRenderable> list = new();

            foreach (var r in Renderables)
                if (r.IsActive)
                    list.Add(r);

            list.Sort((left, right) => left.Layer - right.Layer);

            foreach (var r in list)
            {
                var dpMap = r.GetMap();

                var pos = AnchorUtils.AnchorTo(r.Anchor, Dimensions, dpMap.Dimensions) + r.Offset;

                if (!CropOutOfBounds || _dpMap.GridArea().Overlaps(pos, dpMap.Dimensions + pos))
                {
                    if (Transparency)
                    {
                        _dpMap.PMapTo(dpMap, pos, true);
                    }
                    else
                    {
                        _dpMap.MapTo(dpMap, pos, true);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override DisplayMap GetMap()
        {
            Render();
            return base.GetMap();
        }
    }
}
