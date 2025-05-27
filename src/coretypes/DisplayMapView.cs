namespace SCE
{
    public class DisplayMapView : Grid2DView<Pixel>
    {
        private readonly DisplayMap _dpMap;

        public DisplayMapView(DisplayMap dpMap)
            : base(dpMap)
        {
            _dpMap = dpMap;
        }

        public DisplayMap ToDisplayMap()
        {
            return _dpMap.Clone();
        }
    }
}
