namespace SCE
{
    public class VirtualGrid2D<T,V>
    {
        private const bool DEFAULT_TRIM = false;

        #region Constructors

        public VirtualGrid2D(Grid2D<T> grid, Func<T, V, T> fillFunc)
        {
            Grid = grid;
            FillFunc = fillFunc;
        }

        #endregion

        #region Properties

        public Grid2D<T> Grid { get; }

        public Func<T, V, T> FillFunc { get; }

        #endregion

        #region Fill

        public void FillArea(V item, Rect2D area, bool trimOnOverflow = DEFAULT_TRIM)
        {
            Grid.GenericCycleArea((Vector2Int pos) => Grid[pos] = FillFunc(Grid[pos], item), area, trimOnOverflow);
        }

        public void Fill(V item)
        {
            FillArea(item, Grid.GridArea);
        }

        public void FillHorizontalArea(V item, int y, Vector2Int range, bool trimOnOverflow = DEFAULT_TRIM)
        {
            FillArea(item, new Rect2D(new Vector2Int(range.X, y), new Vector2Int(range.Y, y + 1)), trimOnOverflow);
        }

        public void FillVerticalArea(V item, int x, Vector2Int range, bool trimOnOverflow = DEFAULT_TRIM)
        {
            FillArea(item, new Rect2D(new Vector2Int(x, range.X), new Vector2Int(x + 1, range.Y)), trimOnOverflow);
        }

        public void FillHorizontal(V item, int y)
        {
            FillHorizontalArea(item, y, new Vector2Int(0, Grid.Width));
        }

        public void FillVertical(V item, int x)
        {
            FillVerticalArea(item, x, new Vector2Int(0, Grid.Height));
        }

        #endregion
    }
}
