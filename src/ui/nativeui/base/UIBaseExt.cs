namespace SCE
{
    public class UIBaseExt : UIBase
    {
        protected readonly DisplayMap _dpMap;

        public UIBaseExt(int width, int height, SCEColor? bgColor = null)
        {
            _dpMap = new(width, height, bgColor);
        }

        public UIBaseExt(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(dimensions.X, dimensions.Y, bgColor)
        {
        }

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

        /// <inheritdoc/>
        public override DisplayMapView GetMapView()
        {
            return (DisplayMapView)_dpMap;
        }
    }
}
