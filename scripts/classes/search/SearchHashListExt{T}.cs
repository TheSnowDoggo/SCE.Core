namespace SCE
{
    /// <summary>
    /// An extension class of <see cref="SearchHash{T}"/> with a list.
    /// </summary>
    public class SearchHashListExt<T> : SearchHash<T>
        where T : ISearcheable
    {
        /// <summary>
        /// The list.
        /// </summary>
        protected readonly List<T> list;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchHashListExt{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the <see cref="SearchHashListExt{T}"/>.</param>
        public SearchHashListExt(IEnumerable<T>? collection = null)
            : base()
        {
            list = new();
            if (collection is not null)
                AddRange(collection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchHashListExt{T}"/> class.
        /// </summary>
        /// <param name="capacity">The initial size of the <see cref="SearchHashListExt{T}"/>.</param>
        public SearchHashListExt(int capacity)
            : base(capacity)
        {
            list = new(capacity);
        }

        #endregion

        /// <inheritdoc/>
        public override bool Add(T t)
        {
            list.Add(t);
            return base.Add(t);
        }

        /// <inheritdoc/>
        public override bool Remove(T t)
        {
            list.Remove(t);
            return base.Remove(t);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            base.Clear();
            list.Clear();
        }
    }
}
