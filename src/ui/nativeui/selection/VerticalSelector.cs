using CSUtils;
namespace SCE
{
    public class VerticalSelector : UIBaseExt
    {
        private readonly HashSet<int> _updates = new();

        private Selection[] selections;

        private bool renderQueued = true; 

        public VerticalSelector(int width, int height)
            : base(width, height)
        {
            selections = new Selection[height];
        }

        public VerticalSelector(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        public Selection this[int index]
        {
            get => selections[index];
            set
            {
                if (MiscUtils.QueueSet(ref selections[index], value))
                {
                    _updates.Add(index);
                }
            }
        }

        private int selected = 0;

        public int Selected
        {
            get => selected;
            set => MiscUtils.QueueSet(ref selected, value, ref renderQueued);
        }

        private ColorSet selectedColors = new(SCEColor.Black, SCEColor.Gray);

        public ColorSet SelectedColors
        {
            get => selectedColors;
            set => MiscUtils.QueueSet(ref selectedColors, value, ref renderQueued);
        }

        private ColorSet unselectedColors = new(SCEColor.Gray, SCEColor.Black);

        public ColorSet UnselectedColors
        {
            get => unselectedColors;
            set => MiscUtils.QueueSet(ref unselectedColors, value, ref renderQueued);
        }

        private Anchor selectionAnchor = Anchor.None;

        public Anchor SelectionAnchor
        {
            get => selectionAnchor;
            set => MiscUtils.QueueSet(ref selectionAnchor, value, ref renderQueued);
        }

        private Pixel basePixel = new (SCEColor.Black);

        public Pixel BasePixel
        {
            get => basePixel;
            set => MiscUtils.QueueSet(ref basePixel, value, ref renderQueued);
        }

        private StackType stackMode = StackType.TopDown;

        public StackType StackMode
        {
            get => stackMode;
            set => MiscUtils.QueueSet(ref stackMode, value, ref renderQueued);
        }

        private bool allowNullSelection = false;

        public bool AllowNullSelection
        {
            get => allowNullSelection;
            set => MiscUtils.QueueSet(ref allowNullSelection, value, ref renderQueued);
        }

        private bool fitToLength = false;

        public bool FitToLength
        {
            get => fitToLength;
            set => MiscUtils.QueueSet(ref fitToLength, value, ref renderQueued);
        }

        public void MoveSelected(int move)
        {
            Selected = Utils.Mod(Selected + move, selections.Length);
        }

        public bool RunSelected()
        {
            if (Selected < 0 || Selected >= selections.Length || this[Selected] == null)
            {
                return false;
            }
            this[Selected].OnSelect?.Invoke();
            return true;
        }

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            Array.Resize(ref selections, height);
            _updates.Clear();
            renderQueued = true;
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        public override MapView<Pixel> GetMapView()
        {
            if (renderQueued || _updates.Count > 0)
            {
                if (renderQueued)
                {
                    Render(Enumerable.Range(0, Height));

                    renderQueued = false;
                }
                else
                {
                    Render(_updates);
                }

                _updates.Clear();
            }

            return _dpMap;
        }

        private void Render(IEnumerable<int> range)
        {
            foreach (var i in range)
            {
                var y = StackMode == StackType.TopDown ? i : Height - i - 1;

                if (selections[i] is Selection s)
                {
                    ColorSet colors;
                    if (selected == i)
                    {
                        s.OnHover?.Invoke();
                        colors = s.SelectedColors ?? SelectedColors;
                    }
                    else
                    {
                        colors = s.UnselectedColors ?? UnselectedColors;
                    }

                    var anchor = s.Anchor ?? SelectionAnchor;

                    var ftl = s.FitToLength ?? FitToLength;

                    if (ftl)
                    {
                        var text = Utils.FTL(s.Text, Width, ' ', AnchorUtils.ToFillType(anchor));

                        _dpMap.MapString(text, new Vector2Int(0, y), colors);
                    }
                    else
                    {
                        var text = Utils.Shorten(s.Text, Width);
                        var x = AnchorUtils.HorizontalFix(anchor, Width - text.Length);

                        _dpMap.Fill(BasePixel, Rect2DInt.Horizontal(y, Width));
                        _dpMap.MapString(text, new Vector2Int(x, y), colors);
                    }
                }
                else
                {
                    Pixel p = !AllowNullSelection ? BasePixel : new(selected == i ? SelectedColors : UnselectedColors);

                    _dpMap.Fill(p, Rect2DInt.Horizontal(y, Width));
                }
            }
        }
    }
}
