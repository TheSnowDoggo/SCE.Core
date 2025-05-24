namespace SCE
{
    public class InputAction : InputBase
    {
        public InputAction(ConsoleKey key, Action? onKey = null)
            : base()
        {
            Key = key;
            OnKey = onKey;
        }

        public Action? OnKey;

        public ConsoleKey Key { get; }

        public HashSet<InputType> AllowedInputModes { get; set; } = new()
        {
            InputType.OnKeyDown,
            InputType.ConsoleStream
        };

        public override void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            if (!AllowedInputModes.Contains(uisKeyInfo.InputMode))
                return;
            if (OnKey is not null && uisKeyInfo.KeyInfo.Key == Key)
                OnKey.Invoke();
        }
    }
}
