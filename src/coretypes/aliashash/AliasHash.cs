using System.Collections;
using System.Diagnostics.CodeAnalysis;
namespace SCE
{
    public class AliasHash<T> : IEnumerable<T>
        where T : notnull
    {
        private readonly Dictionary<T, HashSet<string>> _set;

        private readonly Dictionary<string, T> _aliases = new();

        public AliasHash()
        {
            _set = new();
        }

        public AliasHash(int capacity)
        {
            _set = new(capacity);
        }

        public AliasHash(IEnumerable<T> collection)
            : this()
        {
            AddRange(collection);
        }

        public T this[string alias]
        {
            get => Get(alias);
            set => Set(alias, value);
        }

        public int Count { get => _set.Count; }

        public int AliasCount { get => _aliases.Count; }

        #region Modify

        public virtual bool Add(T item)
        {
            if (_set.ContainsKey(item))
                return false;
            _set[item] = new();
            return true;
        }

        public virtual bool Add(T item, string alias)
        {
            if (!Add(item))
                return false;
            if (_aliases.ContainsKey(alias))
                throw new ArgumentException($"Alias {alias} already exists.");
            _set[item].Add(alias);
            _aliases[alias] = item;
            return true;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void AddRange(IEnumerable<(T, string)> items)
        {
            foreach ((var item, string alias) in items)
                Add(item, alias);
        }

        public void AddEvery(params T[] items)
        {
            AddRange(items);
        }

        public void AddEvery(params (T, string)[] items)
        {
            AddRange(items);
        }

        public virtual bool Remove(T item)
        {
            if (!_set.TryGetValue(item, out var aliases))
                return false;
            foreach (var alias in aliases)
                _aliases.Remove(alias);
            return _set.Remove(item);
        }

        public virtual bool Remove(string alias)
        {
            if (!_aliases.TryGetValue(alias, out var item))
                return false;
            Remove(item);
            return true;
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Remove(item);
        }

        public virtual void Clear()
        {
            _set.Clear();
            _aliases.Clear();
        }

        #endregion

        #region Accessors

        public virtual T Get(string alias)
        {
            return _aliases[alias];
        }

        public virtual U Get<U>(string alias)
        {
            var res = (U?)Convert.ChangeType(Get(alias), typeof(U));
            return res ?? throw new NullReferenceException("Conversion result is null.");
        }

        public virtual bool TryGet(string alias, [MaybeNullWhen(false)] out T? item)
        {
            return _aliases.TryGetValue(alias, out item);
        }

        public virtual bool TryGet<U>(string alias, [MaybeNullWhen(false)] out U? item)
        {
            if (!_aliases.TryGetValue(alias, out var val))
            {
                item = default;
                return false;
            }
            item = (U?)Convert.ChangeType(val, typeof(U));
            return item != null;
        }

        public virtual void Set(string alias, T item)
        {
            _aliases[alias] = item;
        }

        #endregion

        #region Contains

        public virtual bool Contains(T item)
        {
            return _set.ContainsKey(item);
        }

        public virtual bool Contains(string alias)
        {
            return _aliases.ContainsKey(alias);
        }

        #endregion

        #region Aliases

        public virtual void Rename(string oldAlias, string newAlias)
        {
            if (!_aliases.TryGetValue(oldAlias, out var item))
                throw new ArgumentException($"No item with alias {oldAlias} found.");
            if (_aliases.ContainsKey(newAlias))
                throw new ArgumentException($"Alias {newAlias} already exists.");
            _aliases[newAlias] = item;
            _set[item].Add(newAlias);
            _aliases.Remove(oldAlias);
            _set[item].Remove(oldAlias);
        }

        public virtual bool RemoveAlias(string alias)
        {
            if (!_aliases.TryGetValue(alias, out var item))
                return false;
            _set[item].Remove(alias);
            return _aliases.Remove(alias);
        }

        #endregion

        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return _set.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
