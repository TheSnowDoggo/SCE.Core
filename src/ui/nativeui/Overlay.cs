namespace SCE
{
    internal class Overlay<T> : UIBase
        where T : IRenderable
    {
        public Overlay(T renderable, IRenderable? map = null)
        {
            Renderable = renderable;
            Map = map;
        }

        public T Renderable { get; set; }

        public IRenderable? Map { get; set; }

        public bool Transparency { get; set; } = true;

        public override DisplayMapView GetMapView()
        {
            var dpMap = Renderable.GetMapView().ToDisplayMap();

            if (Map != null)
            {
                var overMap = Map.GetMapView();

                var pos = AnchorUtils.AnchorTo(Map.Anchor, dpMap.Dimensions, overMap.Dimensions) + Map.Offset;

                if (dpMap.GridArea().Overlaps(pos, overMap.Dimensions + pos))
                {
                    if (Transparency)
                    {
                        dpMap.PMapTo(overMap, pos);
                    }
                    else
                    {
                        dpMap.MapTo(overMap, pos);
                    }
                }
            }

            return (DisplayMapView)dpMap;
        }
    }
}
