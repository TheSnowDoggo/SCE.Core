namespace SCE
{
    public interface IScene : ISearcheable
    {
        bool IsActive { get; set; }

        void Start();
        void Update();
    }
}
