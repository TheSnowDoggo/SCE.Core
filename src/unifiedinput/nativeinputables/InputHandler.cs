namespace SCE
{
    /// <summary>
    /// A legacy class for getting and handling inputs stored in <see cref="InputLayer"/>.
    /// </summary>
    public class InputHandler : AliasHash<InputLayer>, IScene
    {
        private readonly Queue<UISKeyInfo> uisKeyInfoQueue = new();

        private bool flush;

        private bool flushAll;

        public InputHandler()
            : base()
        {
        }

        public InputHandler(IEnumerable<InputLayer> collection)
            : base(collection)
        {
        }

        public bool IsActive { get; set; } = true;

        public void QueueKey(UISKeyInfo uisKeyInfo)
        {
            uisKeyInfoQueue.Enqueue(uisKeyInfo);
        }

        public void Flush()
        {
            flush = true;
        }

        public void FlushAll()
        {
            flushAll = true;
        }

        public void Start()
        {
        }

        public void Update()
        {
            List<InputLayer> list = new(Count);

            list.AddRange(this);

            list.Sort();

            int queueCount = uisKeyInfoQueue.Count;

            flushAll = false;

            for (int i = 0; i < queueCount; ++i)
            {
                flush = false;
                var uisKeyInfo = uisKeyInfoQueue.Dequeue();

                foreach (var layer in list)
                {
                    if (layer.IsActive)
                    {
                        layer.LoadKeyInfo(uisKeyInfo);
                    }
                    if (flushAll)
                    {
                        uisKeyInfoQueue.Clear();
                        return;
                    }
                    if (flush)
                    {
                        break;
                    }
                }
            }
        }
    }
}
