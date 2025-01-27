namespace SCE
{
    public class FrameLimiter : IUpdateLimit
    {
        private int count = 0;

        public FrameLimiter(int framesPerUpdate)
        {
            FramesPerUpdate = framesPerUpdate;
        }

        public int Count { get => count; }

        public int FramesPerUpdate { get; set; } = 1;

        public bool OnUpdate()
        {
            bool update = ++count >= FramesPerUpdate;
            if (update)
                count = 0;
            return update;
        }
    }
}
