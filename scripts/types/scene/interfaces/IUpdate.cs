namespace SCE
{
    public interface IUpdate : ISearcheable
    {
        bool IsActive { get; set; }
        void Update();
    }
}
