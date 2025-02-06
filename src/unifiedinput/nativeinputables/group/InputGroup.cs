namespace SCE
{
    public class InputGroup : SearchHashTypeExt<IInputReceiver>, IInputReceiver
    {
        private const string DEFAULT_NAME = "input_group";

        private bool quitLayer;

        #region Constructors
        public InputGroup(string name, IEnumerable<IInputReceiver> collection)
            : base(collection)
        {
            Name = name;
        }

        public InputGroup(IEnumerable<IInputReceiver> collection)
            : this(DEFAULT_NAME, collection)
        {
        }

        public InputGroup(string name = DEFAULT_NAME)
            : base()
        {
            Name = name;
        }
        #endregion

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            quitLayer = false;
            foreach (var inputable in this)
            {
                if (inputable.IsActive)
                    inputable.LoadKeyInfo(uisKeyInfo);
                if (quitLayer)
                    break;
            }
        }

        public void Quit()
        {
            quitLayer = true;
        }
    }
}
