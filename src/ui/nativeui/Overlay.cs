namespace SCE
{
    public class Overlay<T> : UIInterceptBase<T>
        where T : IRenderable
    {
        public Overlay(T renderable, IRenderable? map = null)
            : base(renderable)
        {
            Map = map;
        }

        public IRenderable? Map { get; set; }

        public bool Transparency { get; set; } = true;

        public override DisplayMapView GetMapView()
        {
            var dpMap = Renderable.GetMapView().ToDisplayMap();

            if (Map != null)
            {
                var overMap = Map.GetMapView();

                var pos = AnchorUtils.AnchorTo(Map.Anchor, dpMap.Dimensions, overMap.Dimensions) + Map.Offset;

                if (Transparency)
                {
                    dpMap.PMapTo(overMap, pos);
                }
                else
                {
                    dpMap.MapTo(overMap, pos);
                }
            }

            return (DisplayMapView)dpMap;
        }
    }
}
