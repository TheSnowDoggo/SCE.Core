namespace SCE
{
    /// <summary>
    /// A legacy class for getting and handling inputs stored in <see cref="InputLayer"/>.
    /// </summary>
    public class InputHandler : AliasHashLExt<InputLayer>, IScene
    {
        private readonly Queue<UISKeyInfo> uisKeyInfoQueue = new();

        private bool quitKey;

        private bool flushKey;

        public InputHandler()
            : base()
        {
        }

        public InputHandler(IEnumerable<InputLayer> collection)
            : base(collection)
        {
        }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public void QueueKey(UISKeyInfo uisKeyInfo)
        {
            uisKeyInfoQueue.Enqueue(uisKeyInfo);
        }

        public void Quit()
        {
            quitKey = true;
        }

        public void Flush()
        {
            flushKey = true;
        }

        public void Start()
        {
        }

        public void Update()
        {
            if (!IsActive)
                return;
            SortLayers();
            RunQueue();
        }

        private void SortLayers()
        {
            _list.Sort();
        }

        private void RunQueue()
        {
            int queueCount = uisKeyInfoQueue.Count;

            flushKey = false;

            for (int i = 0; i < queueCount; ++i)
            {
                quitKey = false;
                var uisKeyInfo = uisKeyInfoQueue.Dequeue();

                foreach (var layer in _list)
                {
                    if (layer.IsActive)
                    {
                        layer.LoadKeyInfo(uisKeyInfo);
                    }
                    if (flushKey)
                    {
                        uisKeyInfoQueue.Clear();
                        return;
                    }
                    if (quitKey)
                    {
                        break;
                    }
                }
            }
        }
    }
}
