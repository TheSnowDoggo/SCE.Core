namespace SCE
{
    public class DuplicateNameException : Exception
    {
        public DuplicateNameException()
            : base()
        {
        }
        public DuplicateNameException(string? message)
            : base(message)
        {
        }
    }
}
