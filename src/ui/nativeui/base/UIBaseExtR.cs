namespace SCE
{
    public class UIBaseExtR : UIBaseExt
    {
        public UIBaseExtR(int width, int height, SCEColor? bgColor = null)
            : base(width, height, bgColor)
        {
        }

        public UIBaseExtR(Vector2Int dimensions, SCEColor? bgColor = null)
            : base(dimensions.X, dimensions.Y, bgColor)
        {
        }

        public virtual bool CleanResize(int width, int height)
        {
            if (width < 0 || height < 0)
            {
                return false;
            }
            _dpMap.CleanResize(width, height);
            return true;
        }

        public bool CleanResize(Vector2Int dimensions)
        {
            return CleanResize(dimensions.X, dimensions.Y);
        }

        public virtual bool MapResize(int width, int height)
        {
            if (width < 0 || height < 0)
            {
                return false;
            }
            _dpMap.MapResize(width, height);
            return true;
        }

        public bool MapResize(Vector2Int dimensions)
        {
            return MapResize(dimensions.X, dimensions.Y);
        }
    }
}
