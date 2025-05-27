using System.Collections;
namespace SCE
{
    public class Grid2DView<T> : IEnumerable<Vector2Int>
    {
        private readonly Grid2D<T> _grid;

        public Grid2DView(Grid2D<T> grid)
        {
            _grid = grid;
        }

        public T this[Vector2Int pos] { get => _grid[pos]; }

        public T this[int x, int y] { get => _grid[x, y]; }

        public int Width { get => _grid.Width; }

        public int Height { get => _grid.Height; }

        public Vector2Int Dimensions { get => _grid.Dimensions; }

        public int Size() { return _grid.Size(); }

        public Rect2D GridArea() { return _grid.GridArea(); }

        public Grid2D<T> ToGrid2D()
        {
            return _grid.Clone();
        }

        #region IEnumerable

        public IEnumerator<Vector2Int> GetEnumerator()
        {
            return _grid.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _grid.GetEnumerator();
        }

        public IEnumerable<Vector2Int> EnumerateGrid(Rect2D area, bool rowMajor = true)
        {
            return _grid.EnumerateGrid(area, rowMajor);
        }

        public IEnumerable<Vector2Int> EnumerateGrid(bool rowMajor = true)
        {
            return _grid.EnumerateGrid(rowMajor);
        }

        #endregion
    }
}
