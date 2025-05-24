using System.Collections;

namespace SCE
{
    public class OptionSelector : UIBase, IEnumerable<Option>
    {
        private readonly LineRenderer _lineRenderer;

        private readonly List<Option> _commands = new();

        public OptionSelector(int width, int height, SCEColor? bgColor = null)
        {
            _lineRenderer = new(width, height, bgColor);
        }

        public OptionSelector(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        #region IEnumerable

        public IEnumerator<Option> GetEnumerator()
        {
            return _commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public Option this[int y]
        {
            get => _commands[y];
            set
            {
                _commands[y] = value;
                Update();
            }
        }

        public int Width { get => _lineRenderer.Width; }

        public int Height { get => _lineRenderer.Height; }

        public Vector2Int Dimensions { get => _lineRenderer.Dimensions; }

        #region Settings

        private int selected = 0;

        public int Selected
        {
            get => selected;
            set
            {
                if (_commands.Count == 0 || selected == value)
                    return;
                selected = MathUtils.Cycle(new Vector2Int(0, _commands.Count), value);
                Update();
            }
        }

        private int scrollOffset = 2;

        public int ScrollOffset
        {
            get => scrollOffset;
            set
            {
                if (value >= Height)
                    return;
                scrollOffset = value;
                Update();
            }
        }

        private ColorSet selectedColors = new(SCEColor.Black, SCEColor.White);

        public ColorSet SelectedColors
        {
            get => selectedColors;
            set
            {
                selectedColors = value;
                Update();
            }
        }

        public bool FitToLength
        {
            get => _lineRenderer.FitToLength;
            set
            {
                _lineRenderer.FitToLength = value;
                Update();
            }
        }

        public StackType StackMode
        {
            get => _lineRenderer.StackMode;
            set
            {
                _lineRenderer.StackMode = value;
                Update();
            }
        }

        #endregion

        public void Add(Option command)
        {
            _commands.Add(command);
            Update();
        }

        public void AddRange(IEnumerable<Option> collection)
        {
            _commands.AddRange(collection);
            Update();
        }

        public void AddEvery(params Option[] commandArr)
        {
            AddRange(commandArr);
        }

        public bool Remove(Option command)
        {
            if (_commands.Remove(command))
                return false;
            Update();
            return true;
        }

        public void RemoveAt(int index)
        {
            _commands.RemoveAt(index);
            Update();
        }

        public void RemoveRange(int index, int count)
        {
            _commands.RemoveRange(index, count);
            Update();
        }

        public void Clear()
        {
            _commands.Clear();
            _lineRenderer.Clear();
        }

        public void Update()
        {
            int dif = selected + ScrollOffset - _commands.Count;
            int scrollOffset = dif < 0 ? ScrollOffset : ScrollOffset - dif - 1;
            bool mapY = _commands.Count > Height && selected >= Height - scrollOffset;
            for (int y = 0; y < Height; ++y)
            {
                int i = mapY ? selected - (Height - scrollOffset) + y + 1 : y;
                if (i >= 0 && i < _commands.Count)
                    _lineRenderer.SetLine(y, new Line(_commands[i].Name, i == selected ? SelectedColors : _commands[i].Colors) { Anchor = _commands[i].Anchor });
                else
                    _lineRenderer.ClearLine(y);
            }
        }

        public bool TryRunSelected()
        {
            if (_commands.Count == 0)
                return false;
            return _commands[selected].TryRun();
        }

        /// <inheritdoc/>
        public override DisplayMap GetMap()
        {
            return _lineRenderer.GetMap();
        }
    }
}
