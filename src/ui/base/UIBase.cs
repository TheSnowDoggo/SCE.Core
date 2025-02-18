namespace SCE
{
    public abstract class UIBase : IRenderable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIBase"/> class.
        /// </summary>
        /// <param name="name">The initial name.</param>
        public UIBase(string name = "")
        {
            Name = name;
        }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public bool IsActive { get; set; } = true;

        /// <inheritdoc/>
        public Vector2Int Offset { get; set; }

        /// <inheritdoc/>
        public int Layer { get; set; }

        /// <inheritdoc/>
        public Anchor Anchor { get; set; }

        /// <inheritdoc/>
        public abstract DisplayMap GetMap();
    }
}
