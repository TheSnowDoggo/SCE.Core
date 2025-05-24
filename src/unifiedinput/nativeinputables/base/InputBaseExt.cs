namespace SCE
{
    public abstract class InputBaseExt : InputBase
    {
        public InputBaseExt(HashSet<InputType>? acceptedInputModes = null)
        {
            AcceptedInputModes = acceptedInputModes ?? new();
        }

        public HashSet<InputType> AcceptedInputModes { get; set; }

        public bool IsModeAccepted(InputType inputMode)
        {
            return AcceptedInputModes.Count > 0 && AcceptedInputModes.Contains(inputMode);
        }
    }
}
