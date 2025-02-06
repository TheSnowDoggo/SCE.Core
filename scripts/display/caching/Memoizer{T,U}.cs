namespace SCE
{
    internal class Memoizer<I,O>
        where I : notnull
    {
        private readonly Dictionary<I, O> _cacheDict = new();

        public Memoizer(Func<I, O> func)
        {
            Func = func;
        }

        public Func<I, O> Func { get; }

        public O CachedCall(I i)
        {
            if (!_cacheDict.TryGetValue(i, out O? value))
            {
                value = Func.Invoke(i);
                _cacheDict.Add(i, value);
            }
            return value;
        }
    }
}
