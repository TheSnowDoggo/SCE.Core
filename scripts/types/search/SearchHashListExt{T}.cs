namespace SCE
{
    public class SearchHashListExt<T> : SearchHash<T>
        where T : ISearcheable
    {
        protected readonly List<T> _list;

        public SearchHashListExt(IEnumerable<T> collection)
            : base()
        {
            _list = new();
            AddRange(collection);
        }

        public SearchHashListExt(int capacity)
            : base(capacity)
        {
            _list = new(capacity);
        }

        public SearchHashListExt()
            : base()
        {
            _list = new();
        }

        public override void Add(T t)
        {
            _list.Add(t);
            base.Add(t);
        }

        public override bool Remove(T t)
        {
            _list.Remove(t);
            return base.Remove(t);
        }

        public override void Clear()
        {
            base.Clear();
            _list.Clear();
        }
    }
}
