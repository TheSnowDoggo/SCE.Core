namespace SCE
{
    internal static class Launcher
    {
        internal static void Main()
        {
            Mane mane = new();

            Display.Instance.RenderMode = Display.RenderType.Debug;

            GameHandler.Scenes.AddEvery(mane, Display.Instance);

            GameHandler.Start();
        }
    }

    internal class Mane : SceneBase
    {
        private readonly FlowTable _fl;

        public Mane()
        {
            TextBoxUI tb1 = new(15, 3)
            {
                Text = "hello\nWhat the heck\nis this",
                Anchor = Anchor.Right,
            };

            TextBoxUI tb2 = new(5, 1)
            {
                Text = "hello",
                Anchor = Anchor.Center | Anchor.Right,
            };

            TextBoxUI tb3 = new(5, 1)
            {
                Text = "hello",
                Anchor = Anchor.Center,
            };

            _fl = new(Display.Instance.Dimensions)
            {
                FlowMode = FlowType.TopDown
            };

            _fl.Renderables.AddRange(new IRenderable[] { tb1, tb2, tb3 });
        }

        public override void Start()
        {
            Display.Instance.Renderables.AddRange(new IRenderable[] { _fl });
        }

        public override void Update()
        {
        }
    }
}
