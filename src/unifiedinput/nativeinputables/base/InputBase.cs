namespace SCE
{
    public abstract class InputBase : IInputReceiver
    {
        public bool IsActive { get; set; } = true;

        public abstract void LoadKeyInfo(UISKeyInfo uisKeyInfo);
    }
}
