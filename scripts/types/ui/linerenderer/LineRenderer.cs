namespace SCE
{
    public class LineRenderer : UIBaseExt
    {
        private const string DEFAULT_NAME = "line_renderer";

        private const Color DEFAULT_BGCOLOR = Color.Black;

        private readonly Queue<int> updateQueue = new();

        private Line[] lineArray;

        public LineRenderer(string name, int width, int height, Color? bgColor = null)
            : base(name, width, height, bgColor)
        {
            lineArray = new Line[Dimensions.Y];
            BgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public LineRenderer(string name, Vector2Int dimensions, Color? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public LineRenderer(int width, int height, Color? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public LineRenderer(Vector2Int dimensions, Color? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        public Color BgColor { get; set; }

        public StackMode Mode { get; } = StackMode.TopDown;

        public bool FitLinesToLength { get; set; } = false;

        public Line this[int y]
        {
            get => lineArray[TranslateY(y)];
            set => SetLine(y, value);
        }

        public void SetLine(int y, Line line)
        {
            int translatedY = TranslateY(y);
            lineArray[translatedY] = line;
            updateQueue.Enqueue(translatedY);
        }

        public void ShiftUp(int shift)
        {
            var newLineArray = new Line[lineArray.Length - shift];
            for (int i = 0; i < newLineArray.Length; ++i)
            {
            }
            Clear();
        }

        public void Clear()
        {
            _dpMap.BgColorFill(BgColor);
            lineArray = new Line[Dimensions.Y];
            updateQueue.Clear();
        }

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            lineArray = new Line[height];
            updateQueue.Clear();
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        protected override void Render()
        {
            foreach (int y in updateQueue)
            {
                _dpMap.FillHorizontal(new Pixel(BgColor), y);

                var line = lineArray[y];

                string data = FitLinesToLength ? StringUtils.PostFitToLength(line.Data, Width * Pixel.PIXELWIDTH) : line.Data;

                if (Pixel.GetPixelLength(data) > Width)
                    throw new InvalidOperationException("Text data exceeds dimensions.");

                _dpMap.MapString(new Vector2Int(0, y), data, line.FgColor, line.BgColor);
            }

            updateQueue.Clear();
        }

        private int TranslateY(int y)
        {
            return Mode switch
            {
                StackMode.BottomUp => y,
                StackMode.TopDown => Dimensions.Y - 1 - y,
                _ => throw new NotImplementedException("Unknown mode.")
            };
        }
    }
}
