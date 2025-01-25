using System.Collections;

namespace SCE
{
    public class LineRenderer : UIBaseExt, IEnumerable<Log?>
    {
        private const string DEFAULT_NAME = "line_renderer";

        private const Color DEFAULT_BGCOLOR = Color.Black;

        private Color bgColor;

        private StackType stackMode = StackType.BottomUp;

        private bool fitToLength = false;

        private Line?[] lineArr;

        public LineRenderer(string name, int width, int height, Color? bgColor = null)
            : base(name, width, height, bgColor)
        {
            lineArr = new Line[Height];
            this.bgColor = bgColor ?? DEFAULT_BGCOLOR;
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

        public int Count { get => lineArr.Length; }

        #region Settings
        public Color BgColor
        {
            get => bgColor;
            set
            {
                bgColor = value;
                FullRender();
            }
        }

        public StackType StackMode
        {
            get => stackMode;
            set
            {
                stackMode = value;
                FullRender();
            }
        }

        public bool FitToLength
        {
            get => fitToLength;
            set
            {
                fitToLength = value;
                FullRender();
            }
        }
        #endregion

        #region Indexers
        public Line? this[int y]
        {
            get => y >= 0 && y < lineArr.Length ? lineArr[Translate(y)] : 
                throw new IndexOutOfRangeException("Specified y is invalid.");
            set
            {
                if (!SetLine(y, value))
                    throw new IndexOutOfRangeException("Specified y is invalid.");
            }
        }
        #endregion

        #region Enumeration
        public IEnumerator<Log?> GetEnumerator()
        {
            return (IEnumerator<Log?>)lineArr.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region SetLine
        public bool SetLine(int y, Line? line)
        {
            if (y < 0 || y >= lineArr.Length)
                return false;
            int mappedY = Translate(y);
            lineArr[mappedY] = line;
            RenderLine(mappedY);
            return true;
        }

        public bool SetLine(int y, string message, ColorSet? colorSet = null)
        {
            return SetLine(y, new Line(message, colorSet));
        }

        public bool SetLine(int y, string message, Color fgColor, Color? bgColor = null)
        {
            return SetLine(y, new Line(message, fgColor, bgColor));
        }

        public bool RemoveLine(int y)
        {
            return SetLine(y, null);
        }
        #endregion

        #region Render
        private void RenderLine(int mappedY)
        {
            Line? rawLine = lineArr[mappedY];
            if (rawLine is not null)
                MapLine(mappedY, rawLine);
            else
                ClearAt(mappedY);
        }

        private void FullRender()
        {
            for (int y = 0; y < Height; ++y)
                RenderLine(Translate(y));
        }

        private void MapLine(int y, Line line)
        {
            if (fitToLength)
                _dpMap.MapString(y, StringUtils.PostFitToLength(line.Message, Width * Pixel.PIXELWIDTH), line.Colors);
            else
            {
                ClearAt(y);
                _dpMap.MapString(y, line.Message, line.Colors);
            }
        }

        private void ClearAt(int y)
        {
            _dpMap.FillHorizontal(new Pixel(BgColor), y);
        }
        #endregion

        #region Clear
        public void Clear()
        {
            _dpMap.Fill(new Pixel(BgColor));
            Array.Clear(lineArr);
        }
        #endregion

        #region Resize
        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            _dpMap.Fill(new Pixel(BgColor));
            lineArr = new Line[height];
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }
        #endregion

        #region Translate
        private int Translate(int y)
        {
            return StackMode == StackType.BottomUp ? y : Height - y - 1;
        }
        #endregion
    }
}
