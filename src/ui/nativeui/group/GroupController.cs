namespace SCE
{
    public class GroupController : IRenderable
    {
        public GroupController(IRenderable renderable)
        {
            Renderable = renderable;
        }

        public GroupSource? Source { get; set; }

        public IRenderable Renderable { get; set; }

        public bool IsActive { get => Source == null ? Renderable.IsActive : Source.IsActive && Renderable.IsActive; }

        public Vector2Int Offset { get => Source == null ? Renderable.Offset : Source.Offset + Renderable.Offset; }

        public int Layer { get => Source?.Layer ?? Renderable.Layer; }

        public Anchor Anchor { get => Source?.Anchor ?? Renderable.Anchor; }

        public MapView<Pixel> GetMapView()
        {
            return Renderable.GetMapView();
        }
    }
}
