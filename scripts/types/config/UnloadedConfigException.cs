namespace SCE
{
    internal class UnloadedConfigException : Exception
    {
        public UnloadedConfigException(string message)
            : base(message)
        {}
        public UnloadedConfigException()
            : base()
        {}
    }
}
