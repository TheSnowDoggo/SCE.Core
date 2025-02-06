namespace SCE
{
    public class InvalidAreaException : Exception
    {
        public InvalidAreaException()
            : base()
        {
        }
        public InvalidAreaException(string? message)
            : base(message)
        {
        }
    }
}
