namespace SCE
{
    using System.Collections;

    internal class SearchList : IEnumerable<ISearcheable>
    {
        private readonly List<ISearcheable> _list;

        public SearchList(List<ISearcheable> list)
        {
            _list = list;
        }
        public SearchList()
            : this(new List<ISearcheable>())
        {
        }

        public int Count { get => _list.Count; }

        public ISearcheable this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public IEnumerator<ISearcheable> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region List
        public void Add(ISearcheable iSearcheable)
        {
            _list.Add(iSearcheable);
        }

        public void Add(ISearcheable[] iSearcheableArray)
        {
            foreach (ISearcheable iSearcheable in iSearcheableArray)
                Add(iSearcheable);
        }

        public void Add(List<ISearcheable> iSearcheableList)
        {
            foreach (ISearcheable searcheable in iSearcheableList)
                Add(searcheable);
        }

        public bool Remove(ISearcheable iSearcheable)
        {
            return _list.Remove(iSearcheable);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void Clear()
        {
            _list.Clear();
        }
        #endregion

        #region Search
        public bool Contains(Func<ISearcheable, bool> func, out int index)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (func.Invoke(_list[i]))
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        public bool Contains(Func<ISearcheable, bool> func)
        {
            return Contains(func, out _);
        }

        public bool Contains(string name, out int index)
        {
            return Contains((scene) => scene.Name == name, out index);
        }

        public bool Contains(string name)
        {
            return Contains(name, out _);
        }

        public ISearcheable Get(Func<ISearcheable, bool> func)
        {
            if (!Contains(func, out int index))
                throw new SearchNotFoundException("No ISearcheable found.");
            return _list[index];
        }

        public ISearcheable Get(string name)
        {
            if (!Contains(name, out int index))
                throw new SearchNotFoundException($"No ISearcheable with name \"{name}\" found.");
            return _list[index];
        }

        public T Get<T>(Func<ISearcheable, bool> func)
            where T : ISearcheable
        {
            if (Get(func) is not T t)
                throw new InvalidCastException("ISearcheable cannot be cast to given type.");
            return t;
        }

        public T Get<T>(string name)
            where T : ISearcheable
        {
            if (Get(name) is not T t)
                throw new InvalidCastException("ISearcheable cannot be cast to given type.");
            return t;
        }
        #endregion
    }
}
