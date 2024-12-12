namespace SCE
{
    public class LineRenderer : IRenderable
    {
        private const bool DefaultActiveState = true;

        private const bool DefaultFitLinesToLength = false;

        private const Color DefaultBgColor = Color.Black;

        private const StackMode DefaultMode = StackMode.TopDown;

        private readonly DisplayMap dpMap;

        private readonly Queue<int> updateQueue = new();

        private Line[] lineArray;

        public LineRenderer(Vector2Int dimensions, Color bgColor, StackMode mode = DefaultMode)
        {
            dpMap = new(dimensions, bgColor);

            lineArray = new Line[Dimensions.Y];

            BgColor = bgColor;

            Mode = mode;
        }

        public LineRenderer(Vector2Int dimensions, StackMode mode = DefaultMode)
            : this(dimensions, DefaultBgColor, mode)
        {
        }

        public bool IsActive { get; set; } = DefaultActiveState;

        public Color BgColor { get; set; }

        public StackMode Mode { get; }

        public bool FitLinesToLength { get; set; } = DefaultFitLinesToLength;

        public Vector2Int Position { get; set; }

        public int Layer { get; set; }

        public int Width { get => dpMap.Width; }

        public int Height { get => dpMap.Height; }

        public Vector2Int Dimensions { get => dpMap.Dimensions; }

        public Line this[int y]
        {
            get => lineArray[TranslateY(y)];
            set
            {
                int translatedY = TranslateY(y);
                lineArray[translatedY] = value;
                updateQueue.Enqueue(translatedY);
            }
        }

        public DisplayMap GetMap()
        {
            RenderQueue();
            return dpMap;
        }

        public void Clear()
        {
            dpMap.Fill(new Pixel(BgColor));
            lineArray = new Line[Dimensions.Y];
            updateQueue.Clear();
        }

        private void RenderQueue()
        {
            foreach (int y in updateQueue)
            {
                dpMap.FillHorizontal(new Pixel(BgColor), y);

                Line line = lineArray[y];

                string data = FitLinesToLength ? StringUtils.PostFitToLength(line.Data, Width * Pixel.PIXELWIDTH) : line.Data;

                if (Pixel.GetPixelLength(data) > Width)
                {
                    throw new InvalidOperationException("Text data exceeds dimensions.");
                }

                dpMap.MapLine(new Vector2Int(0, y), data, line.FgColor, line.BgColor);
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
