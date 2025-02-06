namespace SCE
{
    public class LineOverflowException : Exception
    {
        public LineOverflowException()
            : base()
        {
        }
        public LineOverflowException(string? message)
            : base(message)
        {
        }
    }
}
