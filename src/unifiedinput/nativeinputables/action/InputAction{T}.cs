namespace SCE
{
    public class InputAction<T> : InputBase
    {
        private const string DEFAULT_NAME = "input_action<T>";

        #region Constructors
        public InputAction(string name, ConsoleKey key, Action<T>? onKey = null, T? value = default)
            : base(name)
        {
            Key = key;
            OnKey = onKey;
            Value = value;
        }

        public InputAction(ConsoleKey key, Action<T>? onKey = null, T? value = default)
            : this(DEFAULT_NAME, key, onKey, value)
        {
        }

        public InputAction(string name, ConsoleKey key, T? value)
            : this(name, key, null, value)
        {
        }

        public InputAction(ConsoleKey key, T? value)
            : this(DEFAULT_NAME, key, value)
        {
        }
        #endregion

        public Action<T>? OnKey;

        public T? Value { get; set; }

        public ConsoleKey Key { get; }

        public HashSet<InputType> AllowedInputModes { get; set; } = new() { InputType.OnKeyDown, InputType.ConsoleStream };

        public override void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            if (!AllowedInputModes.Contains(uisKeyInfo.InputMode))
                return;
            if (Value is not null && OnKey is not null && uisKeyInfo.KeyInfo.Key == Key)
                OnKey.Invoke(Value);
        }
    }
}
