namespace SCE
{
    public class SearchHashTypeExt<T> : SearchHash<T>
        where T : ISearcheable
    {
        protected readonly HashSet<Type> _typeSet = new();

        public SearchHashTypeExt(IEnumerable<T> collection)
            : base()
        {
            _typeSet = new();
            AddRange(collection);
        }

        public SearchHashTypeExt(int capacity)
            : base(capacity)
        {
            _typeSet = new(capacity);
        }

        public SearchHashTypeExt()
            : base()
        {
            _typeSet = new();
        }

        public override void Add(T t)
        {
            base.Add(t);
            _typeSet.Add(t.GetType());
        }

        public override bool Remove(T t)
        {
            _typeSet.Remove(t.GetType());
            return base.Remove(t);
        }

        public override void Clear()
        {
            base.Clear();
            _typeSet.Clear();
        }

        #region Search
        public bool Contains<U>()
            where U : ISearcheable
        {
            return Contains(typeof(U));
        }

        public bool Contains(Type type)
        {
            return _typeSet.Contains(type);
        }
        #endregion
    }
}
