using CSUtils;
using System.Text;

namespace SCE
{
    public class TextBoxUI : UIBaseExt
    {
        private bool renderQueued = true;

        public TextBoxUI(int width, int height)
            : base(width, height)
        {
        }

        public TextBoxUI(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        private Pixel basePixel = new(SCEColor.Black);

        public Pixel BasePixel
        {
            get => basePixel;
            set => MiscUtils.QueueSet(ref basePixel, value, ref renderQueued);
        }

        private string text = string.Empty;

        public string Text
        {
            get => text;
            set => MiscUtils.QueueSet(ref text, value, ref renderQueued);
        }

        private SCEColor textFgColor = SCEColor.Gray;

        public SCEColor TextFgColor
        {
            get => textFgColor;
            set => MiscUtils.QueueSet(ref textFgColor, value, ref renderQueued);
        }

        private SCEColor textBgColor = SCEColor.Transparent;

        public SCEColor TextBgColor
        {
            get => textBgColor;
            set => MiscUtils.QueueSet(ref textBgColor, value, ref renderQueued);
        }

        private Anchor textAnchor = Anchor.None;

        public Anchor TextAnchor
        {
            get => textAnchor;
            set => MiscUtils.QueueSet(ref textAnchor, value, ref renderQueued);
        }

        private bool newlineOverflow = false;

        public bool NewlineOverflow
        {
            get => newlineOverflow;
            set => MiscUtils.QueueSet(ref newlineOverflow, value, ref renderQueued);
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

        public override MapView<Pixel> GetMapView()
        {
            if (renderQueued)
            {
                _dpMap.Fill(BasePixel);

                string[] lines;
                if (NewlineOverflow)
                {
                    lines = Utils.OverflowSplitLines(Text, Width, Height);
                }
                else
                {
                    lines = Utils.SplitLines(Text, Width, Height); 
                }

                int startY = AnchorUtils.VerticalFix(TextAnchor, Height - lines.Length);

                for (int i = 0; i < lines.Length; ++i)
                {
                    int x = AnchorUtils.HorizontalFix(TextAnchor, Width - lines[i].Length);

                    _dpMap.MapString(lines[i], new Vector2Int(x, startY + i), TextFgColor, TextBgColor);
                }

                renderQueued = false;
            }
            return _dpMap;
        }
    }
}