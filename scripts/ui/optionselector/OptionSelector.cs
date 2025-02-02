using System.Collections;

namespace SCE
{
    public class OptionSelector : UIBase, IEnumerable<Option>
    {
        private const string DEFAULT_NAME = "command_selector";

        private readonly LineRenderer lineRenderer;

        private readonly List<Option> commands = new();

        #region Constructors

        public OptionSelector(string name, int width, int height, SCEColor? bgColor = null)
            : base(name)
        {
            lineRenderer = new(width, height, bgColor);
        }

        public OptionSelector(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public OptionSelector(int width, int height, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public OptionSelector(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, dimensions.X, dimensions.Y, bgColor)
        {
        }

        #endregion

        #region Indexers

        public Option this[int y]
        {
            get => commands[y];
            set => SetThis(y, value);
        }

        private void SetThis(int y, Option value)
        {
            commands[y] = value;
            RenderOptions();
        }

        #endregion

        #region Settings

        private int selected = 0;

        public int Selected
        {
            get => selected;
            set => SetSelected(value);
        }

        private void SetSelected(int value)
        {
            if (commands.Count == 0 || selected == value)
                return;
            selected = MathUtils.Cycle(new Vector2Int(0, commands.Count), value);
            RenderOptions();
        }

        private int scrollOffset = 2;

        public int ScrollOffset
        {
            get => scrollOffset;
            set => SetScrollOffset(value);
        }

        private void SetScrollOffset(int value)
        {
            if (value >= Height)
                return;
            scrollOffset = value;
            RenderOptions();
        }

        private ColorSet selectedColors = new(SCEColor.Black, SCEColor.White);

        public ColorSet SelectedColors
        {
            get => selectedColors;
            set => SetSelectedColors(value);
        }

        private void SetSelectedColors(ColorSet value)
        {
            selectedColors = value;
            RenderOptions();
        }

        public bool FitToLength
        {
            get => lineRenderer.FitToLength;
            set => SetFitToLength(value);
        }

        private void SetFitToLength(bool value)
        {
            lineRenderer.FitToLength = value;
            RenderOptions();
        }

        public StackType StackMode
        {
            get => lineRenderer.StackMode;
            set => SetStackMode(value);
        }

        private void SetStackMode(StackType value)
        {
            lineRenderer.StackMode = value;
            RenderOptions();
        }

        #endregion

        #region Properties

        public int Width { get => lineRenderer.Width; }

        public int Height { get => lineRenderer.Height; }

        public Vector2Int Dimensions { get => lineRenderer.Dimensions; }

        #endregion

        #region IEnumerable

        public IEnumerator<Option> GetEnumerator()
        {
            return commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region List

        public void Add(Option command)
        {
            commands.Add(command);
            RenderOptions();
        }

        public void AddRange(IEnumerable<Option> collection)
        {
            commands.AddRange(collection);
            RenderOptions();
        }

        public void AddEvery(params Option[] commandArr)
        {
            AddRange(commandArr);
        }

        public bool Remove(Option command)
        {
            if (commands.Remove(command))
                return false;
            RenderOptions();
            return true;
        }

        public void RemoveAt(int index)
        {
            commands.RemoveAt(index);
            RenderOptions();
        }

        public void RemoveRange(int index, int count)
        {
            commands.RemoveRange(index, count);
            RenderOptions();
        }

        public void Clear()
        {
            commands.Clear();
            lineRenderer.Clear();
        }

        #endregion

        public void RenderOptions()
        {
            int dif = selected + ScrollOffset - commands.Count;
            int scrollOffset = dif < 0 ? ScrollOffset : ScrollOffset - dif - 1;
            bool mapY = commands.Count > Height && selected >= Height - scrollOffset;
            for (int y = 0; y < Height; ++y)
            {
                int i = mapY ? selected - (Height - scrollOffset) + y + 1 : y;
                if (i >= 0 && i < commands.Count)
                    lineRenderer.SetLine(y, new Line(commands[i].Name, i == selected ? SelectedColors : commands[i].Colors) { Anchor = commands[i].Anchor });
                else
                    lineRenderer.ClearLine(y);
            }
        }

        public bool TryRunSelected()
        {
            if (commands.Count == 0)
                return false;
            return commands[selected].TryRun();
        }

        public override DisplayMap GetMap()
        {
            return lineRenderer.GetMap();
        }
    }
}
