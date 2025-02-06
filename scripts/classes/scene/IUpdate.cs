namespace SCE
{
    /// <summary>
    /// An interface for classes which receive update calls.
    /// </summary>
    public interface IUpdate : ISearcheable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this should receive updates.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Called on update (every frame).
        /// </summary>
        void Update();
    }
}
