namespace SCECore.Types
{
    using System.Collections;

    public class ValueMap<T1, T2> : IEnumerable<(T1, T2)>
        where T1 : IEquatable<T1>
        where T2 : IEquatable<T2>
    {
        private readonly List<(T1, T2)> list;

        public ValueMap(List<(T1, T2)> list)
        {
            this.list = list;
        }

        public ValueMap()
            : this(new List<(T1, T2)>())
        {
        }

        public T1 this[T2 t2] { get => FindFirst(t2); }

        public T2 this[T1 t1] { get => FindFirst(t1); }

        /// <inheritdoc/>
        public IEnumerator<(T1, T2)> GetEnumerator() => list.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add((T1, T2) record) => list.Add(record);

        public void Add(T1 t1, T2 t2) => list.Add((t1, t2));

        public bool Contains(T1 key)
        {
            try
            {
                FindFirst(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Contains(T2 key)
        {
            try
            {
                FindFirst(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private T2 FindFirst(T1 key)
        {
            foreach ((T1, T2) record in list)
            {
                (T1 t1, T2 t2) = record;

                if (t1.Equals(key))
                {
                    return t2;
                }
            }

            throw new ArgumentException($"No set with key {key} found");
        }

        private T1 FindFirst(T2 key)
        {
            foreach ((T1, T2) record in list)
            {
                (T1 t1, T2 t2) = record;

                if (t2.Equals(key))
                {
                    return t1;
                }
            }

            throw new ArgumentException($"No set with key {key} found");
        }
    }
}
