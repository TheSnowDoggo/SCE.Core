namespace SCECore
{
    /// <summary>
    /// The exception that is thrown when an attempt is made to access an element of a grid with a position that is outside its bounds.
    /// </summary>
    internal class PositionOutOfBoundsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionOutOfBoundsException"/> class.
        /// </summary>
        public PositionOutOfBoundsException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionOutOfBoundsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PositionOutOfBoundsException(string? message)
            : base(message)
        {
        }
    }
}
