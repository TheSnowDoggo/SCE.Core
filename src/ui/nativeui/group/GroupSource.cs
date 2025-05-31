namespace SCE
{
    public class GroupSource
    {
        public bool IsActive { get; set; } = true;

        public Vector2Int Offset { get; set; }

        public int Layer { get; set; }

        public Anchor Anchor { get; set; }
    }
}
