namespace SCE
{
    /// <summary>
    /// Represents a text box UI element.
    /// </summary>
    public class TextBoxUI : Image, IRenderable
    {
        private const bool DefaultActiveState = true;

        private readonly List<Area2DInt> renderedAreaList = new();

        private Color bgColor = Color.Black;

        private Text? text = null;

        private Text? renderedText = null;

        private TextBoxUI? renderedTextBoxUI;

        private bool forceRender = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxUI"/> class.
        /// </summary>
        /// <param name="dimensions">The dimensions of the text box.</param>
        public TextBoxUI(Vector2Int dimensions)
            : base(dimensions)
        {
            OnResize += TextBoxUI_OnResize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxUI"/> class.
        /// </summary>
        /// <param name="image">The base image of the text box.</param>
        /// <param name="text">The text of the text box.</param>
        /// <param name="isActive">The active state of the text box.</param>
        /// <param name="textCaching">The text caching state of the text box.</param>
        /// <param name="basicTextBoxRendering">The basic text box rendering state of the text box.</param>
        public TextBoxUI(Image image)
            : base(image)
        {
            OnResize += TextBoxUI_OnResize;
        }

        /// <summary>
        /// Gets or sets the text to be rendered.
        /// </summary>
        public Text Text
        {
            get => text ??= new();
            set => text = value;
        }

        /// <summary>
        /// Gets or sets the background color of the text box used for previous text clearing if basic text box rendering is enabled.
        /// </summary>
        public Color BgColor
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <summary>
        /// Forces the text to be rendered on the next update.
        /// </summary>
        public void ForceNextRender()
        {
            forceRender = true;
        }

        /// <summary>
        /// Clears stored non-setting data.
        /// </summary>
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

        // Smart text map functions
        private void SmartMapLine(Vector2Int position, string line, Color fgColor = Color.White, Color bgColor = Color.Transparent)
        {
            MapLine(position, line, fgColor, bgColor);

            if (BasicTextBoxRendering)
            {
                int pixelLength = Pixel.GetPixelLength(line);

                renderedAreaList.Add(new Area2DInt(position, position + new Vector2Int(pixelLength, 1)));
            }
        }

        private void SmartMapText(Text text)
        {
            MapText(text, SmartMapLine);
        }

        private void Render()
        {
            if (BasicTextBoxRendering)
            {
                SmartClear();
                renderedText = (Text)Text.Clone();
                SmartMapText(renderedText);
            }
            else
            {
                renderedTextBoxUI = RenderClone();
                renderedText = renderedTextBoxUI.Text;
                renderedTextBoxUI.MapText(renderedText);
            }
        }

        /// <summary>
        /// Clears old text when basic text box rendering is enable.
        /// </summary>
        private void SmartClear()
        {
            foreach (Area2DInt area in renderedAreaList)
            {
                FillArea(new Pixel(Pixel.EmptyElement, Color.Black, BgColor), area);
            }

            renderedAreaList.Clear();
        }

        private void FillBackground() => BgColorFill(BgColor);

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
