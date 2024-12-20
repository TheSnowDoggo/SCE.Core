namespace SCE
{
    public class UIBase : IRenderable
    {
        protected readonly DisplayMap _dpMap;

        public UIBase(int width, int height)
        {
            _dpMap = new(width, height);
        }
        public UIBase(Vector2Int dimensions)
        {
            _dpMap = new(dimensions);
        }
        public UIBase(int width, int height, Color bgColor)
        {
            _dpMap = new(width, height, bgColor);
        }
        public UIBase(Vector2Int dimensions, Color bgColor)
        {
            _dpMap = new(dimensions, bgColor);
        }

        public string Name { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public Vector2Int Position { get; set; }

        public int Layer { get; set; }

        public Anchor Anchor { get; set; }

        public int Width { get => _dpMap.Width; }

        public int Height { get => _dpMap.Height; }

        public Vector2Int Dimensions { get => _dpMap.Dimensions; }

        public DisplayMap GetMap()
        {
            Render();
            return _dpMap;
        }

        protected virtual void Render()
        {
        }
    }
}
