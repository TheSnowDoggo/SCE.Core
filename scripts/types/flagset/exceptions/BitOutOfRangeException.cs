namespace SCE
{
    public class BitOutOfRangeException : Exception
    {
        public BitOutOfRangeException()
            : base()
        {
        }
        public BitOutOfRangeException(string? message)
            : base(message)
        {
        }
    }
}
