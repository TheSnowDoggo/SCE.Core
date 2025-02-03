using System.Reflection.Metadata.Ecma335;

namespace SCE
{
    public class Option : ISearcheable
    {
        private static readonly ColorSet DEFAULT_COLORS = new(SCEColor.Gray, SCEColor.Black);

        public Option(string name, Action? action = null, ColorSet? colorSet = null)
        {
            Name = name;
            Action = action;
            Colors = colorSet ?? DEFAULT_COLORS;
        }

        public Option(string name, ColorSet colorSet)
            : this(name, null, colorSet)
        {
        }

        public string Name { get; set; }

        public Action? Action { get; set; }

        public ColorSet Colors { get; set; }

        public HorizontalAnchor Anchor { get; set; }

        public bool TryRun()
        {
            if (Action is null)
                return false;
            Action.Invoke();
            return true;
        }
    }
}
