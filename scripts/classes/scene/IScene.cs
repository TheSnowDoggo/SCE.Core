namespace SCE
{
    /// <summary>
    /// An interface for classes which receive a start and update calls.
    /// </summary>
    public interface IScene : IUpdate
    {
        /// <summary>
        /// Called on start.
        /// </summary>
        void Start();
    }
}
