namespace SCE
{
    public class TextBoxUI : UIBaseExt
    {
        private const string DEFAULT_NAME = "textbox";

        private const SCEColor DEFAULT_BGCOLOR = SCEColor.Black;

        #region Constructors

        public TextBoxUI(string name, int width, int height, SCEColor? bgColor = null)
            : base(name, width, height, bgColor)
        {
            this.bgColor = bgColor ?? DEFAULT_BGCOLOR;
        }

        public TextBoxUI(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public TextBoxUI(int width, int height, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public TextBoxUI(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        #endregion

        #region Settings

        private SCEColor bgColor;

        public SCEColor BgColor
        {
            get => bgColor;
            set => SetBgColor(value);
        }

        private void SetBgColor(SCEColor value)
        {
            bgColor = value;
            Render();
        }

        #endregion

        #region TextSettings

        private string text = string.Empty;

        public string Text
        {
            get => text;
            set => SetText(value);
        }

        private void SetText(string value)
        {
            text = value;
            Render();
        }

        private SCEColor textFgColor = SCEColor.Gray;

        public SCEColor TextFgColor
        {
            get => textFgColor;
            set => SetTextFgColor(value);
        }

        private void SetTextFgColor(SCEColor value)
        {
            textFgColor = value;
            Render();
        }

        private SCEColor textBgColor = SCEColor.Transparent;

        public SCEColor TextBgColor
        {
            get => textBgColor;
            set => SetTextBgColor(value);
        }

        private void SetTextBgColor(SCEColor value)
        {
            textBgColor = value;
            Render();
        }

        private HorizontalAnchor horizontalAnchor = HorizontalAnchor.Left;

        public HorizontalAnchor HorizontalAnchor
        {
            get => horizontalAnchor;
            set => SetHorizontalAnchor(value);
        }

        private void SetHorizontalAnchor(HorizontalAnchor value)
        {
            horizontalAnchor = value;
            Render();
        }

        private VerticalAnchor verticalAnchor = VerticalAnchor.Top;

        public VerticalAnchor VerticalAnchor
        {
            get => verticalAnchor;
            set => SetVerticalAnchor(value);
        }

        private void SetVerticalAnchor(VerticalAnchor value)
        {
            verticalAnchor = value;
            Render();
        }

        #endregion

        #region Render

        public void Render()
        {
            _dpMap.Data.Fill(new Pixel(BgColor));

            var stringArr = StringUtils.SmartSplitLineArray(Text, Width, Height);

            int startY = AnchorUtils.VerticalAnchoredStart(VerticalAnchor, stringArr.Length, Height) - 1;
            for (int i = 0; i < stringArr.Length; ++i)
            {
                int x = AnchorUtils.HorizontalAnchoredStart(HorizontalAnchor, stringArr[i].Length, Width);
                _dpMap.MapString(new Vector2Int(x, startY - i), stringArr[i], TextFgColor, TextBgColor);
            }
        }

        #endregion

        #region Resize

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            Render();
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        #endregion
    }
}