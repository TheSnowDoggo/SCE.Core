namespace SCECore
{
    public class LineRenderer : IRenderable
    {
        private const bool DefaultActiveState = true;

        private const bool DefaultFitLinesToLength = false;

        private const Color DefaultBgColor = Color.Black;

        private const StackMode DefaultMode = StackMode.TopDown;

        private readonly Image image;

        private readonly Queue<int> updateQueue = new();

        private Line[] lineArray;

        public LineRenderer(Vector2Int dimensions, Color bgColor, StackMode mode = DefaultMode)
        {
            image = new(dimensions, bgColor);

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

        public Vector2Int Position
        {
            get => image.Position;
            set => image.Position = value;
        }

        public int Layer
        {
            get => image.Layer;
            set => image.Layer = value;
        }

        public int Width { get => image.Width; }

        public int Height { get => image.Height; }

        public Vector2Int Dimensions { get => image.Dimensions; }

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

        public Image GetImage()
        {
            RenderQueue();
            return image;
        }

        public void Clear()
        {
            image.Fill(new Pixel(BgColor));
            lineArray = new Line[Dimensions.Y];
            updateQueue.Clear();
        }

        private void RenderQueue()
        {
            foreach (int y in updateQueue)
            {
                image.FillHorizontal(new Pixel(BgColor), y);

                Line line = lineArray[y];

                string data = FitLinesToLength ? StringUtils.FitToLength(line.Data, Width * Pixel.PIXELWIDTH) : line.Data;

                if (Pixel.GetPixelLength(data) > Width)
                {
                    throw new InvalidOperationException("Text data exceeds dimensions.");
                }

                image.MapLine(new Vector2Int(0, y), data, line.FgColor, line.BgColor);
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
