namespace SCE
{
    /// <summary>
    /// An abstract base class for scenes.
    /// </summary>
    public class SceneBase : IScene
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
        public virtual void Start()
        {
        }

        /// <inheritdoc/>
        public virtual void Update()
        {
        }
    }
}
