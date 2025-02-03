namespace SCE
{
    public class ConsoleEmulator : UIBaseExt
    {
        private const string DEFAULT_NAME = "console_emulator";

        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;
        private const SCEColor DEFAULT_FGCOLOR = SCEColor.Gray;

        private readonly DisplayMap consoleBuffer;

        #region Constructors

        public ConsoleEmulator(string name, int width, int height, SCEColor? bgColor = null)
            : base(name, width, height, bgColor)
        {
            BgColor = bgColor ?? DEFAULT_BGCOLOR;
            consoleBuffer = new(width, height, bgColor);
        }

        public ConsoleEmulator(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public ConsoleEmulator(int width, int height, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public ConsoleEmulator(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        #endregion

        #region Settings

        public SCEColor BgColor { get; set; }

        public SCEColor FgColor { get; set; } = DEFAULT_FGCOLOR;

        private Vector2Int cursorPos;

        public Vector2Int CursorPos
        {
            get => cursorPos;
            set => SetCursorPos(value);
        }

        private void SetCursorPos(Vector2Int value)
        {
            if (value < 0 || value >= Dimensions)
                throw new ArgumentException("Invalid Cursor Position.");
            cursorPos = value;
        }

        #endregion

        public void Write(string str)
        {
            foreach (char chr in str)
                WriteChar(chr);
        }

        private void WriteChar(char chr)
        {
            bool newLine = cursorPos.X == Width || chr == '\n';
            bool carriageReturn = newLine || chr == '\r';

            if (carriageReturn)
                cursorPos.X = 0;
            if (newLine)
                ++cursorPos.Y;
                
            consoleBuffer[cursorPos.X, TranslateY(cursorPos.Y)] = new Pixel(chr, FgColor, BgColor);
            ++cursorPos.X;
        }

        public void MapBuffer()
        {
            _dpMap.MapFrom(consoleBuffer, Vector2Int.Zero);
        }

        #region Translate

        private int TranslateY(int y)
        {
            return Height - y - 1;
        }

        #endregion
    }
}
