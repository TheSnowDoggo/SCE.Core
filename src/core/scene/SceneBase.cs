namespace SCE
{
    /// <summary>
    /// An abstract base class for scenes.
    /// </summary>
    public class SceneBase : IScene
    {
        /// <inheritdoc/>
        public bool IsActive { get; set; } = true;

        /// <inheritdoc/>
        public virtual void Start()
        {
        }

        /// <inheritdoc/>
        public virtual void Update()
        {
        }
    }
}
