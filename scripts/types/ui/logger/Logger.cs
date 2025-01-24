namespace SCE
{
    public class Logger : UIBaseExt
    {
        public enum DisplayType
        {
            Full,
            MessageOnly,
        }

        private const string DEFAULT_NAME = "logger";

        private const Color DEFAULT_BGCOLOR = Color.Black;

        private readonly List<Log> _logs = new();

        private StackType stackMode = StackType.TopDown;

        private DisplayType displayMode = DisplayType.Full;

        private int viewY = 0;

        public Logger(string name, int width, int height, Color? bgColor = null)
            : base(name, width, height, bgColor)
        {
            BgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public Logger(string name, Vector2Int dimensions, Color? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Logger(int width, int height, Color? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public Logger(Vector2Int dimensions, Color? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        public int Logs { get => _logs.Count; }

        public Color BgColor { get; set; }

        #region Settings
        public StackType StackMode
        {
            get => stackMode;
            set
            {
                stackMode = value;
                RenderLogs();
            }
        }

        public DisplayType DisplayMode
        {
            get => displayMode;
            set
            {
                displayMode = value;
                RenderLogs();
            }
        }

        public bool FollowNewest { get; set; } = true;

        public bool AlwaysFollow { get; set; } = false;

        public bool AllowNegativeY { get; set; } = false;
        #endregion

        #region Transformation
        public void SetY(int y)
        {
            if (!AllowNegativeY && y < 0)
                return;
            viewY = y;
            RenderLogs();
        }

        public void RelativeMove(int y)
        {
            SetY(viewY + y);
        }

        public void GridMove(int y)
        {
            RelativeMove(ViewTranslate(y));
        }
        #endregion

        #region Log
        public void Log(IEnumerable<Log> collection)
        {
            foreach (var log in collection)
                RawLog(log);
            RenderLogs();
        }

        public void Log(Log log)
        {
            RawLog(log);
            RenderLogs();
        }

        public void Log(string message, ColorSet? colorSet = null)
        {
            Log(new Log(message, colorSet));
        }

        private void RawLog(Log log)
        {
            _logs.Add(log);
            if (FollowNewest && (AlwaysFollow || Logs - viewY > Height))
                viewY++;
        }

        public bool Delete(Log log)
        {
            if (!_logs.Remove(log))
                return false;
            RenderLogs();
            return true;
        }

        public void DeleteAt(int index)
        {
            _logs.RemoveAt(index);
            RenderLogs();
        }

        public void DeleteRange(int index, int count)
        {
            _logs.RemoveRange(index, count);
            RenderLogs();
        }

        public void DeleteLast()
        {
            if (_logs.Count > 0)
                _logs.RemoveAt(_logs.Count - 1);
            RenderLogs();
        }
        #endregion

        #region Render
        private void RenderLogs()
        {
            for (int gridY = 0; gridY < Height; ++gridY)
            {
                int mappedY = GridTranslate(gridY + ViewTranslate(viewY));
                if (mappedY >= 0 && mappedY < _logs.Count)
                    MapLog(gridY, _logs[mappedY]);
                else
                    ClearAt(gridY);
            }
        }

        private void MapLog(int y, Log log)
        {
            _dpMap.MapString(new Vector2Int(0, y), GetLogString(log), log.Colors.FgColor, log.Colors.BgColor);
        }

        private string GetLogString(Log log)
        {
            return DisplayMode switch
            {
                DisplayType.Full => log.ToString(),
                DisplayType.MessageOnly => log.Message,
                _ => throw new NotImplementedException()
            };
        }

        private void ClearAt(int y)
        {
            _dpMap.FillHorizontal(new Pixel(BgColor), y);
        }
        #endregion

        #region Clear
        public void Clear()
        {
            _logs.Clear();
            _dpMap.Clear();
        }
        #endregion

        #region Translate
        private int GridTranslate(int y)
        {
            return StackMode == StackType.BottomUp ? y : Height - y - 1;
        }

        private int ViewTranslate(int y)
        {
            return StackMode == StackType.BottomUp ? y : -y;
        }
        #endregion
    }
}
