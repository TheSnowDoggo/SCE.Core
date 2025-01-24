namespace SCE
{
    public class SearchHashTypeExt<T> : SearchHash<T>
        where T : ISearcheable
    {
        protected readonly Dictionary<Type, int> _typeDict = new();

        public SearchHashTypeExt(IEnumerable<T> collection)
            : base()
        {
            _typeDict = new();
            AddRange(collection);
        }

        public SearchHashTypeExt(int capacity)
            : base(capacity)
        {
            _typeDict = new(capacity);
        }

        public SearchHashTypeExt()
            : base()
        {
            _typeDict = new();
        }

        public override void Add(T t)
        {
            base.Add(t);
            AddType(t.GetType());
        }

        public override bool Remove(T t)
        {
            RemoveType(t.GetType());
            return base.Remove(t);
        }

        public override void Clear()
        {
            base.Clear();
            _typeDict.Clear();
        }

        protected void AddType(Type type)
        {
            if (_typeDict.ContainsKey(type))
                ++_typeDict[type];
            else
                _typeDict.Add(type, 1);
        }

        protected void RemoveType(Type type)
        {
            if (!_typeDict.TryGetValue(type, out int value))
                return;
            if (value <= 1)
                _typeDict.Remove(type);
            else
                --_typeDict[type];
        }

        #region Search
        public bool Contains<U>()
        {
            return Contains(typeof(U));
        }

        public bool Contains(Type type)
        {
            return _typeDict.TryGetValue(type, out int value) && value > 0;
        }
        #endregion
    }
}
