namespace SCECore.Types
{
    public class Image : DisplayMap, ICloneable, IEquatable<Image>, IRenderable
    {
        private const bool DefaultActiveState = true;

        private const Color DefaultBgColor = Color.Black;

        private const byte DefaultLayer = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="dimensions">The initial dimensions of the new image.</param>
        /// <param name="bgColor">The initial background color of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(Vector2Int dimensions, Color bgColor, bool isActive = DefaultActiveState)
            : base(dimensions, bgColor)
        {
            IsActive = isActive;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="dimensions">The initial dimensions of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(Vector2Int dimensions, bool isActive = DefaultActiveState)
            : this(dimensions, DefaultBgColor, isActive)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="displayMap">The base <see cref="DisplayMap"/> of the new image.</param>
        /// <param name="position">The initial position of the new image.</param>
        /// <param name="layer">The initial layer of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(DisplayMap displayMap, Vector2Int position, byte layer, bool isActive = DefaultActiveState)
            : base(displayMap)
        {
            Position = position;
            Layer = layer;
            IsActive = isActive;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="displayMap">The base <see cref="DisplayMap"/> of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(DisplayMap displayMap, bool isActive = DefaultActiveState)
            : this(displayMap, Vector2Int.Zero, DefaultLayer, isActive)
        {
        }

        /// <inheritdoc/>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the base <see cref="Vector2Int"/> position of the <see cref="Image"/>.
        /// </summary>
        public Vector2Int Position { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="byte"/> layer of the <see cref="Image"/>.
        /// </summary>
        public byte Layer { get; set; }

        /// <summary>
        /// Gets or sets the action called before returning itself in <see cref="GetImage"/> (called by <see cref="IRenderable"/>).
        /// </summary>
        public Action? OnRender { get; set; }

        /// <inheritdoc/>
        public override Image Clone()
        {
            return new(base.Clone(), Position, Layer, IsActive);
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
        public virtual Image GetImage()
        {
            OnRender?.Invoke();
            return this;
        }
    }
}
