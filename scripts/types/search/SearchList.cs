namespace SCE
{
    using System;
    using System.Collections;

    public class SearchList<T> : IEnumerable<T>
        where T : ISearcheable
    {
        protected readonly List<T> _list;

        public SearchList(List<T> list)
        {
            _list = list;
        }
        public SearchList()
            : this(new List<T>())
        {
        }

        public int Count { get => _list.Count; }

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region List
        public void Add(T t)
        {
            _list.Add(t);
        }

        public void Add(T[] tArray)
        {
            foreach (T t in tArray)
                Add(t);
        }

        public void Add(List<T> tList)
        {
            foreach (T t in tList)
                Add(t);
        }

        public bool Remove(T t)
        {
            return _list.Remove(t);
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
        public int IndexOf(T t)
        {
            return _list.IndexOf(t);
        }

        public bool Contains(T t)
        {
            return _list.Contains(t);
        }

        public int IndexOf(Func<T, bool> func)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (func.Invoke(_list[i]))
                    return i;
            }
            return -1;
        }

        public int IndexOf(string name)
        {
            return IndexOf((t) => t.Name == name);
        }

        public bool Contains(Func<T, bool> func, out int index)
        {
            index = IndexOf(func);
            return index != -1;
        }

        public bool Contains(Func<T, bool> func)
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

        public T Get(Func<T, bool> func)
        {
            if (!Contains(func, out int index))
                throw new SearchNotFoundException("No matching element found.");
            return _list[index];
        }

        public T Get(string name)
        {
            if (!Contains(name, out int index))
                throw new SearchNotFoundException($"No element with name \"{name}\" found.");
            return _list[index];
        }

        public U Get<U>(Func<U, bool> func)
            where U : ISearcheable
        {
            if (Get(func) is not U u)
                throw new InvalidCastException("Cannot cast to given type.");
            return u;
        }

        public U Get<U>(string name)
            where U : ISearcheable
        {
            if (Get(name) is not U u)
                throw new InvalidCastException("Cannot cast to given type.");
            return u;
        }

        public U GetFirst<U>()
        {
            foreach (T t in _list)
            {
                if (t is U u)
                    return u;
            }
            throw new SearchNotFoundException("No element of type U found.");
        }
        #endregion
    }
}
