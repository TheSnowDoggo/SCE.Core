namespace SCECore
{
    /// <summary>
    /// Interface for programs that contain multiple IAutoloadbles to be loaded into GameHandler.
    /// </summary>
    public interface IProgram
    {
        /// <summary>
        /// Gets a list of all IAutoloadables in this instance.
        /// </summary>
        public List<IAutoloadable> AutoloadableList { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        public bool IsActive { get; }
    }
}
