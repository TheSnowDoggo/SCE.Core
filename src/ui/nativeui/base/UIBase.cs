namespace SCE
{
    public abstract class UIBase : IRenderable
    {
        /// <inheritdoc/>
        public bool IsActive { get; set; } = true;

        /// <inheritdoc/>
        public Vector2Int Offset { get; set; }

        /// <inheritdoc/>
        public int Layer { get; set; }

        /// <inheritdoc/>
        public Anchor Anchor { get; set; }

        /// <inheritdoc/>
        public abstract DisplayMapView GetMapView();
    }
}
