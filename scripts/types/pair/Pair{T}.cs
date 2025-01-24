namespace SCE
{
    public struct Pair<T> : IEquatable<Pair<T>>
    {
        public T First;

        public T Second;

        public Pair(T first, T second)
        {
            First = first;
            Second = second;
        }

        #region Equality
        public static bool operator ==(Pair<T> left, Pair<T> right) => left.Equals(right);

        public static bool operator !=(Pair<T> left, Pair<T> right) => !(left == right);
        #endregion

        public bool Equals(Pair<T> other)
        {
            return First is not null && Second is not null && First.Equals(other.First) && Second.Equals(other.Second);
        }

        public override bool Equals(object? obj)
        {
            return obj is Pair<T> pair && Equals(pair);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(First, Second);
        }

        public override string ToString()
        {
            return $"Pair<{typeof(T).Name}>({First},{Second})";
        }

        public void Expose(out T first, out T second)
        {
            first = First;
            second = Second;
        }
    }
}
