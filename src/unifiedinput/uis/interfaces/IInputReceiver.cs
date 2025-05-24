namespace SCE
{
    public interface IInputReceiver
    {
        bool IsActive { get; set; }

        void LoadKeyInfo(UISKeyInfo uisKeyInfo);
    }
}
