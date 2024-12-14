namespace SCE
{
    /// <summary>
    /// An interface used for getting the <see cref="Image"/> to render to the <see cref="Display"/>.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        bool IsActive { get; }

        Vector2Int Position { get; }

        int Layer { get; }

        public Anchor Anchor { get; }

        DisplayMap GetMap();
    }
}
