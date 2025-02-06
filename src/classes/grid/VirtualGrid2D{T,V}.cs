namespace SCE
{
    /// <summary>
    /// Provides modular filling functions for a <see cref="Grid2D{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the grid.</typeparam>
    /// <typeparam name="V">The type of the values to fill with.</typeparam>
    public class VirtualGrid2D<T,V>
    {
        private const bool DEFAULT_TRIM = false;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualGrid2D{T, V}"/> class.
        /// </summary>
        /// <param name="grid">The grid to use.</param>
        /// <param name="fillFunc">The func to be invoked when filling.</param>
        public VirtualGrid2D(Grid2D<T> grid, Func<T, V, T> fillFunc)
        {
            Grid = grid;
            FillFunc = fillFunc;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the grid of this.
        /// </summary>
        public Grid2D<T> Grid { get; }

        /// <summary>
        /// Gets the filling func of this.
        /// </summary>
        public Func<T, V, T> FillFunc { get; }

        #endregion

        #region Fill

        /// <summary>
        /// Fills the grid over a specified area <paramref name="area"/>.
        /// </summary>
        /// <param name="item">The item to fill with.</param>
        /// <param name="area">The area to fill over.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the area to fit within the bounds of this grid.</param>
        public void FillArea(V item, Rect2D area, bool trimOnOverflow = DEFAULT_TRIM)
        {
            Grid.GenericCycleArea((Vector2Int pos) => Grid[pos] = FillFunc(Grid[pos], item), area, trimOnOverflow);
        }

        /// <summary>
        /// Fills the grid.
        /// </summary>
        /// <param name="item">The item to fill with.</param>
        public void Fill(V item)
        {
            FillArea(item, Grid.GridArea);
        }

        /// <summary>
        /// Fills the grid at a specified y over a horizontal range <paramref name="xRange"/>.
        /// </summary>
        /// <param name="item">The item to fill with.</param>
        /// <param name="y">The zero-based y to fill at.</param>
        /// <param name="xRange">The horizontal range to fill over.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the range to fit within the bounds of this grid.</param>
        public void FillHorizontalArea(V item, int y, Vector2Int xRange, bool trimOnOverflow = DEFAULT_TRIM)
        {
            FillArea(item, new Rect2D(new Vector2Int(xRange.X, y), new Vector2Int(xRange.Y, y + 1)), trimOnOverflow);
        }

        /// <summary>
        /// Fills the grid at a specified x over a vertical range <paramref name="yRange"/>.
        /// </summary>
        /// <param name="item">The item to fill with.</param>
        /// <param name="x">The zero-based x to fill at.</param>
        /// <param name="yRange">The vertical range to fill over.</param>
        /// <param name="trimOnOverflow">If <see langword="true"/>, try to trim the range to fit within the bounds of this grid.</param>
        public void FillVerticalArea(V item, int x, Vector2Int yRange, bool trimOnOverflow = DEFAULT_TRIM)
        {
            FillArea(item, new Rect2D(new Vector2Int(x, yRange.X), new Vector2Int(x + 1, yRange.Y)), trimOnOverflow);
        }

        /// <summary>
        /// Fills the grid at a specified y (a horizontal line).
        /// </summary>
        /// <param name="item">The item to fill with.</param>
        /// <param name="y">The zero-based y to fill at.</param>
        public void FillHorizontal(V item, int y)
        {
            FillHorizontalArea(item, y, new Vector2Int(0, Grid.Width));
        }

        /// <summary>
        /// Fills the grid at a specified x (a vertical line).
        /// </summary>
        /// <param name="item">The item to fill with.</param>
        /// <param name="x">The zero-based x to fill at.</param>
        public void FillVertical(V item, int x)
        {
            FillVerticalArea(item, x, new Vector2Int(0, Grid.Height));
        }

        #endregion
    }
}
