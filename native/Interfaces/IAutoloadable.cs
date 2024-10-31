namespace SCECore.Interfaces
{
    /// <summary>
    /// Interface for classes that implement Start and Update methods.
    /// Loaded into and called by GameHandler.
    /// </summary>
    public interface IAutoloadable
    {
        /// <summary>
        /// Called when GameHandler is started.
        /// </summary>
        void Start();

        /// <summary>
        /// Called every frame before the display is rendered.
        /// </summary>
        void Update();

        /// <summary>
        /// Called every frame after the display is rendered.
        /// </summary>
        void LateUpdate()
        {
        }
    }
}
