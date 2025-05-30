using CSUtils;
namespace SCE
{
    public class ConsoleRenderer : UIBaseExt
    {
        public const int DEFAULT_BUFFERHEIGHT = 9001;

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
            bufferWidth = width;
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

        public Pixel[] this[int top]
        {
            get
            {
                if (top < 0 || top >= BufferHeight)
                {
                    throw new IndexOutOfRangeException("Top exceeds buffer height.");
                }
                var arr = _ls[top] ??= new Pixel[BufferWidth];
                if (arr.Length != BufferWidth)
                {
                    Array.Resize(ref arr, BufferWidth);
                }
                return arr;
            }
            set
            {
                if (top < 0 || top >= BufferHeight)
                {
                    throw new ArgumentException("Top exceeds buffer height.");
                }
                _ls[top] = value;
            }
        }

        public Pixel this[int top, int left]
        {
            get
            {
                if (left < 0 || left >= BufferWidth)
                {
                    throw new IndexOutOfRangeException("Left exceeds buffer width.");
                }
                return this[top][left];
            }
            set
            {
                if (left < 0 || left >= BufferWidth)
                {
                    throw new ArgumentException("Left exceeds buffer width.");
                }
                this[top][left] = value;
            }
        }

        public Pixel this[Vector2Int pos]
        {
            get => this[pos.Y, pos.X];
            set => this[pos.Y, pos.X] = value;
        }

        #region Write

        public Vector2Int Translate(int index)
        {
            if (index < 0 || index >= BufferWidth * BufferHeight)
            {
                throw new IndexOutOfRangeException("Index out of bounds.");
            }
            return new(index % BufferWidth, index / BufferWidth);
        }

        public int Translate(Vector2Int pos)
        {
            return pos.Y * BufferWidth + pos.X;
        }

        public void ScrollCursor(int scroll, out int overflow)
        {
            overflow = 0;

            var nextX = cursorPos.X + scroll;

            var lines = (scroll > 0 ? nextX : BufferWidth - nextX - 1) / BufferWidth;

            cursorPos.X = Utils.Mod(nextX, BufferWidth);

            if (scroll > 0)
            {
                int nextY = cursorPos.Y + lines;
                if (nextY >= BufferHeight)
                {
                    cursorPos = new Vector2Int(BufferWidth, BufferHeight) - 1;
                    overflow = 1 + BufferHeight - nextY;
                }
                else
                {
                    cursorPos.Y += lines;
                }
            }
            else
            {
                int nextY = cursorPos.Y - lines;
                if (nextY < 0)
                {
                    cursorPos = Vector2Int.Zero;
                    overflow = nextY;
                }
                else
                {
                    cursorPos.Y -= lines;
                }
            }
        }

        public void ScrollCursor(int scroll)
        {
            ScrollCursor(scroll, out _);
        }

        private void ShiftCursor(int move, bool carriageReturn = true)
        {
            ScrollCursor(move, out int overflow);
            for (int i = 0; i < overflow; ++i)
            {
                _ls.Increase();
            }
            if (carriageReturn && overflow > 0)
            {
                cursorPos.X = 0;
            }
        }

        public bool Delete()
        {
            ScrollCursor(-1, out int overflow);
            if (overflow != 0)
            {
                return false;
            }

            this[CursorPos.Y, cursorPos.X] = Pixel.Empty;
            renderQueued = true;

            return false;
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

        public void Write(char c)
        {
            switch (c)
            {
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
                    Newline();
                    break;
                case '\a':
                    break;
                default:
                    this[CursorPos.Y, CursorPos.X] = new Pixel(c, FgColor, BgColor);
                    ShiftCursor(1);
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

        public void WriteLine()
        {
            Newline();
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

        #endregion

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

        public void Clear()
        {
            _ls.CleanResize(BufferHeight);
            Scroll = 0;
            renderQueued = true;
        }

        public override DisplayMapView GetMapView()
        {
            if (renderQueued)
            {
                for (int y = 0; y < Height; ++y)
                {
                    var area = Rect2DInt.Horizontal(y, _dpMap.Width);

                    var index = y + Scroll;
                    var arr = this[index];
                    if (index < BufferHeight && arr != null)
                    {
                        _dpMap.Fill(pos => arr[pos.X], area);
                    }
                }

                renderQueued = false;
            }
            return (DisplayMapView)_dpMap;
        }
    }
}
