namespace SCE
{
    public sealed class Display : CustomDisplay
    {
        private static readonly Lazy<Display> _lazy = new(() => new());

        public Display()
        {
            OnRender = () => Renderables;
        }

        /// <summary>
        /// Gets the lazy instance of this class.
        /// </summary>
        public static Display Instance { get => _lazy.Value; }

        public AliasHash<IRenderable> Renderables { get; } = new();
    }
}