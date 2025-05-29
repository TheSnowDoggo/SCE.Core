using CSUtils;
namespace SCE
{
    public class VerticalSelector : UIBaseExt
    {
        private Selection[] selections;

        private bool renderQueued = true;

        private int selected = 0;

        private ColorSet selectedColors = new(SCEColor.Black, SCEColor.Gray);

        private ColorSet unselectedColors = new(SCEColor.Gray, SCEColor.Black);

        private Anchor selectionAnchor = Anchor.None;

        private Pixel basePixel = new(SCEColor.Black);

        private StackType stackMode = StackType.TopDown;

        private bool allowNullSelection = false;

        private bool fitToLength = false;

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
                selections[index] = value;
                renderQueued = true;
            }
        }

        public int Selected
        {
            get => selected;
            set
            {
                selected = value;
                renderQueued = true;
            }
        }

        public ColorSet SelectedColors
        {
            get => selectedColors;
            set
            {
                selectedColors = value;
                renderQueued = true;
            }
        }

        public ColorSet UnselectedColors
        {
            get => unselectedColors;
            set
            {
                unselectedColors = value;
                renderQueued = true;
            }
        }

        public Anchor SelectionAnchor
        {
            get => selectionAnchor;
            set
            {
                selectionAnchor = value;
                renderQueued = true;
            }
        }

        public Pixel BasePixel
        {
            get => basePixel;
            set
            {
                basePixel = value;
                renderQueued = true;
            }
        }

        public StackType StackMode
        {
            get => stackMode;
            set
            {
                stackMode = value;
                renderQueued = true;
            }
        }

        public bool AllowNullSelection
        {
            get => allowNullSelection;
            set
            {
                allowNullSelection = value;
                renderQueued = true;
            }
        }

        public bool FitToLength
        {
            get => fitToLength;
            set
            {
                fitToLength = value;
                renderQueued = true;
            }
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
            renderQueued = true;
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        public override DisplayMapView GetMapView()
        {
            if (renderQueued)
            {
                for (int y = 0; y < Height; ++y)
                {
                    var mapY = StackMode == StackType.TopDown ? y : Height - y - 1;

                    if (selections[y] is Selection s)
                    {
                        ColorSet colors;
                        if (selected == y)
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
                            
                            _dpMap.MapString(text, new Vector2Int(0, mapY), colors);
                        }
                        else
                        {
                            var text = Utils.Shorten(s.Text, Width);
                            var x = AnchorUtils.HorizontalFix(anchor, Width - text.Length);

                            _dpMap.Fill(BasePixel, Rect2DInt.Horizontal(mapY, Width));
                            _dpMap.MapString(text, new Vector2Int(x, mapY), colors);
                        }
                    }
                    else
                    {
                        Pixel p = !AllowNullSelection ? BasePixel : new(selected == y ? SelectedColors : UnselectedColors);

                        _dpMap.Fill(p, Rect2DInt.Horizontal(mapY, Width));
                    }
                }

                renderQueued = false;
            }

            return (DisplayMapView)_dpMap;
        }
    }
}
