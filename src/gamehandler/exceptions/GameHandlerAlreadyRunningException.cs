namespace SCE
{
    internal class GameHandlerAlreadyRunningException : Exception
    {
        public GameHandlerAlreadyRunningException()
            : base()
        {
        }
        public GameHandlerAlreadyRunningException(string? message)
            : base(message)
        {
        }
    }
}
