namespace SCE
{
    public class UIBaseExt : UIBase
    {
        protected readonly DisplayMap _dpMap;

        public UIBaseExt(string name, int width, int height, SCEColor? bgColor = null)
            : base(name)
        {
            _dpMap = bgColor is SCEColor color ? new(width, height, color) : new(width, height);
        }

        public UIBaseExt(string name, Vector2Int dimensions, SCEColor? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public UIBaseExt(int width, int height, SCEColor? bgColor = null)
            : this(string.Empty, width, height, bgColor)
        {
        }

        public UIBaseExt(Vector2Int dimensions, SCEColor? bgColor = null)
            : this(string.Empty, dimensions, bgColor)
        {
        }

        public int Width { get => _dpMap.Width; }

        public int Height { get => _dpMap.Height; }

        public Vector2Int Dimensions { get => _dpMap.Dimensions; }

        public override DisplayMap GetMap()
        {
            Render();
            return _dpMap;
        }

        protected virtual void Render()
        {
        }
    }
}
