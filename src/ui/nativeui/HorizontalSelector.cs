using CSUtils;

namespace SCE
{
    public class HorizontalSelector : UIBaseExt
    {
        private readonly List<Selection> _selections = new();

        private bool renderQueued = true;

        public HorizontalSelector(int width, int height)
            : base(width, height)
        {
        }

        public HorizontalSelector(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        public IList<Selection> Icons { get => _selections.AsReadOnly(); }

        private Pixel basePixel = new(SCEColor.Black);

        public Pixel BasePixel
        {
            get => basePixel;
            set => MiscUtils.QueueSet(ref basePixel, value, ref renderQueued);
        }

        public int selected = 0;

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

        private SlideType slideMode = SlideType.LeftRight;

        public SlideType SlideMode
        {
            get => slideMode;
            set => MiscUtils.QueueSet(ref slideMode, value, ref renderQueued);
        }

        private bool fitToLength = true;

        public bool FitToLength
        {
            get => fitToLength;
            set => MiscUtils.QueueSet(ref fitToLength, value, ref renderQueued);
        }

        public void Add(Selection selection)
        {
            _selections.Add(selection);
            renderQueued = true;
        }

        public void AddEvery(params Selection[] selections)
        {
            foreach (var sel in selections)
            {
                Add(sel);
            }
        }

        public bool Remove(Selection selection)
        {
            var res = _selections.Remove(selection);
            if (res)
            {
                renderQueued = true;
            }
            return res;
        }

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            renderQueued = true;
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        public void CycleSelected(int move)
        {
            Selected = Utils.Mod(Selected + move, _selections.Count);
        }

        public bool RunSelected()
        {
            if (Selected < 0 || Selected > _selections.Count)
            {
                return false;
            }
            var sel = _selections[Selected];
            if (sel == null || sel.OnSelect == null)
            {
                return false;
            }
            sel.OnSelect.Invoke();
            return true;
        }

        public override MapView<Pixel> GetMapView()
        {
            if (renderQueued)
            {
                bool lr = SlideMode == SlideType.LeftRight;

                int x = 0;
                for (int i = 0; i < _selections.Count && x < Width; ++i)
                {
                    var sel = _selections[i];

                    var lines = Utils.SplitLines(sel.Text, Width - x, Height);

                    var width = lines.Max(s => s.Length);

                    var anchor = sel.Anchor ?? SelectionAnchor;

                    ColorSet colors;
                    if (i == Selected)
                    {
                        colors = sel.SelectedColors ?? SelectedColors;
                        sel.OnHover?.Invoke();
                    }
                    else
                    {
                        colors = sel.UnselectedColors ?? UnselectedColors;
                    }

                    var ftl = sel.FitToLength ?? FitToLength;

                    int nameY = AnchorUtils.VerticalFix(anchor, Height - lines.Length);

                    int mapX = lr ? x : Width - x - width;

                    for (int j = 0; j < lines.Length; ++j)
                    {
                        if (ftl)
                        {
                            var str = Utils.FTL(lines[j], width, ' ', AnchorUtils.ToFillType(anchor));

                            _dpMap.MapString(str, new Vector2Int(mapX, nameY + j), colors);
                        }
                        else
                        {
                            int nameX = AnchorUtils.HorizontalFix(anchor, width - lines[j].Length);

                            _dpMap.MapString(lines[j], new Vector2Int(mapX + nameX, nameY + j), colors);
                        }
                    }

                    x += width;
                }

                renderQueued = false;
            }

            return _dpMap;
        }
    }
}