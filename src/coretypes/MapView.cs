using System.Collections;

namespace SCE
{
    public abstract class MapView<T> : IEnumerable<Vector2Int>
    {
        public T this[Vector2Int pos] { get => this[pos.X, pos.Y]; }

        public abstract T this[int x, int y] { get; }

        public abstract Vector2Int Dimensions { get; }

        public int Width { get => Dimensions.X; }

        public int Height { get => Dimensions.Y; }

        public int Size() { return Width * Height; }

        public Rect2DInt GridArea() { return new(Dimensions); }

        public virtual IEnumerator<Vector2Int> GetEnumerator()
        {
            return GridArea().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
