namespace SCE
{
    public class Scaler<T> : UIBase 
        where T : IRenderable
    {
        public Scaler(T renderable)
        {
            Renderable = renderable;
        }

        public T Renderable { get; set; }

        public Vector2Int Scale { get; set; } = new(2, 1);

        public override DisplayMapView GetMapView()
        {
            var dpMap = Renderable.GetMapView().ToDisplayMap();
            dpMap.Upscale(Scale);
            return dpMap;
        }
    }
}
