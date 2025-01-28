namespace SCE
{
    /// <summary>
    /// Represents errors thrown when invalid dimensions are used to initialize a grid.
    /// </summary>
    public class InvalidDimensionsException : Exception
    {
        public InvalidDimensionsException()
            : base()
        {
        }
        public InvalidDimensionsException(string? message)
            : base(message)
        {
        }
    }
}
