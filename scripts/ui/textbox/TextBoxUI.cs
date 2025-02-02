namespace SCE
{
    /// <summary>
    /// Represents a text box UI element.
    /// </summary>
    public class TextBoxUI : Image, IRenderable
    {
        private const string DEFAULT_NAME = "textbox";

        private readonly List<Rect2D> renderedAreaList = new();

        private SCEColor bgColor = SCEColor.Black;

        private Text? text = null;

        private Text? renderedText = null;

        private TextBoxUI? renderedTextBoxUI;

        private bool forceRender = true;

        public TextBoxUI(string name, int width, int height)
            : base(name, width, height)
        {
            OnResize += TextBoxUI_OnResize;
        }

        public TextBoxUI(string name, Vector2Int dimensions)
            : this(name, dimensions.X, dimensions.Y)
        {
        }

        public TextBoxUI(int width, int height)
            : this(DEFAULT_NAME, width, height)
        {
        }

        public TextBoxUI(Vector2Int dimensions)
            : this(DEFAULT_NAME, dimensions)
        {
        }

        public TextBoxUI(string name, DisplayMap displayMap)
            : base(name, displayMap)
        {
            OnResize += TextBoxUI_OnResize;
        }

        public TextBoxUI(DisplayMap displayMap)
            : this(DEFAULT_NAME, displayMap)
        {
        }

        /// <summary>
        /// Gets or sets the text to be rendered.
        /// </summary>
        public Text Text
        {
            get => text ??= new();
            set => text = value;
        }

        public SCEColor BgColor
        {
            get => bgColor;
            set
            {
                bgColor = value;
                FillBackground();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text box only updates the render image when the text has been modified.
        /// </summary>
        /// <remarks>
        /// Recommended to be kept on for best performance.
        /// </remarks>
        public bool TextCaching { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the text box only clears the render image where text has been previously rendered.
        /// </summary>
        /// <remarks>
        /// <i>Note: Background images cannot be used while enabled, only plain background colors.</i>
        /// </remarks>
        public bool BasicTextBoxRendering { get; set; } = true;

        public override DisplayMap GetMap()
        {
            OnRender?.Invoke();

            if (!TextCaching || forceRender || renderedText is null || Text != renderedText)
            {
                Render();
                forceRender = false;
            }

            return renderedTextBoxUI ?? this;
        }

        public override TextBoxUI Clone()
        {
            TextBoxUI clone = new(base.Clone())
            {
                Text = (Text)Text.Clone(),
                IsActive = IsActive,
                TextCaching = TextCaching,
                BasicTextBoxRendering = BasicTextBoxRendering,
            };

            return clone;
        }

        public void ForceNextRender()
        {
            forceRender = true;
        }

        public void ClearData()
        {
            OnRender = null;
            Text = new();
            IsActive = false;
        }

        private TextBoxUI RenderClone()
        {
            TextBoxUI clone = new(base.Clone())
            {
                Text = (Text)Text.Clone(),
                TextCaching = false,
                BasicTextBoxRendering = false,
            };

            return clone;
        }

        private void Render()
        {
            if (BasicTextBoxRendering)
            {
                SmartClear();
                renderedText = (Text)Text.Clone();
                MapText(renderedText, true);
            }
            else
            {
                renderedTextBoxUI = RenderClone();
                renderedText = renderedTextBoxUI.Text;
                renderedTextBoxUI.MapText(renderedText, false);
            }
        }

        #region Mapping
        private void RecordArea(Vector2Int position, string line)
        {
            if (line.Length > 0)
            {
                Rect2D area = new(position, position + new Vector2Int(line.Length, 1));

                renderedAreaList.Add(area);
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> array of every line the <see cref="Text.Data"/> of the given <paramref name="text"/> can be split into.
        /// </summary>
        /// <param name="text">The <see cref="Text"/> to split up its <see cref="Text.Data"/>.</param>
        /// <returns>A <see cref="string"/> array of every line the <see cref="Text.Data"/> of the given <paramref name="text"/> can be split into.</returns>
        public string[] GetSplitLineArray(Text text)
        {
            if (text.NewLineOverflow)
                return StringUtils.SmartSplitLineArray(text.Data, Width, Height);
            else
                return StringUtils.BasicSplitLineArray(text.Data, Height);
        }

        private void MapText(Text text, bool recordArea)
        {
            if (text.Data == string.Empty)
                return;

            string[] lineArray = GetSplitLineArray(text);

            int i = 0, topY = Height - 1, startY = text.GetStartMapY(lineArray.Length, Height);
            do
            {
                string line = text.GetFormattedBody(lineArray[i]);

                Vector2Int position = new(text.GetStartMapX(line.Length, Width), topY - (startY + i));

                MapString(position, line, text.FgColor, text.BgColor);

                if (recordArea && BasicTextBoxRendering)
                    RecordArea(position, line);

                ++i;
            }
            while (i < lineArray.Length && i < Height);
        }
        #endregion

        /// <summary>
        /// Clears old text when basic text box rendering is enable.
        /// </summary>
        private void SmartClear()
        {
            foreach (var area in renderedAreaList)
                Data.FillArea(new Pixel(BgColor), area);
            renderedAreaList.Clear();
        }

        private void FillBackground()
        {
            Data.Fill(new Pixel(BgColor));
        }

        private void TextBoxUI_OnResize()
        {
            if (BasicTextBoxRendering)
            {
                renderedAreaList.Clear();

                FillBackground();
            }
            forceRender = true;
        }
    }
}
