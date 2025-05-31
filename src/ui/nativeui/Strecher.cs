namespace SCE
{
    public class Scaler<T> : UIInterceptBase<T>
        where T : IRenderable
    {
        public Scaler(T renderable)
            : base(renderable)
        {
        }

        public Vector2Int Scale { get; set; } = new(2, 1);

        public override MapView<Pixel> GetMapView()
        {
            DisplayMap dpMap = new(Renderable.GetMapView());
            dpMap.Upscale(Scale);
            return dpMap;
        }
    }
}