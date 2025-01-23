namespace SCE
{
    public class Image : DisplayMap, ICloneable, IEquatable<Image>, IRenderable
    {
        private const string DEFAULT_NAME = "image";

        private const int DEFAULT_LAYER = 0;

        public Image(string name, int width, int height, Color? bgColor = null)
            : base(width, height)
        {
            if (bgColor is Color color)
                BgColorFill(color);
            Name = name;
        }

        public Image(string name, Vector2Int dimensions, Color? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Image(int width, int height, Color bgColor)
            : base(width, height, bgColor)
        {
        }

        public Image(Vector2Int dimensions, Color? bgColor = null)
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

        public string Name { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public Vector2Int Position { get; set; }

        public int Layer { get; set; } = DEFAULT_LAYER;

        public Anchor Anchor { get; set; }

        /// <summary>
        /// Gets or sets the action called before returning itself in <see cref="GetMap"/> (called by <see cref="IRenderable"/>).
        /// </summary>
        public Action? OnRender { get; set; }

        /// <inheritdoc/>
        public override Image Clone()
        {
            Image clone = new(base.Clone())
            {
                Position = Position,
                IsActive = IsActive,
                Layer = Layer
            };

            return clone;
        }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }

        public bool Equals(Image? other)
        {
            if (other is null)
            {
                return false;
            }

            return other.IsActive == IsActive && other.Position == Position && other.Layer == Layer && other.OnRender == OnRender && base.Equals(other);
        }

        public override bool Equals(object? obj)
        {
            return obj is Image image && Equals(image);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsActive, Position, Layer, OnRender, base.GetHashCode());
        }

        /// <inheritdoc/>
        public virtual DisplayMap GetMap()
        {
            OnRender?.Invoke();
            return this;
        }
    }
}
