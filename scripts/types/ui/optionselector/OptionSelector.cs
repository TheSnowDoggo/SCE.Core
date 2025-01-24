namespace SCE
{
    using System.Collections;

    public class OptionSelector : UIBase, IEnumerable<Option>
    {
        private const string DEFAULT_NAME = "option_selector";

        private const Color DEFAULT_BGCOLOR = Color.Transparent;

        private const StackType DefaultMode = StackType.TopDown;

        private readonly LineRenderer lineRenderer;

        private readonly Queue<int> _updateQueue = new();

        private readonly List<Option> _optionList = new();

        public event EventHandler<OptionSelectorInvokeEventArgs>? OnInvokeEvent;
        public event EventHandler<OptionSelectorInvokeEventArgs>? PostInvokeEvent;
        public event EventHandler? SelectedModifyEvent;

        private ColorSet selectedColorSet = DefaultSelectedColorSet;
        private ColorSet unselectedColorSet = DefaultUnselectedColorSet;

        private int selected;

        public OptionSelector(string name, int width, int height, Color? bgColor = null)
            : base(name)
        {
            lineRenderer = bgColor is Color color ? new(width, height, color) : new(width, height);
            lineRenderer.FitLinesToLength = true;
        }

        public OptionSelector(string name, Vector2Int dimensions, Color? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public OptionSelector(int width, int height, Color? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public OptionSelector(Vector2Int dimensions, Color? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        private static ColorSet DefaultSelectedColorSet { get; } = new(Color.Black, Color.White);

        private static ColorSet DefaultUnselectedColorSet { get; } = new(Color.White, Color.Black);

        public int Selected
        {
            get => selected;
            set
            {
                if ((value < 0 || value >= Dimensions.Y) && value != -1)
                    throw new ArgumentOutOfRangeException("Selected is invalid.");

                int previous = selected;

                selected = value;

                if (selected != previous)
                {
                    Enqueue(previous);
                    if (selected != -1)
                        Enqueue(selected);
                }

                SelectedModifyEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool AnyOptionSelected { get => Selected != -1; }

        public Option SelectedOption { get => AnyOptionSelected ? _optionList[Selected] : throw new Exception("No option selected."); }

        public int Count { get => _optionList.Count; }

        public ColorSet SelectedColorSet
        {
            get => selectedColorSet;
            set
            {
                selectedColorSet = value;

                if (AnyOptionSelected)
                    Enqueue(Selected);
            }
        }

        public ColorSet UnselectedColorSet
        {
            get => unselectedColorSet;
            set
            {
                unselectedColorSet = value;

                for (int i = 0; i < _optionList.Count; ++i)
                {
                    if (i != Selected)
                        Enqueue(i);
                }
            }
        }

        public Color BgColor
        {
            get => lineRenderer.BgColor;
            set => lineRenderer.BgColor = value;
        }

        public Vector2Int Dimensions { get => lineRenderer.Dimensions; }

        public int Width { get => lineRenderer.Width; }

        public int Height { get => lineRenderer.Height; }

        public int MaxOptions { get => Height; }

        public Option this[int index]
        {
            get => _optionList[index];
            set
            {
                _optionList[index] = value;
                Enqueue(index);
            }
        }

        public IEnumerator<Option> GetEnumerator()
        {
            return _optionList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override DisplayMap GetMap()
        {
            Update();
            return lineRenderer.GetMap();
        }

        public void Add(Option option)
        {
            if (_optionList.Count == MaxOptions)
                throw new Exception("Max options reached.");

            _optionList.Add(option);
            Enqueue(_optionList.Count - 1);
        }

        public void Add(Option[] optionArray)
        {
            if (optionArray.Length + _optionList.Count > MaxOptions)
                throw new ArgumentException("Option array will exceed max options.");

            foreach (Option option in optionArray)
                Add(option);
        }

        public void Add(List<Option> optionList)
        {
            Add(optionList.ToArray());
        }

        public bool Remove(Option option)
        {
            return RemoveAt(_optionList.IndexOf(option));
        }

        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= _optionList.Count)
                return false;

            _optionList.RemoveAt(index);

            for (int i = index; i <= _optionList.Count; ++i)
            {
                Enqueue(i);
            }

            if (Selected >= index)
                CycleSelected(-1);

            return true;
        }

        public void Clear()
        {
            lineRenderer.Clear();
            _updateQueue.Clear();
            _optionList.Clear();

            Selected = -1;
        }

        public void InvokeSelected()
        {
            Option option = SelectedOption;
            OnInvokeEvent?.Invoke(this, new(option));
            SelectedOption.Action?.Invoke();
            PostInvokeEvent?.Invoke(this, new(option));
        }

        public bool TryInvokeSelected()
        {
            if (AnyOptionSelected)
                InvokeSelected();
            return AnyOptionSelected;
        }

        public void CycleSelected(int cycle)
        {
            Selected = _optionList.Count > 0 ? MathUtils.Mod((Selected == -1 ? 0 : Selected) + cycle, _optionList.Count) : -1;
        }

        private void Enqueue(int i)
        {
            if (!_updateQueue.Contains(i))
                _updateQueue.Enqueue(i);
        }

        private void Update()
        {
            foreach (int i in _updateQueue)
            {
                if (i == -1)
                    continue;

                Line line;
                if (i >= _optionList.Count)
                    line = new("", Color.White, BgColor);
                else if (i == Selected)
                    line = new(_optionList[i].Name, SelectedColorSet.FgColor, SelectedColorSet.BgColor);
                else
                    line = new(_optionList[i].Name, UnselectedColorSet.FgColor, UnselectedColorSet.BgColor);

                lineRenderer[i] = line;
            }

            _updateQueue.Clear();
        }
    }
}
