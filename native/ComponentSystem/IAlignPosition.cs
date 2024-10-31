namespace SCECore.Interfaces
{
    using SCECore.ComponentSystem;

    /// <summary>
    /// An <see cref="IComponent"/> interface which aligns a <see cref="Vector2"/> position.
    /// </summary>
    internal interface IAlignPositionInt : IComponent
    {
        /// <summary>
        /// Gets the aligned <see cref="Vector2"/> position.
        /// </summary>
        public Vector2Int AlignedPosition { get; }
    }
}
