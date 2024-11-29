namespace SCECore
{
    public readonly struct Option
    {
        public Option(string name, Action? action)
        {
            Name = name;
            Action = action;
        }

        public Option(string name)
            : this(name, null)
        {
        }

        public string Name { get; }

        public Action? Action { get; }
    }
}
