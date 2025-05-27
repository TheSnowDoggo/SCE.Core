namespace SCE
{
    public readonly struct UISKeyInfo
    {
        public UISKeyInfo(ConsoleKeyInfo keyInfo, InputType inputMode)
        {
            KeyInfo = keyInfo;
            InputMode = inputMode;
        }

        public ConsoleKeyInfo KeyInfo { get; }

        public InputType InputMode { get; }
    }
}
