namespace SCE
{
    /// <summary>
    /// An extension class of <see cref="SearchHash{T}"/> allowing for quick determination of whether this contains an element of a specified type.
    /// </summary>
    public class SearchHashTypeExt<T> : SearchHash<T>
        where T : ISearcheable
    {
        /// <summary>
        /// Contains each type and the count of each type. 
        /// </summary>
        protected readonly Dictionary<Type, int> typeDict;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchHashTypeExt{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the <see cref="SearchHashTypeExt{T}"/>.</param>
        public SearchHashTypeExt(IEnumerable<T>? collection = null)
            : base()
        {
            typeDict = new();
            if (collection is not null)
                AddRange(collection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchHashTypeExt{T}"/> class.
        /// </summary>
        /// <param name="capacity">The initial size of the <see cref="SearchHashTypeExt{T}"/>.</param>
        public SearchHashTypeExt(int capacity)
            : base(capacity)
        {
            typeDict = new(capacity);
        }

        #endregion

        /// <inheritdoc/>
        public override bool Add(T item)
        {
            AddType(item.GetType());
            return base.Add(item);
        }

        /// <inheritdoc/>
        public override bool Remove(T item)
        {
            RemoveType(item.GetType());
            return base.Remove(item);
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            base.Clear();
            typeDict.Clear();
        }

        /// <summary>
        /// Adds a type to the type dict.
        /// </summary>
        /// <param name="type">The type to add.</param>
        protected void AddType(Type type)
        {
            if (typeDict.ContainsKey(type))
                ++typeDict[type];
            else
                typeDict.Add(type, 1);
        }

        /// <summary>
        /// Removes a type from the type dict.
        /// </summary>
        /// <param name="type">The type to remove.</param>
        protected void RemoveType(Type type)
        {
            if (!typeDict.TryGetValue(type, out int value))
                return;
            if (value <= 1)
                typeDict.Remove(type);
            else
                --typeDict[type];
        }

        #region Search

        /// <summary>
        /// Determines whether the search hash contains a specified type.
        /// </summary>
        /// <typeparam name="U">The type to search for.</typeparam>
        /// <returns><see langword="true"/> if the type is found; otherwise, <see langword="false"/>.</returns>
        public bool Contains<U>()
        {
            return Contains(typeof(U));
        }

        /// <summary>
        /// Determines whether the search hash contains a specified type.
        /// </summary>
        /// <param name="type">The type to search for.</param>
        /// <returns><see langword="true"/> if the type is found; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Type type)
        {
            return typeDict.TryGetValue(type, out int value) && value > 0;
        }

        #endregion
    }
}
