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

            list.Sort((left, right) => right.Layer - left.Layer);

            List<Rect2DInt> mappedAreas = new();

            var thisArea = _dpMap.GridArea();

            foreach (var r in list)
            {
                var dpMap = r.GetMapView();

                var pos = AnchorUtils.AnchorTo(r.Anchor, Dimensions, dpMap.Dimensions) + r.Offset;

                var area = thisArea.GetOverlap(dpMap.GridArea() + pos);

                List<Rect2DInt> areasToMap = new() { area };
                foreach (var mapped in mappedAreas)
                {
                    List<Rect2DInt> nextAreasToMap = new();
                    foreach (var map in areasToMap)
                    {
                        nextAreasToMap.AddRange(map.SplitAway(mapped));
                    }
                    areasToMap.Clear();

                    areasToMap.AddRange(nextAreasToMap);
                    if (areasToMap.Count == 0)
                    {
                        break;
                    }
                }

                foreach (var map in areasToMap)
                {
                    var gridArea = map - pos;
                    if (Transparency)
                    {
                        _dpMap.PMapTo(dpMap, gridArea, pos);
                    }
                    else
                    {
                        _dpMap.MapFrom(dpMap, map);
                    }
                }

                mappedAreas.AddRange(areasToMap);
            }

            return (DisplayMapView)_dpMap;
        }
    }
}
