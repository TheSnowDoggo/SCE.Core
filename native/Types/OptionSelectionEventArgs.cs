namespace SCECore.Types
{
    public class OptionSelectionEventArgs : EventArgs
    {
        public OptionSelectionEventArgs(Option option)
        {
            Option = option;
        }

        public Option Option { get; }
    }
}
