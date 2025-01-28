namespace SCE
{
    /// <summary>
    /// The exception that is thrown when an attempt is made to access an element of a grid with a position that is outside its bounds.
    /// </summary>
    public class PositionOutOfBoundsException : Exception
    {
        public PositionOutOfBoundsException()
            : base()
        {
        }
        public PositionOutOfBoundsException(string? message)
            : base(message)
        {
        }
    }
}
