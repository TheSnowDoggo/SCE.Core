using System.Numerics;

namespace SCE
{
    public class BasicWriter : UIBaseExt
    {
        private const string DEFAULT_NAME = "basic_writer";

        public BasicWriter(string name, int width, int height, Color? bgColor = null)
            : base(name, width, height, bgColor)
        {
        }

        public BasicWriter(string name, Vector2Int dimensions, Color? bgColor = null)
            : base(name, dimensions.X, dimensions.Y, bgColor)
        {
        }

        public BasicWriter(int width, int height, Color? bgColor = null)
            : base(DEFAULT_NAME, width, height, bgColor)
        {
        }

        public BasicWriter(Vector2Int dimensions, Color? bgColor = null)
            : base(DEFAULT_NAME, dimensions, bgColor)
        {
        }
    }
}
