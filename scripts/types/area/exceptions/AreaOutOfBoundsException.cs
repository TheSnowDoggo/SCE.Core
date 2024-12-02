namespace SCE
{
    /// <summary>
    /// Represents errors thrown when an attempt is made to access an <see cref="Area2D"/> on a <see cref="Grid2D{T}"/> that is outside its bounds.
    /// </summary>
    internal class AreaOutOfBoundsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOutOfBoundsException"/> class.
        /// </summary>
        public AreaOutOfBoundsException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOutOfBoundsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AreaOutOfBoundsException(string? message)
            : base(message)
        {
        }
    }
}

