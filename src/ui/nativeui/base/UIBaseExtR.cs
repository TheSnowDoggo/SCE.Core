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

        public virtual void CleanResize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
        }

        public void CleanResize(Vector2Int dimensions)
        {
            CleanResize(dimensions.X, dimensions.Y);
        }

        public virtual void MapResize(int width, int height)
        {
            _dpMap.MapResize(width, height);
        }

        public void MapResize(Vector2Int dimensions)
        {
            MapResize(dimensions.X, dimensions.Y);
        }
    }
}
