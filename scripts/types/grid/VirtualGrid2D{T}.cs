namespace SCE
{
    public class VirtualGrid2D<T> : VirtualGrid2D<T, T>
    {
        public VirtualGrid2D(Grid2D<T> grid)
            : base(grid, (pos, val) => val)
        {
        }
    }
}
