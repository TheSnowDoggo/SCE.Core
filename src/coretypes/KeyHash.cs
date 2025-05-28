using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SCE
{
    public class KeyHash<TKey, TValue> : IEnumerable<KeyValuePair<TKey, HashSet<TValue>>>
        where TKey : notnull
    {
        private readonly Dictionary<TKey, HashSet<TValue>> _dict;

        public KeyHash()
        {
            _dict = new();
        }

        public KeyHash(int capacity)
        {
            _dict = new(capacity);
        }

        public KeyHash(IEnumerable<KeyValuePair<TKey, HashSet<TValue>>> collection)
        {
            _dict = new(collection);
        }

        public int Count { get => _dict.Count; }

        public int TotalCount() { return _dict.Values.Sum(s => s.Count); }

        public HashSet<TValue> this[TKey key]
        {
            get
            {
                if (_dict.TryGetValue(key, out var set))
                {
                    return set;
                }
                return _dict[key] = new();
            }
            set => _dict[key] = value;
        }

        public bool Add(TKey key, TValue value)
        {
            return this[key].Add(value);
        }

        public void AddRange(TKey key, IEnumerable<TValue> collection)
        {
            foreach (var value in collection)
            {
                Add(key, value);
            }
        }

        public bool Remove(TKey key)
        {
            return _dict.Remove(key);
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out HashSet<TValue> set)
        {
            return _dict.TryGetValue(key, out set);
        }

        #region IEnumerable

        public IEnumerator<KeyValuePair<TKey, HashSet<TValue>>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
