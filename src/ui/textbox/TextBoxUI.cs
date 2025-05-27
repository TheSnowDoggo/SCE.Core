using CSUtils;
using System.Text;

namespace SCE
{
    public class TextBoxUI : UIBaseExt
    {
        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        public TextBoxUI(int width, int height, SCEColor? bgColor = null)
            : base(width, height, bgColor)
        {
            this.bgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public TextBoxUI(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

        #region Settings

        private SCEColor bgColor;

        public SCEColor BgColor
        {
            get => bgColor;
            set
            {
                bgColor = value;
                Update();
            }
        }

        private string text = string.Empty;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                Update();
            }
        }

        private SCEColor textFgColor = SCEColor.Gray;

        public SCEColor TextFgColor
        {
            get => textFgColor;
            set
            {
                textFgColor = value;
                Update();
            }
        }

        private SCEColor textBgColor = SCEColor.Transparent;

        public SCEColor TextBgColor
        {
            get => textBgColor;
            set
            {
                textBgColor = value;
                Update();
            }
        }

        private Anchor textAnchor;

        public Anchor TextAnchor
        {
            get => textAnchor;
            set
            {
                textAnchor = value;
                Update();
            }
        }

        public bool NewlineOverflow { get; set; } = false;

        #endregion

        private static string[] OverflowSplitLines(string str, int maxLength, int maxLines)
        {
            List<string> lineList = new();
            StringBuilder sb = new();
            for (int i = 0; i < str.Length && lineList.Count < maxLines; ++i)
            {
                bool newLine = str[i] == '\n';
                if (!newLine)
                    sb.Append(str[i]);
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
                lines.Add(Utils.Shorten(sb.ToString(), maxLength));
            return lines.ToArray();
        }

        public void Update()
        {
            _dpMap.Fill(new Pixel(BgColor));

            var lineArr = NewlineOverflow ? OverflowSplitLines(Text, Width, Height) :
                SplitLines(Text, Width, Height);

            int startY = AnchorUtils.VerticalFix(Anchor, Height - lineArr.Length);
            for (int i = 0; i < lineArr.Length; ++i)
            {
                int x = AnchorUtils.HorizontalFix(TextAnchor, Width - lineArr[i].Length);
                _dpMap.MapString(lineArr[i], new Vector2Int(x, startY + i), TextFgColor, TextBgColor);
            }
        }

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            Update();
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }
    }
}