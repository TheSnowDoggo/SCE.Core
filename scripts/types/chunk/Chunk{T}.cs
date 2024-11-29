namespace SCECore
{
    internal class Chunk<T> : Grid2D<T>
    {
        public Chunk(Vector2Int position, int width, int height)
            : base(width, height)
        {
            Position = position;
        }

        public Chunk(Vector2Int position, Vector2Int dimensions)
            : base(dimensions)
        {
            Position = position;
        }

        public Vector2Int Position { get; set; }
    }
}
