namespace SCE
{
    /// <summary>
    /// A class for storing <see cref="IUpdate"/>.
    /// </summary>
    public class UpdateGroup : AliasHashTExt<IUpdate>, IUpdate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGroup"/> class.
        /// </summary>
        public UpdateGroup()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateGroup"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the <see cref="UpdateGroup"/>.</param>
        public UpdateGroup(IEnumerable<IUpdate> collection)
            : base(collection)
        {
        }

        /// <inheritdoc/>
        public bool IsActive { get; set; }

        /// <inheritdoc/>
        public void Update()
        {
            foreach (var update in this)
            {
                if (update.IsActive)
                {
                    update.Update();
                }
            }
        }
    }
}
