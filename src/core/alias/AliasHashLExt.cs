namespace SCE
{
    /// <summary>
    /// An extension class of <see cref="SearchHash{T}"/> with a list.
    /// </summary>
    public class AliasHashLExt<T> : AliasHash<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AliasHashLExt{T}"/> class.
        /// </summary>
        public AliasHashLExt()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasHashLExt{T}"/> class.
        /// </summary>
        /// <param name="capacity">The initial size.</param>
        public AliasHashLExt(int capacity)
            : base(capacity)
        {
            List = new(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasHashLExt{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection of initial elements.</param>
        public AliasHashLExt(IEnumerable<T> collection)
            : base(collection)
        {
        }

        protected List<T> List { get; } = new();

        #region Modify

        /// <inheritdoc/>
        public override bool Add(T t)
        {
            List.Add(t);
            return base.Add(t);
        }

        /// <inheritdoc/>
        public override bool Remove(T t)
        {
            List.Remove(t);
            return base.Remove(t);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            base.Clear();
            List.Clear();
        }

        #endregion
    }
}
