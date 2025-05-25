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

        public override DisplayMap GetMap()
        {
            var dpMap = Renderable.GetMap();

            if (Map != null)
            {
                var overMap = Map.GetMap();

                var pos = AnchorUtils.AnchorTo(Map.Anchor, dpMap.Dimensions, overMap.Dimensions) + Map.Offset;

                if (dpMap.GridArea().Overlaps(pos, overMap.Dimensions + pos))
                {
                    dpMap.MapTo(overMap, pos, true);
                }
            }

            return dpMap;
        }
    }
}
