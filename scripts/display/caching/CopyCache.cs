namespace SCE
{
    internal class CopyCache : Memoizer<int, string>
    {
        public CopyCache(char chr)
            : base((n) => StringUtils.Copy(chr, n))
        {
            Character = chr;
        }

        public char Character { get; }
    }
}
