namespace SCE
{
    public class Image : DisplayMap, ICloneable, IEquatable<Image>, IRenderable
    {
        private const int DefaultLayer = 0;

        public Image(int width, int height)
            : base(width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="dimensions">The initial dimensions of the new image.</param>
        public Image(Vector2Int dimensions)
            : base(dimensions)
        {
        }

        public Image(int width, int height, Color bgColor)
            : base(width, height, bgColor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="dimensions">The initial dimensions of the new image.</param>
        /// <param name="bgColor">The initial background color of the new image.</param>
        public Image(Vector2Int dimensions, Color bgColor)
            : base(dimensions, bgColor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="displayMap">The base <see cref="DisplayMap"/> of the new image.</param>
        public Image(DisplayMap displayMap)
            : base(displayMap)
        {
        }

        public string Name { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public Vector2Int Position { get; set; }

        public int Layer { get; set; } = DefaultLayer;

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
