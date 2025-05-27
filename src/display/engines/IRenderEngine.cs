namespace SCE
{
    /// <summary>
    /// Abstract base class for custom render engines.
    /// </summary>
    public abstract class RenderEngine
    {
        /// <summary>
        /// Renders the built DisplayMap.
        /// </summary>
        public abstract void Render(DisplayMap dpMap);

        /// <summary>
        /// Sets the viewport dimensions.
        /// </summary>
        public virtual Vector2Int? GetViewportDimensions()
        {
            return null;
        }
    }
}
