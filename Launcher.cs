using CSUtils;

namespace SCE
{
    internal static class Launcher
    {
        internal static void Main()
        {
            Mane mane = new();

            Display.Instance.RenderMode = Display.RenderType.CCS;

            GameHandler.Scenes.AddEvery(mane, Display.Instance);

            GameHandler.Start();
        }
    }

    internal class Mane : SceneBase
    {
        private static readonly Random _rand = new();

        private static readonly SCEColor[] _heatColors =
        {
            SCEColor.Black,
            SCEColor.DarkGray,
            SCEColor.Gray,
            SCEColor.DarkBlue,
            SCEColor.Blue,
            SCEColor.DarkCyan,
            SCEColor.Cyan,
            SCEColor.Yellow,
            SCEColor.DarkYellow,
            SCEColor.Red,
            SCEColor.DarkRed,
        };

        private const double HEAT_MULTIPLIER = 10.0;

        private readonly FlowTable _fl;

        private readonly Logger _log;

        private readonly Grid2D<int> _heatMap;

        private readonly TextBoxUI _fps;

        private double timer;

        public Mane()
        {
            _heatMap = new(15, 15);

            TextBoxUI tb1 = new(15, 3)
            {
                Text = "hello\nWhat the heck\nis this",
                Anchor = Anchor.Right,
                TextAnchor = Anchor.Right,
            };

            TextBoxUI tb2 = new(5, 1)
            {
                Text = "hello",
                Anchor = Anchor.Center | Anchor.Right,
            };

            TextBoxUI tb3 = new(30, 1)
            {
                Text = "testing chicken",
            };

            _log = new(60, 10);

            int tlrWidth = 2;
            TranslateLineRenderer tlr = new(_heatMap.Dimensions * new Vector2Int(tlrWidth, 1), tlrWidth, pos =>
            {
                var c = SCEColor.Black;
                for (int i = 0; i < _heatColors.Length; ++i)
                {
                    if (_heatMap[pos] <= i * HEAT_MULTIPLIER)
                    {
                        c = _heatColors[i];
                        break;
                    }
                }
                return Utils.CopyArr(new Pixel(c), tlrWidth);
            });

            _fl = new(Display.Instance.Dimensions)
            {
                FlowMode = FlowType.TopDown
            };

            _fl.Renderables.AddRange(new IRenderable[] { tb1, tb2, tb3, _log, tlr });

            _fps = new(20, 1, SCEColor.White)
            {
                TextFgColor = SCEColor.Black,
                TextBgColor = SCEColor.Transparent,
                TextAnchor = Anchor.Right,
                Anchor = Anchor.Bottom | Anchor.Right,
            };
        }

        private void OnDisplayResize()
        {
            _fl.CleanResize(Display.Instance.Dimensions);
        }

        public override void Start()
        {
            Display.Instance.Renderables.AddRange(new IRenderable[] { _fl, _fps });
            Display.Instance.OnDisplayResize += OnDisplayResize;
        }

        public override void Update()
        {
            if (timer > 0)
                timer -= GameHandler.DeltaTime;
            else
            {
                timer = 0.01;
                var pos = Vector2Int.Rand(_heatMap.Dimensions);
                var inc = _rand.Next(10);
                int next = _heatMap[pos] + 1 + inc;
                _log.Log($"[{pos}] {_heatMap[pos]} to {next}");
                _heatMap[pos] = next;
            }
            _fps.Text = $"FPS: {GameHandler.FPS}";
        }
    }
}
