namespace SCE
{
    public abstract class UIInterceptBase<T> : IRenderable
        where T : IRenderable
    {
        public UIInterceptBase(T renderable)
        {
            Renderable = renderable;
        }

        public T Renderable { get; set; }

        public bool IsActive { get => Renderable.IsActive; }

        public Vector2Int Offset { get => Renderable.Offset; }

        public int Layer { get => Renderable.Layer; }

        public Anchor Anchor { get => Renderable.Anchor; }

        public abstract DisplayMapView GetMapView();
    }
}
