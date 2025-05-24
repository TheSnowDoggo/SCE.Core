namespace SCE
{
    public class SceneGroup : AliasHashTExt<IScene>, IScene
    {
        public SceneGroup()
            : base()
        {
        }

        public SceneGroup(IEnumerable<SceneBase> collection)
           : base(collection)
        {
        }

        public bool IsActive { get; set; } = true;

        public bool IsRunning { get; set; } = true;

        public bool IsStarting { get; set; } = true;

        public void Start()
        {
            foreach (var scene in this)
                if (scene.IsActive)
                    scene.Start();
        }

        public void Update()
        {
            foreach (var scene in this)
                if (scene.IsActive)
                    scene.Update();
        }
    }
}
