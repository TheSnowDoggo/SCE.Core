namespace SCE
{
    public class UIBaseExt : UIBase
    {
        private const string DEFAULT_NAME = "uibase_ext";

        protected readonly DisplayMap _dpMap;

        #region Constructors

        public UIBaseExt(string name, int width, int height, SCEColor? bgColor = null)
            : base(name)
        {
            _dpMap = new(width, height, bgColor);
        }

        public UIBaseExt(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public UIBaseExt(int width, int height, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public UIBaseExt(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width { get => _dpMap.Width; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height { get => _dpMap.Height; }

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        public Vector2Int Dimensions { get => _dpMap.Dimensions; }

        #endregion

        /// <inheritdoc/>
        public override DisplayMap GetMap()
        {
            return _dpMap;
        }
    }
}
