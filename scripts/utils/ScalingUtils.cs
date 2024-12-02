namespace SCE
{
    /// <summary>
    /// Legacy <see cref="Grid2D{T}"/> scaling class.
    /// </summary>
    public static class ScalingUtils
    {
        public static Grid2D<T> GetScaledGrid2D<T>(Grid2D<T> grid, int scaleFactor)
        {
            Grid2D<T> startGrid = grid.Clone(), newGrid = new(startGrid.Dimensions * scaleFactor);

            void CycleAction(Vector2Int pos)
            {
                Vector2Int start = pos * scaleFactor;

                Area2DInt area = new(start, start + scaleFactor);

                T value = startGrid[pos];

                newGrid.FillArea(value, area);
            }
            startGrid.GenericCycle(CycleAction);

            return newGrid;
        }
    }
}
