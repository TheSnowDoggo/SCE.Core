using CSUtils;
using SCE.UIS;
using System.Diagnostics;
namespace SCE
{
    internal class Demo
    {
        private static void Main()
        {
            Test2();
        }

        private static void Test2()
        {
            Console.WriteLine("Installing package...");

            Display dp = new();

            dp.SetupHere(Console.WindowWidth, 1);

            int end = 50000000;

            ProgressBar pb = new(50, 1)
            {
                Max = end,
                BackFill = new(SCEColor.DarkGray),
            };

            TextBoxUI tb = new(4, 1)
            {
                TextFgColor = SCEColor.Green,
                Offset = new(pb.Width, 0),
            };

            dp.Renderables.AddEvery(pb, tb);

            dp.Update();

            ++Console.CursorTop;

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < end; ++i)
            {
                Utils.IsPrime(i);

                if (sw.Elapsed.TotalSeconds > 0.1 || i == end - 1)
                {
                    var prog = (int)Math.Round(i / (float)end * 100.0f);

                    pb.Value = i;
                    tb.Text = $"{prog:00}%";

                    dp.Update();

                    sw.Restart();
                }
            }

            dp.Renderables.Clear();

            Console.WriteLine("Finished!");

            Console.ReadLine();
        }

        private static void Test1()
        {
            InputHandler ih = new();

            // The input controller only runs the Input thread so you use an InputHandler to receive input events.
            // Each input is queued up into the InputHandler and the queue is then cleared every frame.
            InputController.Link(ih);

            Mane mane = new();

            // You need to add the Display instance to the scenes for rendering.
            // Scenes are run in the order they are added, so it's recommended to put display last.
            GameHandler.Scenes.AddEvery((ih, "inputhandler"), (mane, "mane"), (Display.Instance, "display"));

            // Sets the framerate cap in FPS. -1 represents uncapped framerate.
            GameHandler.FrameCap = -1;

            // Starts GameHandler thread (for scenes).
            GameHandler.Start();

            // Start Input thread
            InputController.Start();
        }
    }

    internal class Mane : SceneBase
    {
        private readonly TextBoxUI _fps;

        private readonly InputEntryV2 _ie;

        private readonly FlowTable _fl;

        private readonly LineRenderer _lr;

        private readonly Logger _logger;

        private readonly FlowTable _loggerFl;

        private readonly Overlay<TextBoxUI>[] _tbArr = new Overlay<TextBoxUI>[6];

        private readonly Vector2Int _tbDimensions = new(50, 2);

        private readonly Image _cur;

        private readonly Scaler<Image> _scl;

        private readonly ConsoleRenderer _cr;

        private int selectIndex;

        private double timer;

        private readonly ProgressBar _pb;

        private readonly VerticalSelector _vs;

        public Mane()
        {
            _pb = new(100, 1)
            {
                Max = 200,
                Anchor = Anchor.Middle | Anchor.Center,
                FlowMode = FlowType.LeftRight,
            };

            _cr = new(30, 15)
            {
                Anchor = Anchor.Bottom,
            };

            _vs = new(20, 10)
            {
                Anchor = Anchor.Right,
                Offset = new(0, 1),
            };

            for (int i = 0; i < _vs.Height; ++i)
            {
                int num = i + 1;
                _vs[i] = new()
                {
                    Text = $"Selection {num}",
                    OnSelect = () => _cr.WriteLine($"You selected {num}"),
                };
            }

            Image img = new(5, 5, SCEColor.DarkBlue)
            {
                Anchor = Anchor.Center | Anchor.Middle,
            };

            img.Fill(new Pixel(SCEColor.Magenta), Rect2DInt.Vertical(2, img.Height));

            img.Fill(new Pixel(SCEColor.Cyan), Rect2DInt.Horizontal(1, img.Width));

            _scl = new(img);

            // One limitation of the logger is each log can only occupy one line
            // This may be updated in the future.
            _logger = new(60, 10, SCEColor.DarkGray)
            {
                Anchor = Anchor.Right,
                // Determines the direction logs are shown in
                StackMode =  StackType.TopDown,
            };

            // The height determines how many lines can be rendered.
            _lr = new(_logger.Width, 8)
            {
                Anchor = Anchor.Right,
                // Combine Center anchors with Right or Left anchors to determine the center bias.
                // Left bias (default) with round down if the text cannot be fully centered, Right bias rounds up              
                StackMode = StackType.BottomUp,
                BasePixel = new(SCEColor.Transparent),
            };

            _lr.Text[0] = "- View Logs -";
            _lr.Attributes[0] = new()
            {
                Anchor = Anchor.Center,
            };

            for (int i = 1; i < _lr.Height; ++i)
            {
                _lr.Text[i] = $"testing chicken {i}";
                _lr.Attributes[i] = new()
                {
                    Anchor = Anchor.Center,
                    FgColor = SCEColor.Red,
                    BgColor = SCEColor.DarkMagenta,
                };
            }

            // Flow tables are used to stack multiple UI elements in a certain direction
            _loggerFl = new(_logger.Width, _logger.Height + _lr.Height)
            {
                // Use bitwise OR to combine anchors
                Anchor = Anchor.Bottom | Anchor.Right,
                // Top to bottom stacking
                FlowMode = FlowType.TopDown,
            };

            _loggerFl.Renderables.AddRange(new IRenderable[] { _lr, _logger });

            _cur = new(new DisplayMap(1, 1)
            {
                [0, 0] = new('\0', SCEColor.DarkGray, SCEColor.White)
            })
            {
                IsActive = false,
            };

            _fps = new(20, 1)
            {
                TextFgColor = SCEColor.Gray,
                Anchor = Anchor.Right,
            };

            _fl = new(_tbDimensions.X, _tbDimensions.Y * _tbArr.Length)
            {
                FlowMode = FlowType.TopDown,
            };

            for (int i = 0; i < _tbArr.Length; ++i)
            {
                TextBoxUI tb = new(_tbDimensions)
                {
                    NewlineOverflow = true,
                };
                _tbArr[i] = new(tb);
                _fl.Renderables.Add(_tbArr[i]);
            }

            _ie = new()
            {
                // Intercepters intercept key inputs and are useful for advanced functionality
                Intercepters = new()
                {
                    InputEntryV2.Backspace(),
                    InputEntryV2.Exit(EntryExit),
                    InputEntryV2.CharacterLimiter(_tbDimensions.ScalarProduct()),
                    InputEntryV2.Scroll(),
                },
            };

            _ie.OnReceive += OnReceive;
        }

        private static InputHandler InputHandler { get => GameHandler.Scenes.Get<InputHandler>("inputhandler"); }

        private TextBoxUI Selected { get => _tbArr[selectIndex].Renderable; }

        public override void Start()
        {
            SetupDisplay();
            SetupInput();

            Highlight(SCEColor.Black, SCEColor.White);

            _logger.Log("Welcome to the SCE Demo!");
            _logger.Log("Have a peek around at some of the many features.");
            
            _cr.WriteLine("this was a console emulator");
        }

        public override void Update()
        {
            // You can also use GameHandler.DeltaTime to get the time elapsed between each frame.
            _fps.Text = $"FPS: {GameHandler.FPS}";

            if (timer > 0)
            {
                timer -= GameHandler.DeltaTime;
            }
            else
            {
                timer = 0.1;

                _scl.Renderable.Rotate90(true);   
            }

            _pb.Value += (float)(GameHandler.DeltaTime * 10.0);
        }

        private void SetupDisplay()
        {
            DebugEngine.Instance.ShowSIFCodes = true;

            // Damn it is fast.
            Display.Instance.RenderEngine = BMEngine.Instance;

            Display.Instance.ResizeMode = Display.ResizeType.Auto;

            // Adds the IRenderables to the display to render.
            Display.Instance.Renderables.AddRange(new IRenderable[] { _fl, _loggerFl, _cr, _fps, _scl, _pb, _vs });

            Display.Instance.BasePixel = new(SCEColor.DarkCyan);
        }

        private void SetupInput()
        {
            InputAction enter = new(ConsoleKey.Enter, () => _vs.RunSelected());

            InputMap<int> slider = new(_vs.MoveSelected)
            {
                { ConsoleKey.UpArrow,   -1 },
                { ConsoleKey.DownArrow, +1 },
            };

            InputLayer mane = new(0) { enter, slider };

            InputLayer entry = new(-1) { _ie };
            entry.IsActive = false;

            InputHandler.AddEvery((mane, "mane"), (entry, "entry"));
        }

        private void OnReceive()
        {
            Selected.Text = _ie.Input;
            _cur.Offset = new(_ie.CharacterIndex % Selected.Width, _ie.CharacterIndex / Selected.Width);
        }

        private void Highlight(SCEColor fg, SCEColor bg)
        {
            Selected.TextFgColor = fg;
            Selected.BasePixel = new(bg);
        }

        private void EntryEnter()
        {
            if (!_ie.IsReceiving)
            {
                _ie.IsReceiving = true;
                _ie.Input = Selected.Text;
                InputHandler.Get("mane").IsActive = false;
                InputHandler.Get("entry").IsActive = true;
                Highlight(SCEColor.Black, SCEColor.Gray);
                _cur.IsActive = true;
            }
        }

        private void EntryExit()
        {
            _ie.Clear();
            InputHandler.Get("mane").IsActive = true;
            InputHandler.Get("entry").IsActive = false;
            Highlight(SCEColor.Black, SCEColor.White);
            _cur.IsActive = false;
        }

        private void Slide(int move)
        {
            Highlight(SCEColor.White, SCEColor.Black);
            _tbArr[selectIndex].Map = null;
            selectIndex = Utils.Mod(selectIndex + move, _tbArr.Length);
            Highlight(SCEColor.Black, SCEColor.White);
            _tbArr[selectIndex].Map = _cur;
        }
    }
}