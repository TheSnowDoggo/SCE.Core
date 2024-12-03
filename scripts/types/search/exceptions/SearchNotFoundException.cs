namespace SCE
{
    internal class SearchNotFoundException : Exception
    {
        public SearchNotFoundException()
            : base()
        {
        }
        public SearchNotFoundException(string? message)
            : base(message)
        {
        }
    }
}
