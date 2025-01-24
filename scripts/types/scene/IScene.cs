namespace SCE
{
    public interface IScene : ISearcheable
    {
        bool IsActive { get; set; }
        bool IsRunning { get; set; }
        bool IsStarting { get; set; }

        void Start();
        void Update();
    }
}
