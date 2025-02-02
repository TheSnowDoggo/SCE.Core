namespace SCE
{
    public class Image : DisplayMap, ICloneable, IEquatable<Image>, IRenderable
    {
        private const string DEFAULT_NAME = "image";

        private const int DEFAULT_LAYER = 0;

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
            : base(width, height, bgColor)
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

        public string Name { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public Vector2Int Offset { get; set; }

        public int Layer { get; set; } = DEFAULT_LAYER;

        public Anchor Anchor { get; set; }

        public Action? OnRender { get; set; }

        #region Clone
        public override Image Clone()
        {
            return new(base.Clone())
            {
                Offset = Offset,
                IsActive = IsActive,
                Layer = Layer
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Equality
        public bool Equals(Image? other)
        {
            if (other is null)
                return false;
            return other.IsActive == IsActive && other.Offset == Offset && other.Layer == Layer && other.OnRender == OnRender && base.Equals(other);
        }

        public override bool Equals(object? obj)
        {
            return obj is Image image && Equals(image);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        public virtual DisplayMap GetMap()
        {
            OnRender?.Invoke();
            return this;
        }
    }
}
