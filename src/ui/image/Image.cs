namespace SCE
{
    public class Image : DisplayMap, ICloneable, IRenderable
    {
        private const string DEFAULT_NAME = "image";

        #region Constructors

        public Image(string name, int width, int height, SCEColor? bgColor = null)
            : base(width, height, bgColor)
        {
            Name = name;
        }

        public Image(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Image(int width, int height, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public Image(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Image(string name, DisplayMap displayMap)
            : base(displayMap)
        {
            Name = name;
        }

        public Image(DisplayMap displayMap)
            : this(DEFAULT_NAME, displayMap)
        {
        }

        #endregion

        #region IRenderable

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public int Layer { get; set; }

        public Vector2Int Offset { get; set; }

        public Anchor Anchor { get; set; }

        #endregion

        public Action? OnRender;

        #region Clone

        public override Image Clone()
        {
            return new(base.Clone())
            {
                IsActive = IsActive,
                Layer = Layer,
                Offset = Offset,
                Anchor = Anchor,
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        /// <inheritdoc/>
        public virtual DisplayMap GetMap()
        {
            OnRender?.Invoke();
            return this;
        }
    }
}
