namespace SCE
{
    public class Selection
    {
        public string Text { get; init; } = "";

        public Action? OnSelect { get; init; }

        public Action? OnHover { get; init; }

        public ColorSet? UnselectedColors { get; init; }

        public ColorSet? SelectedColors { get; init; }

        public Anchor? Anchor { get; init; }

        public bool? FitToLength { get; init; }
    }
}
