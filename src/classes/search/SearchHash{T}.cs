using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SCE
{
    /// <summary>
    /// A class for storing a collection of <see cref="ISearcheable"/> elements with efficient hashed lookup.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchHash<T> : IEnumerable<T>
        where T : ISearcheable
    {
        /// <summary>
        /// Contains the elements.
        /// </summary>
        protected readonly HashSet<T> hashSet;

        /// <summary>
        /// Contains the names and associated elements.
        /// </summary>
        protected readonly Dictionary<string, T> nameDict;

        /// <summary>
        /// Contains the unique ids for each name for handling duplicate names.
        /// </summary>
        protected readonly Dictionary<string, int> nameIdDict;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchHash{T}"/> class.
        /// </summary>
        /// <param name="capacity">The initial size of the <see cref="SearchHash{T}"/>.</param>
        public SearchHash(int capacity)
        {
            hashSet = new(capacity);
            nameDict = new(capacity);
            nameIdDict = new(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchHash{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the <see cref="SearchHash{T}"/>.</param>
        public SearchHash(IEnumerable<T>? collection = null)
            : this(0)
        {
            if (collection is not null)
                AddRange(collection);
        }

        #endregion

        /// <summary>
        /// Gets the number of elements that are contained in the search hash.
        /// </summary>
        public int Count { get => hashSet.Count; }

        /// <summary>
        /// Gets or sets a value indicating whether added values with duplicate names should be assigned new unique identifiers.
        /// </summary>
        public bool AssignUniqueName { get; set; } = true;

        #region Indexers

        /// <summary>
        /// Gets or sets the element with the specified name.
        /// </summary>
        /// <param name="name">The name of the element to get or set.</param>
        /// <returns>The element with the specified name.</returns>
        public T this[string name]
        {
            get => nameDict[name];
            set => nameDict[name] = value;
        }

        /// <summary>
        /// Gets a value indicating whether this search hash contains the specified element.
        /// </summary>
        /// <param name="key">The element to check for.</param>
        /// <returns><see langword="true"/> if the search hash contains the specified element; otherwise, <see langword="false"/>.</returns>
        public bool this[T key]
        {
            get => hashSet.Contains(key);
        }

        #endregion

        #region Enumerator

        /// <inheritdoc/>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return hashSet.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Modification

        public virtual bool SetIf(T item, bool condition)
        {
            if (condition)
                return Add(item);
            Remove(item);
            return false;
        }

        /// <summary>
        /// Adds the specified element to the search hash.
        /// </summary>
        /// <param name="item">The element to add.</param>
        /// <returns><see langword="true"/> if the element was added sucessfully; otherwise, <see langword="false"/>.</returns>
        public virtual bool Add(T item)
        {
            if (hashSet.Contains(item))
                return false;
            if (nameIdDict.ContainsKey(item.Name))
            {
                item.Name = AssignUniqueName ? $"{item.Name}_{++nameIdDict[item.Name]}"
                    : throw new DuplicateNameException("Duplicate names not allowed.");
            }
            nameDict.Add(item.Name, item);
            nameIdDict.Add(item.Name, 0);
            hashSet.Add(item);
            return true;
        }

        /// <summary>
        /// Adds the specified collection of elements to the search hash.
        /// </summary>
        /// <param name="collection">The collection of elements to add.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                Add(item);
        }

        /// <summary>
        /// Adds every specified element to the search hash.
        /// </summary>
        /// <param name="arr">The array of elements to add.</param>
        public virtual void AddEvery(params T[] arr)
        {
            AddRange(arr);
        }

        /// <summary>
        /// Adds every specified collection of elements to the search hash.
        /// </summary>
        /// <param name="arr">The array array of elements to add.</param>
        public virtual void AddEvery(params T[][] arr)
        {
            foreach (var range in arr)
                AddRange(range);
        }

        /// <summary>
        /// Removes the specified element from the search hash.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns><see langword="true"/> if the element is sucessfully found and removed; otherwise, <see langword="false"/>.</returns>
        public virtual bool Remove(T item)
        {
            if (!hashSet.Remove(item))
                return false;
            nameDict.Remove(item.Name);
            if (--nameIdDict[item.Name] <= 0)
                nameIdDict.Remove(item.Name);
            return true;
        }

        /// <summary>
        /// Removes the element with the specified name from the search hash.
        /// </summary>
        /// <param name="name">The name of the element to remove.</param>
        /// <returns><see langword="true"/> if the element is sucessfully found and removed; otherwise, <see langword="false"/>.</returns>
        public virtual bool Remove(string name)
        {
            if (Contains(name, out T? value))
                return Remove(value);
            return false;
        }

        /// <summary>
        /// Removes the specified collection of elements from the search hash.
        /// </summary>
        /// <param name="collection">The collection of elements to remove.</param>
        public void RemoveRange(IEnumerable<T> collection)
        {
            foreach (var t in collection)
                Remove(t);
        }

        /// <summary>
        /// Removes all elements from the search hash.
        /// </summary>
        public virtual void Clear()
        {
            hashSet.Clear();
            nameDict.Clear();
            nameIdDict.Clear();
        }

        #endregion

        #region Search

        /// <summary>
        /// Determines whether the search hash contains the specified element.
        /// </summary>
        /// <param name="item">The element to search for.</param>
        /// <returns><see langword="true"/> if the element is found; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item)
        {
            return hashSet.Contains(item);
        }

        /// <summary>
        /// Determines whether the search hash contains the element with the specified name.
        /// </summary>
        /// <param name="name">The name of the element to search for.</param>
        /// <returns><see langword="true"/> if the element is found; otherwise, <see langword="false"/>.</returns>
        public bool Contains(string name)
        {
            return nameDict.ContainsKey(name);
        }

        /// <summary>
        /// Determines whether the search hash contains the element with the specified name and outputs the value.
        /// </summary>
        /// <param name="name">The name of the element to search for.</param>
        /// <param name="value">The element found with the specified <paramref name="name"/>.</param>
        /// <returns><see langword="true"/> if the element is found; otherwise, <see langword="false"/>.</returns>
        public bool Contains(string name, [MaybeNullWhen(false)] out T value)
        {
            return nameDict.TryGetValue(name, out value);
        }

        /// <summary>
        /// Returns the element with the specified name.
        /// </summary>
        /// <param name="name">The name of the element to search for.</param>
        /// <returns>The element with the specified name</returns>
        public T Get(string name)
        {
            if (!nameDict.TryGetValue(name, out T? value))
                throw new SearchNotFoundException("No element with given name found.");
            return value;
        }

        /// <summary>
        /// Returns the element with the specified name.
        /// </summary>
        /// <param name="name">The name of the element to search for.</param>
        /// <typeparam name="U">The type to cast the found element to.</typeparam>
        /// <returns>The element with the specified name</returns>
        public U Get<U>(string name)
        {
            if (Get(name) is not U u)
                throw new InvalidCastException("Cannot cast to given type.");
            return u;
        }

        #endregion
    }
}
