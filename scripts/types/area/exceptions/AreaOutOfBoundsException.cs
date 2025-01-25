namespace SCE
{
    /// <summary>
    /// Represents errors thrown when an attempt is made to access an <see cref="Area2D"/> on a <see cref="Grid2D{T}"/> that is outside its bounds.
    /// </summary>
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

