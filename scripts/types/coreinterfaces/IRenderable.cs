namespace SCECore
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

        /// <summary>
        /// Returns the <see cref="Image"/> to render to the <see cref="Display"/>.
        /// </summary>
        /// <returns>The <see cref="Image"/> to render to the <see cref="Display"/>.</returns>
        Image GetImage();
    }
}
