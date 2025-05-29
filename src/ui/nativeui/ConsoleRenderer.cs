using CSUtils;
namespace SCE
{
    public class ConsoleRenderer : UIBaseExt
    {
        public const int DEFAULT_BUFFERHEIGHT = 9001;
        public const int DEFAULT_BUFFERWIDTH = 200;

        private readonly LoopStack<Pixel[]> _ls;

        private Vector2Int cursorPos;

        private int bufferWidth;

        private int scroll;

        private Pixel basePixel = new(SCEColor.Black);

        private bool renderQueued = true;

        public ConsoleRenderer(int width, int height)
            : base(width, height)
        {
            _ls = new(Math.Max(height, DEFAULT_BUFFERHEIGHT));
            bufferWidth = Math.Max(width, DEFAULT_BUFFERWIDTH);
        }

        public ConsoleRenderer(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        public Vector2Int CursorPos
        {
            get => cursorPos;
            set
            {
                if (value < Vector2Int.Zero || value.X >= BufferWidth || value.Y >= BufferHeight)
                {
                    throw new ArgumentException("Cursor position invalid.");
                }
                cursorPos = value;            }
        }

        public int BufferHeight
        {
            get => _ls.Length;
            set
            {
                if (value < Height)
                {
                    throw new ArgumentException("Buffer height cannot be less than window width.");
                }
                _ls.Resize(value);
            }
        }

        public int BufferWidth
        {
            get => bufferWidth;
            set
            {
                if (value < Width)
                {
                    throw new ArgumentException("Buffer width cannot be less than window width.");
                }
                bufferWidth = value;
            }
        }

        public int Scroll
        {
            get => scroll;
            set => MiscUtils.QueueSet(ref scroll, value, ref renderQueued);
        }

        public bool AutoScroll { get; set; } = true;

        public int AutoScrollOffset { get; set; } = 0;

        public SCEColor FgColor { get; set; } = SCEColor.Gray;

        public SCEColor BgColor { get; set; } = SCEColor.Black;

        public Pixel BasePixel
        {
            get => basePixel;
            set => MiscUtils.QueueSet(ref basePixel, value, ref renderQueued);
        }

        public int TabSize { get; set; } = 8;

        public Pixel[] this[int y]
        {
            get => _ls[y];
            set
            {
                _ls[y] = value;
                renderQueued = true;
            }
        }

        private void ShiftCursor(int move)
        {
            var next = cursorPos.X + move;
            cursorPos.X = Utils.Mod(next, BufferWidth);
            var lines = next / BufferWidth;
            for (int i = 0; i < lines; ++i)
            {
                Newline();
            }
        }

        private void Newline(bool carriageReturn = true)
        {
            if (++cursorPos.Y >= BufferHeight)
            {
                cursorPos.Y = BufferHeight - 1;
                _ls.Increase();
            }
            if (carriageReturn)
            {
                cursorPos.X = 0;
            }
        }

        private Pixel[] CurrentLine()
        {
            var arr = _ls[CursorPos.Y] ??= new Pixel[BufferWidth];
            if (arr.Length != BufferWidth)
            {
                Array.Resize(ref arr, BufferWidth);
            }
            return arr;
        }

        public void Write(char c)
        {
            switch (c)
            {
                default:
                    CurrentLine()[CursorPos.X] = new Pixel(c, FgColor, BgColor);
                    ShiftCursor(1);
                    break;
                case '\b':
                    if (cursorPos.X > 0)
                    {
                        --cursorPos.X;
                    }
                    break;
                case '\t':
                    ShiftCursor(TabSize);
                    break;
                case '\r':
                    cursorPos.X = 0;
                    break;
                case '\v':
                case '\f':
                case '\n':
                    Newline(true);
                    break;
                case '\a':
                    break;
            }

            renderQueued = true;

            if (AutoScroll && CursorPos.Y - Scroll == Height - AutoScrollOffset)
            {
                ++Scroll;
            }
        }

        public void Write(string str)
        {
            foreach (var c in str)
            {
                Write(c);
            }
        }

        public void Write(object obj)
        {
            if (obj?.ToString() is string str)
            {
                Write(str);
            }
        }

        public void WriteLine(string str)
        {
            Write(str);
            Newline();
        }

        public void WriteLine(object obj)
        {
            Write(obj);
            Newline();
        }

        public void Clear()
        {
            _ls.CleanResize(BufferHeight);
            Scroll = 0;
            renderQueued = true;
        }

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            if (BufferWidth < width)
            {
                BufferWidth = width;
            }
            renderQueued = true;
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        public override DisplayMapView GetMapView()
        {
            if (renderQueued)
            {
                for (int y = 0; y < Height; ++y)
                {
                    var area = Rect2DInt.Horizontal(y, _dpMap.Width);

                    var index = y + Scroll;
                    if (index < BufferHeight && _ls[index] != null)
                    {
                        var arr = _ls[index];
                        _dpMap.Fill(pos => arr[pos.X], area);
                    }
                    else
                    {
                        _dpMap.Fill(BasePixel, area);
                    }
                }

                renderQueued = false;
            }
            return (DisplayMapView)_dpMap;
        }
    }
}
