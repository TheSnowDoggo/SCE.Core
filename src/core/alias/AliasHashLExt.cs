namespace SCE
{
    /// <summary>
    /// An extension class of <see cref="SearchHash{T}"/> with a list.
    /// </summary>
    public class AliasHashLExt<T> : AliasHash<T>
    {
        /// <summary>
        /// The list of items.
        /// </summary>
        protected readonly List<T> _list;

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
            _list = new(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasHashLExt{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection of initial elements.</param>
        public AliasHashLExt(IEnumerable<T> collection)
            : base(collection)
        {
            _list = new();
        }

        /// <inheritdoc/>
        public override bool Add(T t)
        {
            _list.Add(t);
            return base.Add(t);
        }

        /// <inheritdoc/>
        public override bool Remove(T t)
        {
            _list.Remove(t);
            return base.Remove(t);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            base.Clear();
            _list.Clear();
        }
    }
}
