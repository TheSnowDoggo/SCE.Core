namespace SCE
{
    public interface IRenderable
    {
        /// <summary>
        /// Gets the active state of the renderable.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Gets the positional offset of the renderable.
        /// </summary>
        Vector2Int Offset { get; }

        /// <summary>
        /// Gets the render layer of the renderable.
        /// </summary>
        int Layer { get; }

        /// <summary>
        /// Gets the render anchor of the renderable.
        /// </summary>
        public Anchor Anchor { get; }

        /// <summary>
        /// Gets the displaymap to render.
        /// </summary>
        /// <returns>The displaymap to render.</returns>
        DisplayMap GetMap();
    }
}
