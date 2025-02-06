namespace SCE
{
    /// <summary>
    /// An abstract base class for scenes.
    /// </summary>
    public abstract class SceneBase : IScene
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneBase"/> class.
        /// </summary>
        /// <param name="name">The initial name of the scene.</param>
        public SceneBase(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneBase"/> class.
        /// </summary>
        public SceneBase()
            : this(string.Empty)
        {
        }

        #endregion

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public bool IsActive { get; set; } = true;

        /// <inheritdoc/>
        public abstract void Start();

        /// <inheritdoc/>
        public abstract void Update();
    }
}
