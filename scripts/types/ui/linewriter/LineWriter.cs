namespace SCE
{
    using System.Text;

    public class LineWriter : IRenderable
    {
        private const bool DefaultActiveState = true;

        private const HandleType DefaultOverflowHandling = HandleType.Error;

        private const Color DefaultBgColor = Color.Black;

        private const Color DefaultFgColor = Color.White;

        private readonly DisplayMap dpMap;

        private Vector2Int cursorPos = Vector2Int.Zero;

        public LineWriter(Vector2Int dimensions, Color initialBgColor)
        {
            dpMap = new(dimensions, initialBgColor);

            BgColor = initialBgColor;
        }

        public LineWriter(Vector2Int dimensions)
            : this(dimensions, DefaultBgColor)
        {
        }

        public bool IsActive { get; set; } = DefaultActiveState;

        public HandleType OverflowHandling { get; set; } = DefaultOverflowHandling;

        public Color FgColor { get; set; } = DefaultFgColor;

        public Color BgColor { get; set; }

        public Vector2Int CursorPos
        {
            get => cursorPos;
            set
            {
                if (!IsNewCursorValid(value))
                {
                    throw new ArgumentException("Cursor position is invalid.");
                }

                cursorPos = value;
            }
        }

        public Vector2Int Position { get; set; }

        public int Layer { get; set; }

        public int Width { get => dpMap.Width; }

        public int Height { get => dpMap.Height; }

        public Vector2Int Dimensions { get => dpMap.Dimensions; }

        public DisplayMap GetMap()
        {
            return dpMap;
        }

        public enum HandleType
        {
            Error,
            Clear,
            ContinueAtTop,
            Discard,
            DiscardClear,
        }

        public void Clear(bool resetCursor = true)
        {
            dpMap.Fill(new Pixel(BgColor));

            if (resetCursor)
                ResetCursor();
        }

        public void ResetCursor()
        {
            cursorPos = Vector2Int.Zero;
        }

        private static bool IsCommand(char chr)
        {
            return chr is '\r' or '\n';
        }

        private bool IsNewCursorValid(Vector2Int newPos)
        {
            return dpMap.GridArea.Contains(Translate(newPos));
        }

        public void CleanResize(Vector2Int dimensions)
        {
            dpMap.CleanResize(dimensions);

            dpMap.BgColorFill(BgColor);
        }

        public void Resize(Vector2Int dimensions)
        {
            DisplayMap transferMap = new(dimensions, BgColor);

            transferMap.MapTo(dpMap, Vector2Int.Up * (dimensions - Dimensions), true);

            dpMap.CleanResize(dimensions);

            dpMap.MapTo(transferMap);
        }

        #region Write
        public void Write(string str)
        {
            StringBuilder buffer = new();

            int mod = cursorPos.X % Pixel.PIXELWIDTH;
            if (mod != 0)
            {
                Vector2Int pos = Translate(cursorPos);

                buffer.Append(dpMap[pos].Element is null ? StringUtils.Copy(' ', mod) : dpMap[pos].Element[..mod]);
            }

            for (int i = 0; i < str.Length; ++i)
            {
                bool last = i == str.Length - 1;

                char chr = str[i];

                bool command = IsCommand(chr);

                if (!command)
                {
                    buffer.Append(chr);
                    cursorPos.X++;
                }

                bool overflow = !command && TranslateX(cursorPos.X) == Dimensions.X && (last || !IsCommand(str[i + 1]));

                bool newLine = chr == '\n' || overflow;

                bool carriageReturn = chr == '\r' || (newLine && (last || str[i + 1] != '\r'));

                if (last || carriageReturn || newLine)
                {
                    if (TranslateY(cursorPos.Y) < 0)
                    {
                        switch (OverflowHandling)
                        {
                            case HandleType.Error:
                                throw new Exception("Line overflow.");
                            case HandleType.ContinueAtTop:
                                cursorPos.Y = 0;
                                break;
                            case HandleType.Clear:
                                Clear(false);
                                cursorPos.Y = 0;
                                break;
                            case HandleType.Discard:
                                cursorPos = Vector2Int.Zero;
                                return; // Discard
                            case HandleType.DiscardClear:
                                Clear(true);
                                return; // Discard
                        }
                    }

                    string bufferStr = buffer.ToString();

                    buffer.Clear();

                    Vector2Int mapPos = new((cursorPos.X - bufferStr.Length) / Pixel.PIXELWIDTH, TranslateY(cursorPos.Y));

                    dpMap.MapLine(mapPos, bufferStr, FgColor, BgColor);
                }

                if (carriageReturn)
                    cursorPos.X = 0;
                if (newLine)
                    cursorPos.Y++;
            }
        }

        public void WriteLine(string str)
        {
            Write(str + '\n');
        }
        #endregion

        #region Translation
        private static int TranslateX(int x)
        {
            return x / Pixel.PIXELWIDTH;
        }

        private int TranslateY(int y)
        {
            return Dimensions.Y - 1 - y;
        }

        private Vector2Int Translate(Vector2Int pos)
        {
            return new(TranslateX(pos.X), TranslateY(pos.Y));
        }
        #endregion
    }
}
