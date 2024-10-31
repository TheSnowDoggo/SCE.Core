namespace SCECore.Types
{
    /// <summary>
    /// Represents errors thrown when invalid dimensions are used to initialize a grid.
    /// </summary>
    internal class InvalidDimensionsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDimensionsException"/> class.
        /// </summary>
        public InvalidDimensionsException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDimensionsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidDimensionsException(string? message)
            : base(message)
        {
        }
    }
}
