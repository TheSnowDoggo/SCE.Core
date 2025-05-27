using System.Collections;
using CSUtils;
namespace SCE
{
    public class LineRenderer : UIBaseExt, IEnumerable<Line?>
    {
        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        private Line?[] lineArr;

        public LineRenderer(int width, int height, SCEColor? bgColor = null)
            : base(width, height, bgColor)
        {
            lineArr = new Line?[Height];
            this.bgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public LineRenderer(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

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

        public Line? this[int y]
        {
            get
            {
                if (y < 0 || y >= lineArr.Length)
                    throw new IndexOutOfRangeException("Specified y is invalid.");
                return lineArr[Translate(y)];
            }
            set
            {
                if (!SetLine(y, value))
                    throw new IndexOutOfRangeException("Specified y is invalid.");
            }
        }

        #region Settings

        private SCEColor bgColor;

        public SCEColor BgColor
        {
            get => bgColor;
            set
            {
                bgColor = value;
                Update();
            }
        }

        private StackType stackMode = StackType.TopDown;

        public StackType StackMode
        {
            get => stackMode;
            set
            {
                stackMode = value;
                Update();
            }
        }

        private bool fitToLength = false;

        public bool FitToLength
        {
            get => fitToLength;
            set
            {
                fitToLength = value;
                Update();
            }
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
            if (FitToLength)
            {
                _dpMap.MapString(Utils.FTL(line.Message, line.Message.Length, ' ', (Utils.FMode)(Anchor | AnchorUtils.H_MASK)), mappedY, line.Colors);
            }
            else
            {
                ClearAt(mappedY);
                _dpMap.MapString(line.Message, new Vector2Int(AnchorUtils.HorizontalFix(line.Anchor, Width - line.Message.Length), mappedY), line.Colors);
            }
        }

        private void ClearAt(int y)
        {
            _dpMap.Fill(new Pixel(BgColor), Rect2D.Horizontal(y, _dpMap.Width));
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

        public void CleanResize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            _dpMap.Fill(new Pixel(BgColor));
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
