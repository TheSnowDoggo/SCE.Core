namespace SCE
{
    public class LineRenderer : UIBase
    {
        private const bool DefaultFitLinesToLength = false;

        private const Color DefaultBgColor = Color.Black;

        private const StackMode DefaultMode = StackMode.TopDown;

        private readonly Queue<int> updateQueue = new();

        private Line[] lineArray;

        public LineRenderer(Vector2Int dimensions, Color bgColor, StackMode mode = DefaultMode)
            : base(dimensions, bgColor)
        {
            lineArray = new Line[Dimensions.Y];

            BgColor = bgColor;

            Mode = mode;
        }

        public LineRenderer(Vector2Int dimensions, StackMode mode = DefaultMode)
            : this(dimensions, DefaultBgColor, mode)
        {
        }

        public Color BgColor { get; set; }

        public StackMode Mode { get; }

        public bool FitLinesToLength { get; set; } = DefaultFitLinesToLength;

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

        public void Clear()
        {
            _dpMap.Fill(new Pixel(BgColor));
            lineArray = new Line[Dimensions.Y];
            updateQueue.Clear();
        }

        protected override void Render()
        {
            foreach (int y in updateQueue)
            {
                _dpMap.FillHorizontal(new Pixel(BgColor), y);

                Line line = lineArray[y];

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
