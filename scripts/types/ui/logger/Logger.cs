namespace SCE
{
    public class Logger : UIBaseExt
    {
        private const string DEFAULT_NAME = "logger";

        public Logger(string name, int width, int height, Color? bgColor = null)
            : base(name, width, height, bgColor)
        {
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
    }
}
