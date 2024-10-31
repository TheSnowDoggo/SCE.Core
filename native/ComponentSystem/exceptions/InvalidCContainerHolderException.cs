namespace SCECore.ComponentSystem
{
    /// <summary>
    /// Represents errors thrown when an incompatible <see cref="IComponent"/> is added to a component holder.
    /// </summary>
    public class InvalidCContainerHolderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCContainerHolderException"/> class.
        /// </summary>
        public InvalidCContainerHolderException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCContainerHolderException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidCContainerHolderException(string? message)
            : base(message)
        {
        }
    }
}
