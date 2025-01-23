using System.Collections;

namespace SCE
{
    public class SearchHash<T> : IEnumerable<T>
        where T : ISearcheable
    {
        protected readonly HashSet<T> _hashSet;

        protected readonly Dictionary<string, T> _nameDict;

        private int uniqueId = 0;

        public SearchHash(int capacity)
        {
            _hashSet = new(capacity);
            _nameDict = new(capacity);
        }

        public SearchHash()
            : this(0)
        {
        }

        public SearchHash(IEnumerable<T> collection)
            : this()
        {
            AddRange(collection);
        }

        protected Action<T>? OnAdd;

        protected Action<T>? OnRemove;

        protected Action? OnClear;

        public int Count { get => _hashSet.Count; }

        public bool IsEmpty { get => Count == 0; }

        public bool AssignUniqueName { get; set; } = true;

        public IEnumerator<T> GetEnumerator()
        {
            return _hashSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Add(T t)
        {
            if (_nameDict.ContainsKey(t.Name))
            {
                if (AssignUniqueName)
                    t.Name = $"{t.Name}_{++uniqueId}";
                else
                    throw new DuplicateNameException("Duplicate names not allowed.");
            }
            _nameDict.Add(t.Name, t);
            _hashSet.Add(t);
            OnAdd?.Invoke(t);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var t in collection)
                Add(t);
        }

        public virtual bool Remove(T t)
        {
            _nameDict.Remove(t.Name);
            bool result = _hashSet.Remove(t);
            OnRemove?.Invoke(t);
            return result;
        }

        public virtual void Clear()
        {
            _hashSet.Clear();
            _nameDict.Clear();
        }

        #region Search
        public bool Contains(T t)
        {
            return _hashSet.Contains(t);
        }

        public bool Contains(string name)
        {
            return _nameDict.ContainsKey(name);
        }

        public bool Contains(string name, out T? value)
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
