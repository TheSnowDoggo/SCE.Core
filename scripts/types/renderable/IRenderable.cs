namespace SCE
{
    public interface IRenderable : ISearcheable
    {
        bool IsActive { get; }

        Vector2Int Offset { get; }

        int Layer { get; }

        public Anchor Anchor { get; }

        DisplayMap GetMap();
    }
}
