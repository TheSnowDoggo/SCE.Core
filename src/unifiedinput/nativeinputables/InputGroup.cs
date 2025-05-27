namespace SCE
{
    public class InputGroup : AliasHashTExt<IInputReceiver>, IInputReceiver
    {
        private bool quitLayer;

        public InputGroup()
            : base()
        {
        }

        public InputGroup(IEnumerable<IInputReceiver> collection)
            : base(collection)
        {
        }
        
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
