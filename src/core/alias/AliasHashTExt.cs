namespace SCE
{
    /// <summary>
    /// An extension class of <see cref="AliasHash{T}"/> allowing for quick determination of whether this contains an element of a specified type.
    /// </summary>
    public class AliasHashTExt<T> : AliasHash<T>
    {
        /// <summary>
        /// Contains each type and the count of each type. 
        /// </summary>
        protected readonly Dictionary<Type, int> _types;

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasHashTExt{T}"/> class.
        /// </summary>
        public AliasHashTExt()
            : base()
        {
            _types = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasHashTExt{T}"/> class.
        /// </summary>
        /// <param name="capacity">The initial size.</param>
        public AliasHashTExt(int capacity)
            : base(capacity)
        {
            _types = new(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasHashTExt{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection of initial elements.</param>
        public AliasHashTExt(IEnumerable<T> collection)
            : base(collection)
        {
            _types = new();
        }

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
            _types.Clear();
        }

        /// <summary>
        /// Adds a type to the type dict.
        /// </summary>
        /// <param name="type">The type to add.</param>
        protected void AddType(Type type)
        {
            if (_types.ContainsKey(type))
            {
                ++_types[type];
            }
            else
            {
                _types.Add(type, 1);
            }
        }

        /// <summary>
        /// Removes a type from the type dict.
        /// </summary>
        /// <param name="type">The type to remove.</param>
        protected void RemoveType(Type type)
        {
            if (!_types.TryGetValue(type, out int value))
                return;
            if (value <= 1)
            {
                _types.Remove(type);
            }
            else
            {
                --_types[type];
            }
        }

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
            return _types.TryGetValue(type, out int value) && value > 0;
        }
    }
}
