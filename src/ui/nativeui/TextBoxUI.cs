using CSUtils;
using System.Text;

namespace SCE
{
    public class TextBoxUI : UIBaseExt
    {
        private bool renderQueued = true;

        private Pixel basePixel = new(SCEColor.Black);

        private string text = string.Empty;

        private SCEColor textFgColor = SCEColor.Gray;

        private SCEColor textBgColor = SCEColor.Transparent;

        private Anchor textAnchor = Anchor.None;

        private bool newlineOverflow = false;

        public TextBoxUI(int width, int height)
            : base(width, height)
        {
        }

        public TextBoxUI(Vector2Int dimensions)
            : this(dimensions.X, dimensions.Y)
        {
        }

        #region Settings   

        public Pixel BasePixel
        {
            get => basePixel;
            set
            {
                basePixel = value;
                renderQueued = true;
            }
        }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                renderQueued = true;
            }
        } 

        public SCEColor TextFgColor
        {
            get => textFgColor;
            set
            {
                textFgColor = value;
                renderQueued = true;
            }
        }

        public SCEColor TextBgColor
        {
            get => textBgColor;
            set
            {
                textBgColor = value;
                renderQueued = true;
            }
        }

        public Anchor TextAnchor
        {
            get => textAnchor;
            set
            {
                textAnchor = value;
                renderQueued = true;
            }
        }

        public bool NewlineOverflow
        {
            get => newlineOverflow;
            set
            {
                newlineOverflow = value;
                renderQueued = true;
            }
        }

        #endregion

        private static string[] OverflowSplitLines(string str, int maxLength, int maxLines)
        {
            List<string> lineList = new();
            StringBuilder sb = new();
            for (int i = 0; i < str.Length && lineList.Count < maxLines; ++i)
            {
                bool newLine = str[i] == '\n';
                if (!newLine)
                {
                    sb.Append(str[i]);
                }
                if (newLine || i == str.Length - 1 || (sb.Length == maxLength && str[i + 1] != '\n'))
                {
                    lineList.Add(sb.ToString());
                    sb.Clear();
                }
            }
            return lineList.ToArray();
        }

        private static string[] SplitLines(string str, int maxLength, int maxLines)
        {
            List<string> lines = new(maxLines);
            StringBuilder sb = new();
            foreach (char c in str)
            {
                if (c == '\n')
                {
                    lines.Add(Utils.Shorten(sb.ToString(), maxLength));
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            if (sb.Length > 0 && lines.Count < maxLines)
            {
                lines.Add(Utils.Shorten(sb.ToString(), maxLength));
            }
            return lines.ToArray();
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

        public override DisplayMapView GetMapView()
        {
            if (renderQueued)
            {
                _dpMap.Fill(BasePixel);

                var lineArr = NewlineOverflow ? OverflowSplitLines(Text, Width, Height) : SplitLines(Text, Width, Height);

                int startY = AnchorUtils.VerticalFix(TextAnchor, Height - lineArr.Length);
                for (int i = 0; i < lineArr.Length; ++i)
                {
                    int x = AnchorUtils.HorizontalFix(TextAnchor, Width - lineArr[i].Length);
                    _dpMap.MapString(lineArr[i], new Vector2Int(x, startY + i), TextFgColor, TextBgColor);
                }

                renderQueued = false;
            }
            return (DisplayMapView)_dpMap;
        }
    }
}