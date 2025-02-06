namespace SCE
{
    public interface IInputReceiver : ISearcheable
    {
        bool IsActive { get; set; }

        void LoadKeyInfo(UISKeyInfo uisKeyInfo);
    }
}
