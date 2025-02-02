namespace SCE
{
    using System.Text;

    public class LineWriter : UIBaseExt
    {        
        private const string DEFAULT_NAME = "line_writer";

        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        private const HandleType DefaultOverflowHandling = HandleType.Error;

        private Vector2Int cursorPos = Vector2Int.Zero;

        public LineWriter(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : base(name, dimensions, bgColor)
        {
            BgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public LineWriter(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        public HandleType OverflowHandling { get; set; } = DefaultOverflowHandling;

        public SCEColor FgColor { get; set; } = SCEColor.White;

        public SCEColor BgColor { get; set; }

        public Vector2Int CursorPos
        {
            get => cursorPos;
            set => cursorPos = IsNewCursorValid(value) ? value : throw new ArgumentException("Cursor position is invalid.");
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
            _dpMap.Data.Fill(new Pixel(BgColor));

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
            return _dpMap.GridArea.Contains(Translate(newPos));
        }

        public void CleanResize(Vector2Int dimensions)
        {
            _dpMap.CleanResize(dimensions);

            _dpMap.BgColors.Fill(BgColor);
        }

        public void Resize(Vector2Int dimensions)
        {
            DisplayMap transferMap = new(dimensions, BgColor);

            transferMap.MapTo(_dpMap, Vector2Int.Up * (dimensions - Dimensions), true);

            _dpMap.CleanResize(dimensions);

            _dpMap.MapTo(transferMap);
        }

        #region Write
        public void Write(string str)
        {
            StringBuilder buffer = new();

            buffer.Append(_dpMap[Translate(cursorPos)].Element);

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

                bool overflow = !command && cursorPos.X == Dimensions.X && (last || !IsCommand(str[i + 1]));

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

                    Vector2Int mapPos = new(cursorPos.X - bufferStr.Length, TranslateY(cursorPos.Y));

                    _dpMap.MapString(mapPos, bufferStr, FgColor, BgColor);
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
        private int TranslateY(int y)
        {
            return Dimensions.Y - 1 - y;
        }

        private Vector2Int Translate(Vector2Int pos)
        {
            return new(pos.X, TranslateY(pos.Y));
        }
        #endregion
    }
}
