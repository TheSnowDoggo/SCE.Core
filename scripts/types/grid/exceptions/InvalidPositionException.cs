namespace SCE
{
    /// <summary>
    /// Represents errors thrown when an attempt is made to use an invalid <see cref="Vector2"/>.
    /// </summary>
    public class InvalidPositionException : Exception
    {
        public InvalidPositionException()
            : base()
        {
        }
        public InvalidPositionException(string? message)
            : base(message)
        {
        }
    }
}
