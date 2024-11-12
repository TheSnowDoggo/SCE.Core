namespace SCECore.Types
{
    public class OptionSelectorInvokeEventArgs : EventArgs
    {
        public OptionSelectorInvokeEventArgs(Option option)
        {
            Option = option;
        }

        public Option Option { get; }
    }
}
