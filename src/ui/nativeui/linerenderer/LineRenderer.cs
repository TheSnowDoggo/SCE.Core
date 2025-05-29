using CSUtils;
using System;
namespace SCE
{
    public class LineRenderer : UIBaseExt
    {
        private readonly HashSet<int> _updates = new();

        private readonly ArrayUpdateView<string?> textArrView;

        private readonly ArrayUpdateView<LineAttributes> attributeArrView;

        private string?[] textArr;

        private LineAttributes[] attributeArr;

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
            textArr = new string?[height];
            attributeArr = new LineAttributes[height];

            textArrView = new(textArr, i => _updates.Add(i));
            attributeArrView = new(attributeArr, i => _updates.Add(i));
        }

        public LineRenderer(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        public ArrayUpdateView<string?> Text { get => textArrView; }

        public ArrayUpdateView<LineAttributes> Attributes { get => attributeArrView; }

        public Anchor LineAnchor
        {
            get => lineAnchor;
            set => MiscUtils.QueueSet(ref lineAnchor, value, ref renderQueued);
        }

        public Pixel BasePixel
        {
            get => basePixel;
            set => MiscUtils.QueueSet(ref basePixel, value, ref renderQueued);
        }

        public StackType StackMode
        {
            get => stackMode;
            set => MiscUtils.QueueSet(ref stackMode, value, ref renderQueued);
        }

        public bool FitToLength
        {
            get => fitToLength;
            set => MiscUtils.QueueSet(ref fitToLength, value, ref renderQueued);
        }

        public SCEColor LineFgColor
        {
            get => textFgColor;
            set => MiscUtils.QueueSet(ref textFgColor, value, ref renderQueued);
        }

        public SCEColor LineBgColor
        {
            get => textBgColor;
            set => MiscUtils.QueueSet(ref textBgColor, value, ref renderQueued);
        }

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            Array.Resize(ref textArr, height);
            Array.Resize(ref attributeArr, height);
            _updates.Clear();
            renderQueued = true;
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        public override DisplayMapView GetMapView()
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

            return (DisplayMapView)_dpMap;
        }

        private void Render(IEnumerable<int> range)
        {
            foreach (var i in range)
            {
                var y = StackMode == StackType.TopDown ? i : Height - i - 1;

                if (textArr[i] is string text)
                {
                    var fg = attributeArr[i].FgColor ?? textFgColor;

                    var bg = attributeArr[i].BgColor ?? textBgColor;

                    var anchor = attributeArr[i].Anchor ?? LineAnchor;

                    var ftl = attributeArr[i].FitToLength ?? FitToLength;

                    if (ftl)
                    {
                        text = Utils.FTL(text, Width, ' ', AnchorUtils.ToFillType(anchor));

                        _dpMap.MapString(text, new Vector2Int(0, y), fg, bg);
                    }
                    else
                    {
                        text = Utils.Shorten(text, Width);
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
        }
    }
}
