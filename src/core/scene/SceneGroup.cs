namespace SCE
{
    public class SceneGroup : SearchHashTypeExt<IScene>, IScene
    {
        private const string DEFAULT_NAME = "scene_group";

        public SceneGroup(string name, IEnumerable<SceneBase> collection)
            : base(collection)
        {
            Name = name;
        }

        public SceneGroup(string name)
            : base()
        {
            Name = name;
        }

        public SceneGroup(IEnumerable<SceneBase> collection)
            : this(DEFAULT_NAME, collection)
        {
        }

        public SceneGroup()
            : this(DEFAULT_NAME)
        {
        }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public bool IsRunning { get; set; } = true;

        public bool IsStarting { get; set; } = true;

        public void Start()
        {
            foreach (var scene in this)
            {
                if (scene.IsActive)
                    scene.Start();
            }
        }

        public void Update()
        {
            foreach (var scene in this)
            {
                if (scene.IsActive)
                    scene.Update();
            }
        }
    }
}
