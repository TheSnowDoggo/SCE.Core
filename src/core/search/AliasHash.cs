using System.Collections;
using System.Diagnostics.CodeAnalysis;
namespace SCE
{
    public class AliasHash<T> : IEnumerable<T>
    {
        private readonly HashSet<T> _set;

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
        {
            _set = new(collection);
        }

        public int Count { get => _set.Count; }

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[string alias]
        {
            get => Get(alias);
            set => Set(alias, value);
        }

        public virtual bool Add(T item)
        {
            return _set.Add(item);
        }

        public virtual bool Add(T item, string alias)
        {
            if (!Add(item))
                return false;
            if (_aliases.ContainsKey(alias))
                throw new ArgumentException($"Alias {alias} already exists.");
            _aliases[alias] = item;
            return true;
        }

        public virtual void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public virtual void AddRange(IEnumerable<(T, string)> items)
        {
            foreach ((var item, string alias) in items)
                Add(item, alias);
        }

        public virtual bool Remove(T item)
        {
            return _set.Remove(item);
        }

        public virtual bool Remove(string alias)
        {
            if (!_aliases.TryGetValue(alias, out var item))
                return false;
            _aliases.Remove(alias);
            _set.Remove(item);
            return true;
        }

        public virtual T Get(string alias)
        {
            return _aliases[alias];
        }

        public virtual void Set(string alias, T item)
        {
            _aliases[alias] = item;
        }

        public virtual bool TryGet(string alias, [MaybeNullWhen(false)] out T? item)
        {
            return _aliases.TryGetValue(alias, out item);
        }

        public virtual void Rename(string oldAlias, string newAlias)
        {
            if (!_aliases.TryGetValue(oldAlias, out var item))
                throw new ArgumentException($"No item with alias {oldAlias} found.");
            if (_aliases.ContainsKey(newAlias))
                throw new ArgumentException($"Alias {newAlias} already exists.");
            _aliases[newAlias] = item;
            _aliases.Remove(oldAlias);
        }
    }
}
