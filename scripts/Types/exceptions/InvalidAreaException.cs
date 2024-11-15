namespace SCECore.Types
{
    /// <summary>
    /// The exception that is thrown when an attempt is made to use an invalid <see cref="Area2D"/>.
    /// </summary>
    internal class InvalidAreaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAreaException"/> class.
        /// </summary>
        public InvalidAreaException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAreaException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidAreaException(string? message)
            : base(message)
        {
        }
    }
}
