namespace SCE
{
    public class Grid2DView<T> : MapView<T>
    {
        private readonly Grid2D<T> _grid;

        public Grid2DView(Grid2D<T> grid)
        {
            _grid = grid;
        }

        public override T this[int x, int y] { get => _grid[x, y]; }

        public override Vector2Int Dimensions { get => _grid.Dimensions; }

        public Grid2D<T> ToGrid2D()
        {
            return _grid.Clone();
        }
    }
}
