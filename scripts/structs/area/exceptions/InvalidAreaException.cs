namespace SCE
{
    /// <summary>
    /// The exception that is thrown when an attempt is made to use an invalid <see cref="Area2D"/>.
    /// </summary>
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
