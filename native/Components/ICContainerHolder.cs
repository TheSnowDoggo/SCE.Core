namespace SCECore.Components
{
    /// <summary>
    /// An interface between the CContainer and its holder instance.
    /// </summary>
    public interface ICContainerHolder
    {
        /// <summary>
        /// Gets the CContainer.
        /// </summary>
        CContainer CContainer { get; }
    }
}
