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

        public override MapView<Pixel> GetMapView()
        {
            DisplayMap dpMap = new(Renderable.GetMapView());

            if (Map != null && Map.IsActive)
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

            return dpMap;
        }
    }
}
