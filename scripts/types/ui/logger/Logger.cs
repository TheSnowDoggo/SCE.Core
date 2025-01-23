namespace SCE
{
    public class Logger : UIBase
    {
        private const string DEFAULT_NAME = "logger";

        private readonly LineRenderer _lineRenderer;

        private int currentY;

        public Logger(string name, int width, int height, Color? bgColor = null)
            : base(name)
        {
            _lineRenderer = bgColor is Color color ? new(width, height, color) : new(width, height);
        }

        public Logger(string name, Vector2Int dimensions, Color? bgColor = null)
            : this(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public Logger(int width, int height, Color? bgColor = null)
            : this(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public Logger(Vector2Int dimensions, Color? bgColor = null)
            : this(DEFAULT_NAME, dimensions, bgColor)
        {
        }

        public void Log(Line line)
        {
            _lineRenderer[currentY] = line;
            if (++currentY == _lineRenderer.Width)
            {

            }
        }

        private void ShiftUp(int y)
        {

        }

        public override DisplayMap GetMap()
        {
            throw new NotImplementedException();
        }
    }
}
