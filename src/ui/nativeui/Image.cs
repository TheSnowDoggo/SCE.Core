namespace SCE
{
    public class Image : DisplayMap, ICloneable, IRenderable
    {
        public Image(int width, int height, SCEColor? bgColor = null)
            : base(width, height, bgColor)
        {
        }

        public Image(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Image(DisplayMap displayMap)
            : base(displayMap)
        {
        }

        public bool IsActive { get; set; } = true;

        public int Layer { get; set; }

        public Vector2Int Offset { get; set; }

        public Anchor Anchor { get; set; }

        public Action? OnRender;

        public override Image Clone()
        {
            return new(base.Clone())
            {
                IsActive = IsActive,
                Layer = Layer,
                Offset = Offset,
                Anchor = Anchor,
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <inheritdoc/>
        public virtual DisplayMapView GetMapView()
        {
            OnRender?.Invoke();
            return (DisplayMapView)this;
        }
    }
}
