namespace SCE
{
    public readonly struct LineAttributes
    {
        public SCEColor? FgColor { get; init; }

        public SCEColor? BgColor { get; init; }

        public Anchor? Anchor { get; init; }

        public bool? FitToLength { get; init; }
    }
}
