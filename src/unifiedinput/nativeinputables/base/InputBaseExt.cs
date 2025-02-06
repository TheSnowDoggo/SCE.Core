namespace SCE
{
    public abstract class InputBaseExt : InputBase
    {
        private const string DEFAULT_NAME = "input_base_ext";

        public InputBaseExt(string name, HashSet<InputType>? acceptedInputModes = null)
            : base(name)
        {
            AcceptedInputModes = acceptedInputModes ?? new();
        }

        public InputBaseExt(HashSet<InputType>? acceptedInputModes = null)
            : this(DEFAULT_NAME, acceptedInputModes)
        {
        }

        public HashSet<InputType> AcceptedInputModes { get; set; }

        public bool IsModeAccepted(InputType inputMode)
        {
            return AcceptedInputModes.Count > 0 && AcceptedInputModes.Contains(inputMode);
        }
    }
}
