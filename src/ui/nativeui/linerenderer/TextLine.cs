namespace SCE
{
    public class TextLine
    {
        public string Text { get; init; } = "";

        public SCEColor? FgColor { get; init; }

        public SCEColor? BgColor { get; init; }

        public Anchor? Anchor { get; init; }

        public bool? FitToLength { get; init; }
    }
}
