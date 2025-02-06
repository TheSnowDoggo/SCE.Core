namespace SCE
{
    public abstract class InputBase : IInputReceiver
    {
        private const string DEFAULT_NAME = "input_base";

        public InputBase(string name = DEFAULT_NAME)
        {
            Name = name;
        }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public abstract void LoadKeyInfo(UISKeyInfo uisKeyInfo);
    }
}
