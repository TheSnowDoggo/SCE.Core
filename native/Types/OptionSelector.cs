namespace SCECore.Types
{
    using System.Collections;

    public class OptionSelector : IRenderable, IEnumerable<Option>
    {
        private const Color DefaultBgColor = Color.Transparent;

        private const StackMode DefaultMode = StackMode.TopDown;

        private static ColorSet DefaultSelectedColorSet { get; } = new(Color.Black, Color.White);

        private static ColorSet DefaultUnselectedColorSet { get; } = new(Color.White, Color.Black);

        private readonly LineRenderer lineRenderer;

        private readonly Queue<int> updateQueue = new();

        private readonly List<Option> optionList = new();

        private int selected;

        public OptionSelector(Vector2Int dimensions, Color bgColor, StackMode mode = DefaultMode)
        {
            lineRenderer = new(dimensions, bgColor, mode)
            {
                FitLinesToLength = true,
            };
        }

        public OptionSelector(Vector2Int dimensions, StackMode mode = DefaultMode)
            : this(dimensions, DefaultBgColor, mode)
        {
        }

        public int Selected
        {
            get => selected;
            set
            {
                if ((value < 0 || value >= Dimensions.Y) && value != -1)
                {
                    throw new ArgumentOutOfRangeException("Selected is invalid.");
                }

                int previous = selected;

                selected = value;

                if (selected != previous)
                {
                    updateQueue.Enqueue(previous);
                    if (selected != -1)
                        updateQueue.Enqueue(selected);
                }
            }
        }

        public bool AnyOptionSelected { get => Selected != -1; }

        public Option SelectedOption { get => AnyOptionSelected ? optionList[Selected] : throw new Exception("No option selected."); }

        public ColorSet SelectedColorSet { get; set; } = DefaultSelectedColorSet;

        public ColorSet UnselectedColorSet { get; set; } = DefaultUnselectedColorSet;

        public Color BgColor
        {
            get => lineRenderer.BgColor;
            set => lineRenderer.BgColor = value;
        }

        public bool IsActive
        {
            get => lineRenderer.IsActive;
            set => lineRenderer.IsActive = value;
        }

        public int Layer
        {
            get => lineRenderer.Layer;
            set => lineRenderer.Layer = value;
        }

        public Vector2Int Position
        {
            get => lineRenderer.Position;
            set => lineRenderer.Position = value;
        }

        public Vector2Int Dimensions { get => lineRenderer.Dimensions; }

        public int Width { get => lineRenderer.Width; }

        public int Height { get => lineRenderer.Height; }

        public int MaxOptions { get => Height; }

        public Option this[int index]
        {
            get => optionList[index];
            set
            {
                optionList[index] = value;
                updateQueue.Enqueue(index);
            }
        }

        public IEnumerator<Option> GetEnumerator()
        {
            return optionList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Image GetImage()
        {
            Update();
            return lineRenderer.GetImage();
        }

        public void Add(Option option)
        {
            if (optionList.Count == MaxOptions)
            {
                throw new Exception("Max options reached.");
            }

            optionList.Add(option);
            updateQueue.Enqueue(optionList.Count - 1);
        }

        public bool Remove(Option option)
        {
            return RemoveAt(optionList.IndexOf(option));
        }

        public bool RemoveAt(int index)
        {
            if (index >= 0 && index < optionList.Count)
            {
                optionList.RemoveAt(index);

                for (int i = index; i <= optionList.Count; ++i)
                {
                    updateQueue.Enqueue(i);
                }

                if (Selected >= index)
                {
                    CycleSelected(-1);
                }
            }

            return index != -1;
        }

        public void Clear()
        {
            lineRenderer.Clear();
            updateQueue.Clear();
            optionList.Clear();

            Selected = -1;
        }

        public void InvokeSelected()
        {
            SelectedOption.Action?.Invoke();
        }

        public bool TryInvokeSelected()
        {
            if (AnyOptionSelected)
                InvokeSelected();
            return AnyOptionSelected;
        }

        public void CycleSelected(int cycle)
        {
            Selected = optionList.Count > 0 ? SCEMath.Mod((Selected == -1 ? 0 : Selected) + cycle, optionList.Count) : -1;
        }

        private void Update()
        {
            foreach (int i in updateQueue)
            {
                Line line;

                if (i >= optionList.Count)
                    line = new("", Color.White, BgColor);
                else if (i == Selected)
                    line = new(optionList[i].Name, SelectedColorSet.FgColor, SelectedColorSet.BgColor);
                else
                    line = new(optionList[i].Name, UnselectedColorSet.FgColor, UnselectedColorSet.BgColor);

                lineRenderer[i] = line;
            }

            updateQueue.Clear();
        }
    }
}
