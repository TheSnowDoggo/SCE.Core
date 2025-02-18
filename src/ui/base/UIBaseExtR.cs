namespace SCE
{
    public class UIBaseExtR : UIBaseExt
    {
        private const string DEFAULT_NAME = "uibase_extr";

        #region Constructors

        public UIBaseExtR(string name, int width, int height, SCEColor? bgColor = null)
            : base(name, width, height, bgColor)
        {
        }

        public UIBaseExtR(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public UIBaseExtR(int width, int height, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public UIBaseExtR(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        #endregion

        #region Resize

        public virtual bool CleanResize(int width, int height)
        {
            if (width < 0 || height < 0)
                return false;
            _dpMap.CleanResize(width, height);
            return true;
        }

        public virtual bool CleanResize(Vector2Int dimensions)
        {
            return CleanResize(dimensions.X, dimensions.Y);
        }

        public virtual bool MapResize(int width, int height)
        {
            if (width < 0 || height < 0)
                return false;
            _dpMap.MapResize(width, height);
            return true;
        }

        public virtual bool MapResize(Vector2Int dimensions)
        {
            return MapResize(dimensions.X, dimensions.Y);
        }

        #endregion
    }
}
