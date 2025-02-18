namespace SCE
{
    public static class ArrayUtils
    {
        public static T[] Copy<T>(T val, int copies)
        {
            if (copies <= 0)
                return Array.Empty<T>();
            var arr = new T[copies];
            for (int i = 0; i < copies; ++i)
                arr[i] = val;
            return arr;
        }
    }
}
