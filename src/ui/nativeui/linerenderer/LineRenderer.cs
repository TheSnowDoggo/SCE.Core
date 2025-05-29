using CSUtils;

namespace SCE
{
    public class LineRenderer : UIBaseExt
    {
        private TextLine[] lineArr;

        private bool renderQueued = true;

        private Anchor lineAnchor = Anchor.None;

        private Pixel basePixel = new(SCEColor.Black);

        private StackType stackMode = StackType.TopDown;

        private bool fitToLength = false;

        private SCEColor textFgColor = SCEColor.Gray;

        private SCEColor textBgColor = SCEColor.Black;

        public LineRenderer(int width, int height)
            : base(width, height)
        {
            lineArr = new TextLine[height];
        }

        public LineRenderer(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        public TextLine this[int index]
        {
            get => lineArr[index];
            set
            {
                lineArr[index] = value;
                renderQueued = true;
            }
        }

        public Anchor LineAnchor
        {
            get => lineAnchor;
            set
            {
                lineAnchor = value;
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

        public bool FitToLength
        {
            get => fitToLength;
            set
            {
                fitToLength = value;
                renderQueued = true;
            }
        }

        public SCEColor LineFgColor
        {
            get => textFgColor;
            set
            {
                textFgColor = value;
                renderQueued = true;
            }
        }

        public SCEColor LineBgColor
        {
            get => textBgColor;
            set
            {
                textBgColor = value;
                renderQueued = true;
            }
        }

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            Array.Resize(ref lineArr, height);
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
                for (int i = 0; i < Height; ++i)
                {
                    var y = StackMode == StackType.TopDown ? i : Height - i - 1;

                    if (lineArr[i] is TextLine tl)
                    {
                        var fg = tl.FgColor ?? textFgColor;

                        var bg = tl.BgColor ?? textBgColor;

                        var anchor = tl.Anchor ?? LineAnchor;

                        var ftl = tl.FitToLength ?? FitToLength;

                        if (ftl)
                        {
                            var text = Utils.FTL(tl.Text, Width, ' ', AnchorUtils.ToFillType(anchor));

                            _dpMap.MapString(text, new Vector2Int(0, y), fg, bg);
                        }
                        else
                        {
                            var text = Utils.Shorten(tl.Text, Width);
                            var x = AnchorUtils.HorizontalFix(anchor, Width - text.Length);

                            _dpMap.Fill(BasePixel, Rect2DInt.Horizontal(y, Width));
                            _dpMap.MapString(text, new Vector2Int(x, y), fg, bg);
                        }
                    }
                    else
                    {
                        _dpMap.Fill(BasePixel, Rect2DInt.Horizontal(y, Width));
                    }
                }

                renderQueued = false;
            }

            return (DisplayMapView)_dpMap;
        }
    }
}
