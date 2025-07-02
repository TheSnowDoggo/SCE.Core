using CSUtils;

namespace SCE
{
    public class ConsoleRenderer : UIBaseExt
    {
        private const int DEF_BUFFER_HEIGHT = 9001;

        private CycleBuffer<Pixel> buffer;

        private bool renderQueued = true;

        public ConsoleRenderer(int width, int height)
            : base(width, height)
        {
            bufferDimensions = new(width, Math.Max(DEF_BUFFER_HEIGHT, height));

            buffer = new(bufferDimensions.ScalarProduct());
        }

        public ConsoleRenderer(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        public Pixel this[int index]
        {
            get => buffer[index];
            set => buffer[index] = value;
        }

        public Pixel this[int top, int left]
        {
            get => this[Translate(top, left)];
            set => this[Translate(top, left)] = value;
        }

        public Pixel this[Vector2Int pos]
        {
            get => this[pos.Y, pos.X];
            set => this[pos.Y, pos.X] = value;
        }

        public int Translate(int top, int left)
        {
            return top * BufferWidth + left;
        }

        public int Translate(Vector2Int pos)
        {
            return Translate(pos.Y, pos.X);
        }

        public Vector2Int Translate(int index)
        {
            return new(index % BufferWidth, index / BufferWidth);
        }

        private Vector2Int bufferDimensions;

        public Vector2Int BufferDimensions
        {
            get => bufferDimensions;
            set
            {
                MapResizeBuffer(value);
                renderQueued = true;
            }
        }

        public int BufferWidth
        {
            get => BufferDimensions.X;
            set => BufferDimensions = new(value, BufferDimensions.Y);
        }

        public int BufferHeight
        {
            get => BufferDimensions.Y;
            set => BufferDimensions = new(BufferDimensions.X, value);
        }

        public int BufferSize { get => buffer.Length; }

        public Vector2Int CursorPos
        {
            get => new(CursorLeft, CursorTop);
            set
            {
                CursorLeft = value.X;
                CursorTop = value.Y;
            }
        }

        public int CursorIndex
        {
            get => Translate(CursorTop, CursorLeft);
            set => CursorPos = Translate(value);
        }

        private int cursorLeft;

        public int CursorLeft
        {
            get => cursorLeft;
            set
            {
                if (value < 0 || value >= BufferWidth)
                {
                    throw new ArgumentException("Cursor left is outside buffer width.");
                }
                cursorLeft = value;
            }
        }

        private int cursorTop;

        public int CursorTop
        {
            get => cursorTop;
            set
            {
                if (value < 0 || value >= BufferHeight)
                {
                    throw new ArgumentException("Cursor top is outside buffer height.");
                }
                cursorTop = value;
            }
        }

        private int scroll;

        public int Scroll
        {
            get => scroll;
            set => MiscUtils.QueueSet(ref scroll, value, ref renderQueued);
        }

        public bool Autoscroll { get; set; } = true;

        public int AutoScrollOffset { get; set; } = 0;

        public SCEColor FgColor { get; set; } = SCEColor.Gray;

        public SCEColor BgColor { get; set; } = SCEColor.Black;

        public int TabSize { get; set; } = 8;

        public void ShiftCursor(int move, bool newlineOverflow = true)
        {
            if (move == 0)
            {
                return;
            }

            if (newlineOverflow)
            {
                CursorIndex += move;

                int dif = CursorTop + 1 - BufferHeight;
                if (dif > 0)
                {
                    BufferLines(dif);
                }

                CursorIndex = Utils.Clamp(CursorIndex, 0, BufferSize - 1);
            }
            else
            {
                CursorLeft = Utils.Clamp(CursorLeft + move, 0, BufferWidth - 1);
            }
        }

        public void BufferLines(int lines)
        {
            lines = Math.Min(lines, BufferHeight);
            var count = BufferWidth * lines;
            buffer.Shift(count);
            buffer.Fill(Pixel.Empty, BufferSize - count, count);
        }

        public void Newline(int count, bool carriageReturn = true)
        {
            if (count == 0)
            {
                return;
            }

            int next = CursorTop + count;
            int dif = next + 1 - BufferHeight;
            if (dif > 0)
            {
                CursorTop = BufferHeight - 1;
                BufferLines(dif);
            }
            else
            {
                CursorTop = next;
            }
            
            if (carriageReturn)
            {
                CursorLeft = 0;
            }
        }

        public void Newline(bool carriageReturn = true)
        {
            Newline(1, carriageReturn);
        }

        public void Write(string? str)
        {
            if (str == null)
            {
                return;
            }
            for (int i = 0; i < str.Length; ++i)
            {
                switch (str[i])
                {
                    case '\v':
                    case '\f':
                    case '\n':
                        Newline();
                        break;
                    case '\r':
                        CursorLeft = 0;
                        break;
                    case '\b':
                        ShiftCursor(-1, false);
                        break;
                    case '\a':
                        break;
                    case '\t':
                        ShiftCursor(TabSize);
                        break;
                    default:
                        this[CursorPos] = new(str[i], FgColor, BgColor);
                        if (CursorLeft == BufferWidth - 1 && (i == str.Length - 1 || !(str[i + 1] is '\n' or '\f' or '\v')))
                        {
                            Newline();
                        }
                        else
                        {
                            ShiftCursor(1, false);
                        }
                        break;
                }
            }

            renderQueued = true;

            if (Autoscroll && CursorTop - Scroll - AutoScrollOffset == Height && CursorTop < BufferHeight - 1)
            {
                ++Scroll;
            }
        }

        public void Write(char c)
        {
            Write(c.ToString());
        }

        public void Write(object obj)
        {
            if (obj?.ToString() is string s)
            {
                Write(s);
            }
        }

        public void WriteLine()
        {
            Write("\n");
        }

        public void WriteLine(string str)
        {
            Write(str + "\n");
        }

        public void WriteLine(object obj)
        {
            if (obj?.ToString() is string s)
            {
                WriteLine(s);
            }
        }

        public void Shift(int index, int count)
        {
            int end = index + count;
            for (int i = 0; i < count; ++i)
            {
                this[end - i] = this[end - i - 1];
            }
            this[index] = Pixel.Empty;
            renderQueued = true;
        }

        public void Shift(Vector2Int pos, int count)
        {
            Shift(Translate(pos), count);
        }

        public void Delete()
        {
            ShiftCursor(-1);
            this[CursorPos] = Pixel.Empty;
            renderQueued = true;
        }

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            renderQueued = true;
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        public void MapResizeBuffer(int width, int height)
        {
            int bufSize = width * height;

            if (height != BufferHeight && width == BufferWidth)
            {
                buffer.Resize(bufSize);
            }
            else
            {
                CycleBuffer<Pixel> newBuffer = new(bufSize);

                int mapWidth = Math.Min(BufferWidth, width);
                int mapHeight = Math.Min(BufferHeight, height);

                foreach (var pos in new Rect2DInt(mapWidth, mapHeight))
                {
                    newBuffer[pos.Y * BufferWidth + pos.X] = this[pos];
                }

                buffer = newBuffer;
            }

            bufferDimensions = new(width, height);
        }

        public void MapResizeBuffer(Vector2Int dimensions)
        {
            MapResizeBuffer(dimensions.X, dimensions.Y);
        }

        public void CleanResizeBuffer(int width, int height)
        {
            buffer.CleanResize(width * height);
            bufferDimensions = new(width, height);
        }

        public void CleanResizeBuffer(Vector2Int dimensions)
        {
            CleanResizeBuffer(dimensions.X, dimensions.Y);
        }

        public void ClearBuffer()
        {
            buffer.Clear();
        }

        public void Clear()
        {
            Scroll = 0;
            CursorIndex = 0;
            buffer.Clear();
            renderQueued = true;
        }

        public override MapView<Pixel> GetMapView()
        {
            if (renderQueued)
            {
                for (int i = 0; i < Height; ++i)
                {
                    var area = Rect2DInt.Horizontal(i, Width);

                    int y = i + Scroll;
                    if (y >= 0 && y < BufferHeight)
                    {
                        _dpMap.Fill(pos => pos.X < BufferWidth ? this[y, pos.X] : Pixel.Empty, area);
                    }
                    else
                    {
                        _dpMap.Fill(Pixel.Empty, area);
                    }
                }

                renderQueued = false;
            }

            return _dpMap;
        }
    }
}
