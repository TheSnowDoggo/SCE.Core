namespace SCE
{
    public abstract class UIBase : IRenderable
    {
        public UIBase(string name = "")
        {
            Name = name;
        }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public Vector2Int Offset { get; set; }

        public int Layer { get; set; }

        public Anchor Anchor { get; set; }

        public abstract DisplayMap GetMap();
    }
}
