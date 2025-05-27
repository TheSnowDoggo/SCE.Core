namespace SCE
{
    /// <summary>
    /// An extension class of <see cref="AliasHash{T}"/> allowing for quick determination of whether this contains an element of a specified type.
    /// </summary>
    public class AliasHashTExt<T> : AliasHash<T>
        where T : notnull
    {
        private readonly Dictionary<Type, HashSet<T>> _types = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="AliasHashTExt{T}"/> class.
        /// </summary>
        public AliasHashTExt()
            : base()
        {
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
        }

        #region Modify

        /// <inheritdoc/>
        public override bool Add(T item)
        {
            var res = base.Add(item);
            if (res)
                AddType(item, item.GetType());
            return res;
        }

        /// <inheritdoc/>
        public override bool Remove(T item)
        {
            var res = base.Remove(item);
            if (res)
                foreach (var pair in _types)
                    RemoveType(item, pair.Key);
            return res;
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            base.Clear();
            _types.Clear();
        }

        #endregion

        #region Contains

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
            return type == typeof(T) || (_types.TryGetValue(type, out var set) && set.Count > 0);
        }

        #endregion

        #region Types

        public IEnumerable<U> EnumerateType<U>()
        {
            if (_types.TryGetValue(typeof(U), out var set))
            {
                foreach (var item in set.Cast<U>())
                {
                    yield return item;
                }
            }
        }

        public bool AddType(T item, Type type)
        {
            if (!Contains(item) || type == typeof(T) || !type.IsAssignableFrom(item.GetType()))
            {
                return false;
            }

            if (_types.TryGetValue(type, out var set))
            {
                return set.Add(item);
            }
            else
            {
                _types[type] = new(new[] { item });
                return true;
            }
        }

        public bool RemoveType(T item, Type type)
        {
            if (!Contains(item) || type == typeof(T) || !type.IsAssignableFrom(item.GetType()))
            {
                return false;
            }

            if (_types.TryGetValue(type, out var set) && set.Remove(item))
            {
                if (set.Count == 0)
                {
                    _types.Remove(type);
                }
                return true;
            }

            return false;
        }

        public void RemoveTypes(T item)
        {
            foreach (var type in _types.Keys)
            {
                RemoveType(item, type);
            }
        }

        #endregion
    }
}
