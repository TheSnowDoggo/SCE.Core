namespace SCE
{
    public class InputAction : InputBase
    {
        private const string DEFAULT_NAME = "input_action";

        #region Constructors
        public InputAction(string name, ConsoleKey key, Action? onKey = null)
            : base(name)
        {
            Key = key;
            OnKey = onKey;
        }

        public InputAction(ConsoleKey key, Action? onKey = null)
            : this(DEFAULT_NAME, key, onKey)
        {
        }
        #endregion

        public Action? OnKey;

        public ConsoleKey Key { get; }

        public HashSet<InputType> AllowedInputModes { get; set; } = new() { InputType.OnKeyDown, InputType.ConsoleStream };

        public override void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            if (!AllowedInputModes.Contains(uisKeyInfo.InputMode))
                return;
            if (OnKey is not null && uisKeyInfo.KeyInfo.Key == Key)
                OnKey.Invoke();
        }
    }
}
