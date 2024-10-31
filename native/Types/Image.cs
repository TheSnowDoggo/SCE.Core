namespace SCECore.Types
{
    using SCECore.Components;

    public class Image : DisplayMap, ICloneable, IRenderable, ICContainerHolder
    {
        private const bool DefaultActiveState = true;

        private const byte DefaultBgColor = Color.Black;

        private const byte DefaultLayer = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="dimensions">The initial dimensions of the new image.</param>
        /// <param name="bgColor">The initial background color of the new image.</param>
        /// <param name="cList">The initial cList of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(Vector2Int dimensions, byte bgColor, CList cList, bool isActive = DefaultActiveState)
            : base(dimensions, bgColor)
        {
            IsActive = isActive;

            CContainer = new(this, cList);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="dimensions">The initial dimensions of the new image.</param>
        /// <param name="bgColor">The initial background color of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(Vector2Int dimensions, byte bgColor, bool isActive = DefaultActiveState)
            : this(dimensions, bgColor, new CList(), isActive)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="dimensions">The initial dimensions of the new image.</param>
        /// <param name="cList">The initial cList of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(Vector2Int dimensions, CList cList, bool isActive = DefaultActiveState)
            : this(dimensions, DefaultBgColor, cList, isActive)
        {
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
        /// <param name="cList">The initial cList of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(DisplayMap displayMap, Vector2Int position, byte layer, CList cList, bool isActive = DefaultActiveState)
            : base(displayMap)
        {
            Position = position;
            Layer = layer;
            IsActive = isActive;

            CContainer = new(this, cList);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="displayMap">The base <see cref="DisplayMap"/> of the new image.</param>
        /// <param name="position">The initial position of the new image.</param>
        /// <param name="layer">The initial layer of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(DisplayMap displayMap, Vector2Int position, byte layer, bool isActive = DefaultActiveState)
            : this(displayMap, position, layer, new CList(), isActive)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="displayMap">The base <see cref="DisplayMap"/> of the new image.</param>
        /// <param name="cList">The initial cList of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(DisplayMap displayMap, CList cList, bool isActive = DefaultActiveState)
            : this(displayMap, Vector2Int.Zero, DefaultLayer, cList, isActive)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="displayMap">The base <see cref="DisplayMap"/> of the new image.</param>
        /// <param name="isActive">The initial active state of the new image.</param>
        public Image(DisplayMap displayMap, bool isActive = DefaultActiveState)
            : this(displayMap, Vector2Int.Zero, DefaultLayer, new CList(), isActive)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="image">The base <see cref="Image"/> of the new image.</param>
        public Image(Image image)
            : this(image, image.Position, image.Layer, image.CContainer.CList, image.IsActive)
        {
        }

        /// <summary>
        /// A delegate type called when the image is being.
        /// </summary>
        public delegate void CallOnRender();

        /// <inheritdoc/>
        public CContainer CContainer { get; init; }

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
        /// Gets the zero-based top-right corner position from the base <see cref="Vector2"/> position.
        /// </summary>
        public Vector2Int PositionCorner { get => Position + Dimensions; }

        /// <summary>
        /// Gets the aligned <see cref="Vector2Int"/> position of the first <see cref="IComponent"/> which implements <see cref="IAlignPositionInt"/> if found in the <see cref="LunaSCE.CContainer"/>; otherwise, <see cref="Position"/>.
        /// </summary>
        public Vector2Int AlignedPosition { get => ResolveAlignedPosition(); }

        /// <summary>
        /// Gets the zero-based top-right corner position from the aligned <see cref="Vector2Int"/> position.
        /// </summary>
        public Vector2Int AlignedPositionCorner { get => AlignedPosition + Dimensions; }

        /// <summary>
        /// Gets the aligned area from the aligned position and the aligned position corner.
        /// </summary>
        public Area2DInt AlignedArea { get => new(AlignedPosition, AlignedPositionCorner); }

        /// <summary>
        /// Gets or sets the <see cref="CallOnRender"/> delegate called before returning itself in <see cref="GetImage"/> (called by <see cref="IRenderable"/>).
        /// </summary>
        public CallOnRender? OnRender { get; set; }

        /// <inheritdoc/>
        public override Image Clone()
        {
            return new(base.Clone(), Position, Layer, CContainer.CList, IsActive);
        }

        /// <inheritdoc/>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <inheritdoc/>
        public virtual Image GetImage()
        {
            OnRender?.Invoke();
            return this;
        }

        /// <summary>
        /// Searches and returns the aligned <see cref="Vector2Int"/> position of the first <see cref="IComponent"/> which implements <see cref="IAlignPositionInt"/> if found in the <see cref="LunaSCE.CContainer"/>; otherwise, <see cref="Position"/>.
        /// </summary>
        /// <returns>Returns the aligned <see cref="Vector2Int"/> position of the first <see cref="IComponent"/> which implements <see cref="IAlignPositionInt"/> if found in the <see cref="LunaSCE.CContainer"/>; otherwise, <see cref="Position"/>.</returns>
        private Vector2Int ResolveAlignedPosition()
        {
            foreach (IComponent component in CContainer)
            {
                if (component.IsActive && component is IAlignPositionInt alignPosition)
                {
                    return alignPosition.AlignedPosition;
                }
            }

            return Position;
        }
    }
}
