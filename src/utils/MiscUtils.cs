namespace SCE
{
    public static class MiscUtils
    {
        public static bool QueueSet<T>(ref T set, T value)
        {
            var last = set;
            set = value;
            return !EqualityComparer<T>.Default.Equals(last, set);
        }

        public static bool QueueSet<T>(ref T set, T value, ref bool updateFlag)
        {
            var res = QueueSet(ref set, value);
            if (res)
            {
                updateFlag = true;
            }
            return res;
        }
    }
}
