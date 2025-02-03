using System.Collections;

namespace SCE
{
    public class LineRenderer : UIBaseExt, IEnumerable<Line?>
    {
        private const string DEFAULT_NAME = "line_renderer";

        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        private Line?[] lineArr;

        #region Constructors

        public LineRenderer(string name, int width, int height, SCEColor? bgColor = null)
            : base(name, width, height, bgColor)
        {
            lineArr = new Line?[Height];
            this.bgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public LineRenderer(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public LineRenderer(int width, int height, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public LineRenderer(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        #endregion

        #region Indexers

        public Line? this[int y]
        {
            get => GetThis(y);
            set => SetThis(y, value);
        }

        private Line? GetThis(int y)
        {
            if (y < 0 || y >= lineArr.Length)
                throw new IndexOutOfRangeException("Specified y is invalid.");
            return lineArr[Translate(y)];
        }

        private void SetThis(int y, Line? value)
        {
            if (!SetLine(y, value))
                throw new IndexOutOfRangeException("Specified y is invalid.");
        }

        #endregion

        #region Settings

        private SCEColor bgColor;

        public SCEColor BgColor
        {
            get => bgColor;
            set => SetBgColor(value);
        }

        private void SetBgColor(SCEColor value)
        {
            bgColor = value;
            Update();
        }

        private StackType stackMode = StackType.TopDown;

        public StackType StackMode
        {
            get => stackMode;
            set => SetStackMode(value);
        }

        private void SetStackMode(StackType value)
        {
            stackMode = value;
            Update();
        }

        private bool fitToLength = false;

        public bool FitToLength
        {
            get => fitToLength;
            set => SetFitToLength(value);
        }

        private void SetFitToLength(bool value)
        {
            fitToLength = value;
            Update();
        }

        #endregion

        #region IEnumerable

        public IEnumerator<Line?> GetEnumerator()
        {
            return (IEnumerator<Line?>)lineArr.GetEnumerator();
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
            lineArr[y] = line;
            RenderLine(y);
            return true;
        }

        public bool SetLine(int y, string message, ColorSet? colorSet = null)
        {
            return SetLine(y, new Line(message, colorSet));
        }

        public bool SetLine(int y, string message, SCEColor fgColor, SCEColor? bgColor = null)
        {
            return SetLine(y, new Line(message, fgColor, bgColor));
        }

        public void SetRange(int startY, IEnumerable<Line?> collection)
        {
            foreach (var line in collection)
                SetLine(startY++, line);
        }

        public bool ClearLine(int y)
        {
            return SetLine(y, null);
        }

        public void ClearRange(int startY, int count)
        {
            for (int i = 0; i < count; ++i)
                ClearLine(startY + i);
        }

        #endregion

        #region Render

        public void Update()
        {
            for (int i = 0; i < Height; ++i)
                RenderLine(i);
        }

        private void RenderLine(int y)
        {
            if (lineArr[y] is Line line)
                MapLine(Translate(y), line);
            else
                ClearAt(Translate(y));
        }

        private void MapLine(int mappedY, Line line)
        {
            if (fitToLength)
                _dpMap.MapString(mappedY, AnchorUtils.GetHorizontalAnchoredMessage(line.Anchor, line.Message, Width), line.Colors);
            else
            {
                ClearAt(mappedY);
                _dpMap.MapString(new Vector2Int(AnchorUtils.HorizontalAnchoredStart(line.Anchor, line.Message.Length, Width), mappedY), line.Message, line.Colors);
            }
        }

        private void ClearAt(int y)
        {
            _dpMap.Data.FillHorizontal(new Pixel(BgColor), y);
        }

        #endregion

        #region Clear

        public void Clear()
        {
            _dpMap.Data.Fill(new Pixel(BgColor));
            Array.Clear(lineArr);
        }

        #endregion

        #region Resize

        public void CleanResize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            _dpMap.Data.Fill(new Pixel(BgColor));
            lineArr = new Line?[height];
        }

        public void CleanResize(Vector2Int dimensions)
        {
            CleanResize(dimensions.X, dimensions.Y);
        }

        public void MapResize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            Array.Resize(ref lineArr, height);
            Update();
        }

        public void MapResize(Vector2Int dimensions)
        {
            MapResize(dimensions.X, dimensions.Y);
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
