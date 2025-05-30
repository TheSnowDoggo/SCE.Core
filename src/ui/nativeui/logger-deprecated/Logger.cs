﻿using CSUtils;
namespace SCE
{
    public class Logger : UIBaseExt
    {
        public enum DisplayType
        {
            Full,
            MessageOnly,
        }

        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        private readonly List<Log> _logs = new();

        private int viewY = 0;

        public Logger(int width, int height, SCEColor? bgColor = null)
            : base(width, height, bgColor)
        {
            this.bgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public Logger(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        public int Count { get => _logs.Count; }

        public IList<Log> ILogs { get => _logs.AsReadOnly(); }

        public Log this[int index]
        {
            get => _logs[index];
            set
            {
                _logs[index] = value;
                RenderAll();
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
                RenderAll();
            }
        }

        private StackType stackMode = StackType.TopDown;

        public StackType StackMode
        {
            get => stackMode;
            set
            {
                stackMode = value;
                RenderAll();
            }
        }

        private DisplayType displayMode = DisplayType.Full;

        public DisplayType DisplayMode
        {
            get => displayMode;
            set
            {
                displayMode = value;
                RenderAll();
            }
        }

        private bool fitLinesToLength = true;

        public bool FitToLength
        {
            get => fitLinesToLength;
            set
            {
                fitLinesToLength = value;
                RenderAll();
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
            RenderAll();
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

        public void LogRange(IEnumerable<Log> collection)
        {
            foreach (var log in collection)
                LogRaw(log);
            RenderAll();
        }

        public void Log(Log log)
        {
            LogRaw(log);
            RenderAll();
        }

        public void Log(string message, ColorSet? colorSet = null)
        {
            Log(new Log(message, colorSet));
        }

        public void Log(string message, SCEColor fgColor, SCEColor bgColor)
        {
            Log(new Log(message, new ColorSet(fgColor, bgColor)));
        }

        private void LogRaw(Log log)
        {
            _logs.Add(log);
            if (FollowNewest && (AlwaysFollow || Count - viewY > Height))
                viewY++;
        }

        #endregion

        #region Delete

        public bool Delete(Log log)
        {
            if (!_logs.Remove(log))
                return false;
            RenderAll();
            return true;
        }

        public bool DeleteAt(int index)
        {
            if (index < 0 || index >= _logs.Count)
                return false;
            _logs.RemoveAt(index);
            RenderAll();
            return true;
        }

        public bool DeleteFirst()
        {
            return DeleteAt(0);
        }

        public bool DeleteLast()
        {
            return DeleteAt(_logs.Count - 1);
        }

        public void DeleteRange(int index, int count)
        {
            _logs.RemoveRange(index, count);
            RenderAll();
        }
        
        #endregion

        #region Render

        private void RenderLogAt(int gridY)
        {
            int mappedY = GridTranslate(gridY + ViewTranslate(viewY));
            if (mappedY < 0 || mappedY >= _logs.Count)
            {
                ClearAt(gridY);
            }
            else
            {
                if (!FitToLength)
                    ClearAt(gridY);
                MapLog(gridY, _logs[mappedY]);
            }
        }

        private void RenderAll()
        {
            for (int gridY = 0; gridY < Height; ++gridY)
            {
                RenderLogAt(gridY);
            }
        }

        private void MapLog(int y, Log log)
        {
            _dpMap.MapString(GetLogString(log), y, log.Colors);
        }

        private string GetLogString(Log log)
        {
            string str = DisplayMode switch
            {
                DisplayType.Full => log.ToString(),
                DisplayType.MessageOnly => log.Message,
                _ => throw new NotImplementedException()
            };
            return FitToLength ? Utils.FTL(str, Width) : str; 
        }

        private void ClearAt(int y)
        {
            _dpMap.Fill(new Pixel(BgColor), Rect2DInt.Horizontal(y, _dpMap.Width));
        }

        #endregion

        #region Clear

        public void Clear()
        {
            _logs.Clear();
            _dpMap.Fill(new Pixel(BgColor));
            viewY = 0;
        }

        #endregion

        #region Resize

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            _dpMap.Fill(new Pixel(BgColor));
            RenderAll();
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        #endregion

        #region Translate

        private int GridTranslate(int y)
        {
            return StackMode == StackType.TopDown ? y : Height - y - 1;
        }

        private int ViewTranslate(int y)
        {
            return StackMode == StackType.TopDown ? y : -y;
        }

        #endregion
    }
}
