namespace SCE
{
    public abstract class SceneBase : IScene
    {
        public SceneBase(string name)
        {
            Name = name;
        }

        public SceneBase()
            : this(string.Empty)
        {
        }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsRunning { get; set; } = true;

        public bool IsStarting { get; set; } = true;

        public abstract void Start();

        public abstract void Update();
    }
}
