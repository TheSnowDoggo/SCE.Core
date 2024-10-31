namespace SCECore.Types
{
    /// <summary>
    /// Represents errors thrown when an attempt is made to use an invalid <see cref="Vector2"/>.
    /// </summary>
    internal class InvalidPositionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPositionException"/> class.
        /// </summary>
        public InvalidPositionException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPositionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidPositionException(string? message)
            : base(message)
        {
        }
    }
}
