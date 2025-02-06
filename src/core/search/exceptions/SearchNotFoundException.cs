namespace SCE
{
    public class SearchNotFoundException : Exception
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
