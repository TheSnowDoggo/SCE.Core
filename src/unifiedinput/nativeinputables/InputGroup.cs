namespace SCE
{
    public class InputGroup : AliasHashTExt<IInputReceiver>, IInputReceiver
    {
        private bool flush;

        public InputGroup()
            : base()
        {
        }

        public InputGroup(IEnumerable<IInputReceiver> collection)
            : base(collection)
        {
        }
        
        public bool IsActive { get; set; } = true;

        public void Flush()
        {
            flush = true;
        }

        public void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            flush = false;
            foreach (var inputable in this)
            {
                if (inputable.IsActive)
                {
                    inputable.LoadKeyInfo(uisKeyInfo);
                }
                if (flush)
                {
                    break;
                }
            }
        }
    }
}
