namespace SCE
{
    public class InputAction<T> : InputBase
    {
        public InputAction(ConsoleKey key, Action<T>? onKey = null, T? value = default)
            : base()
        {
            Key = key;
            OnKey = onKey;
            Value = value;
        }

        public InputAction(ConsoleKey key, T? value)
            : this(key, null, value)
        {
        }

        public Action<T>? OnKey;

        public T? Value { get; set; }

        public ConsoleKey Key { get; }

        public HashSet<InputType> AllowedInputModes { get; set; } = new() { InputType.OnKeyDown, InputType.ConsoleStream };

        public override void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            if (!AllowedInputModes.Contains(uisKeyInfo.InputMode))
                return;
            if (Value is not null && OnKey is not null && uisKeyInfo.KeyInfo.Key == Key)
            {
                OnKey.Invoke(Value);
            }
        }
    }
}
