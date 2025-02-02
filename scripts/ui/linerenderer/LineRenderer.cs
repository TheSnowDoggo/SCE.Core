using System.Collections;

namespace SCE
{
    public class LineRenderer : UIBaseExt, IEnumerable<Line?>
    {
        private const string DEFAULT_NAME = "line_renderer";

        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        private Line?[] lineArr;

        #region BackingFields
        private SCEColor bgColor;

        private StackType stackMode = StackType.TopDown;

        private bool fitToLength = false;
        #endregion

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

        #region Settings

        public SCEColor BgColor
        {
            get => bgColor;
            set
            {
                bgColor = value;
                Update();
            }
        }

        public StackType StackMode
        {
            get => stackMode;
            set
            {
                stackMode = value;
                Update();
            }
        }

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

        public void MapRange(int y, string[] messageArr, ColorSet? colorSet = null)
        {
            for (int i = 0; i < messageArr.Length; ++i)
                SetLine(y + i, messageArr[i], colorSet);
        }

        public bool RemoveLine(int y)
        {
            return SetLine(y, null);
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
            Line? rawLine = lineArr[y];
            if (rawLine is not null)
                MapLine(Translate(y), (Line)rawLine);
            else
                ClearAt(Translate(y));
        }

        private void MapLine(int mappedY, Line line)
        {
            if (fitToLength)
                _dpMap.MapString(mappedY, GetAnchoredMessage(line), line.Colors);
            else
            {
                ClearAt(mappedY);
                _dpMap.MapString(new Vector2Int(GetAnchoredX(line.Anchor, line.Message.Length), mappedY), line.Message, line.Colors);
            }
        }

        private string GetAnchoredMessage(Line line)
        {
            return line.Anchor switch
            {
                LineAnchor.Left => StringUtils.PostFitToLength(line.Message, Width),
                LineAnchor.Right => StringUtils.PreFitToLength(line.Message, Width),
                LineAnchor.CenterLB => StringUtils.Copy(' ', GetAnchoredX(LineAnchor.CenterLB, line.Message.Length))
                + line.Message + StringUtils.Copy(' ', GetAnchoredX(LineAnchor.CenterRB, line.Message.Length)),
                LineAnchor.CenterRB => StringUtils.Copy(' ', GetAnchoredX(LineAnchor.CenterRB, line.Message.Length))
                + line.Message + StringUtils.Copy(' ', GetAnchoredX(LineAnchor.CenterLB, line.Message.Length)),
                _ => throw new NotImplementedException()
            };
        }

        private int GetAnchoredX(LineAnchor anchor, int msgLength)
        {
            return anchor switch
            {
                LineAnchor.Left => 0,
                LineAnchor.Right => Width - msgLength,
                LineAnchor.CenterLB => (Width - msgLength) / 2,
                LineAnchor.CenterRB => (int)Math.Ceiling((Width - msgLength) / 2.0),                            
                _ => throw new NotImplementedException()
            };
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
        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            _dpMap.Data.Fill(new Pixel(BgColor));
            lineArr = new Line?[height];
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
