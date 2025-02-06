namespace SCE
{
    /// <summary>
    /// A class for storing <see cref="IUpdate"/>.
    /// </summary>
    public class UpdateGroup : SearchHashTypeExt<IUpdate>, IUpdate
    {
        private const string DEFAULT_NAME = "update_group";

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGroup"/> class.
        /// </summary>
        /// <param name="name">The initial name of the update group.</param>
        /// <param name="collection">The collection whose elements are copied to the <see cref="UpdateGroup"/>.</param>
        public UpdateGroup(string name, IEnumerable<IUpdate>? collection = null)
            : base(collection)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGroup"/> class.
        /// </summary>
        public UpdateGroup(IEnumerable<IUpdate>? collection = null)
            : this(DEFAULT_NAME, collection)
        {
        }

        #endregion

        /// <inheritdoc/>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc/>
        public bool IsActive { get; set; }

        /// <inheritdoc/>
        public void Update()
        {
            foreach (var update in this)
            {
                if (update.IsActive)
                    update.Update();
            }
        }
    }
}
