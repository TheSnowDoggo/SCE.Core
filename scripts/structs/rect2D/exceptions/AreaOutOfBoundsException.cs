namespace SCE
{
    public class AreaOutOfBoundsException : Exception
    {
        public AreaOutOfBoundsException()
            : base()
        {
        }
        public AreaOutOfBoundsException(string? message)
            : base(message)
        {
        }
    }
}

