using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SCE
{
    public class SearchHash<T> : IEnumerable<T>
        where T : ISearcheable
    {
        protected readonly HashSet<T> _hashSet;

        protected readonly Dictionary<string, T> _nameDict;

        protected readonly Dictionary<string, int> _nameIdDict;

        #region Constructors

        public SearchHash(int capacity)
        {
            _hashSet = new(capacity);
            _nameDict = new(capacity);
            _nameIdDict = new(capacity);
        }

        public SearchHash(IEnumerable<T>? collection = null)
            : this(0)
        {
            if (collection is not null)
                AddRange(collection);
        }

        #endregion

        public int Count { get => _hashSet.Count; }

        public bool IsEmpty { get => Count == 0; }

        public bool AssignUniqueName { get; set; } = true;

        #region Indexers
        public T this[string name]
        {
            get => _nameDict[name];
            set => _nameDict[name] = value;
        }

        public bool this[T key]
        {
            get => _hashSet.Contains(key);
        }
        #endregion

        #region Enumerator
        public virtual IEnumerator<T> GetEnumerator()
        {
            return _hashSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Modification
        public virtual bool SetIf(T t, bool condition)
        {
            if (condition)
                return Add(t);
            Remove(t);
            return false;
        }

        public virtual bool Add(T t)
        {
            if (_hashSet.Contains(t))
                return false;
            if (_nameIdDict.ContainsKey(t.Name))
            {
                t.Name = AssignUniqueName ? $"{t.Name}_{++_nameIdDict[t.Name]}"
                    : throw new DuplicateNameException("Duplicate names not allowed.");
            }
            _nameDict.Add(t.Name, t);
            _nameIdDict.Add(t.Name, 0);
            _hashSet.Add(t);
            return true;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var t in collection)
                Add(t);
        }

        public virtual void AddEvery(params T[] t)
        {
            AddRange(t);
        }

        public virtual void AddEvery(params T[][] t)
        {
            foreach (var range in t)
                AddRange(range);
        }

        public virtual bool Remove(T t)
        {
            if (!_hashSet.Remove(t))
                return false;
            _nameDict.Remove(t.Name);
            if (--_nameIdDict[t.Name] <= 0)
                _nameIdDict.Remove(t.Name);
            return true;
        }

        public virtual bool Remove(string name)
        {
            if (Contains(name, out T? value))
                return Remove(value);
            return false;
        }

        public void RemoveRange(IEnumerable<T> collection)
        {
            foreach (var t in collection)
                Remove(t);
        }

        public virtual void Clear()
        {
            _hashSet.Clear();
            _nameDict.Clear();
            _nameIdDict.Clear();
        }
        #endregion

        #region Search
        public bool Contains(T t)
        {
            return _hashSet.Contains(t);
        }

        public bool Contains(string name)
        {
            return _nameDict.ContainsKey(name);
        }

        public bool Contains(string name, [MaybeNullWhen(false)] out T value)
        {
            return _nameDict.TryGetValue(name, out value);
        }

        public T Get(string name)
        {
            if (!_nameDict.TryGetValue(name, out T? value))
                throw new SearchNotFoundException("No element with given name found.");
            return value;
        }

        public U Get<U>(string name)
        {
            if (Get(name) is not U u)
                throw new InvalidCastException("Cannot cast to given type.");
            return u;
        }
        #endregion
    }
}
