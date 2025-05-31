using CSUtils;
namespace SCE
{
    public class ProgressBar : UIBaseExt
    {
        private bool renderQueued = true;

        private bool valueQueued = false;

        private int lastFill = -1; 

        public ProgressBar(int width, int height)
            : base(width, height)
        {
        }

        public ProgressBar(Vector2Int dimensions)
            : base(dimensions.X, dimensions.Y)
        {
        }

        private float value;

        public float Value
        {
            get => value;
            set => MiscUtils.QueueSet(ref this.value, value, ref valueQueued);
        }

        private float min;

        public float Min
        {
            get => min;
            set => MiscUtils.QueueSet(ref min, value, ref valueQueued);
        }

        private float max;

        public float Max
        {
            get => max;
            set => MiscUtils.QueueSet(ref max, value, ref valueQueued);
        }

        private Pixel backFill = new(SCEColor.Black);

        public Pixel BackFill
        {
            get => backFill;
            set => MiscUtils.QueueSet(ref backFill, value, ref renderQueued);
        }

        private Pixel progressFill = new(SCEColor.Green);

        public Pixel ProgressFill
        {
            get => progressFill; 
            set => MiscUtils.QueueSet(ref progressFill, value, ref renderQueued);
        }

        private FlowType flowMode = FlowType.LeftRight;

        public FlowType FlowMode
        {
            get => flowMode;
            set => MiscUtils.QueueSet(ref flowMode, value, ref renderQueued);
        }

        public void Resize(int width, int height)
        {
            _dpMap.CleanResize(width, height);
            renderQueued = true;
        }

        public void Resize(Vector2Int dimensions)
        {
            Resize(dimensions.X, dimensions.Y);
        }

        public override MapView<Pixel> GetMapView()
        {
            if (renderQueued || valueQueued)
            {
                float t = Utils.Delerp(Value, Min, Max);

                bool h = FlowMode is FlowType.LeftRight or FlowType.RightLeft;

                int fill = (int)MathF.Round(Utils.Lerp(0, h ? Width : Height, t));

                if (!valueQueued || fill != lastFill)
                {
                    lastFill = fill;

                    if (h)
                    {
                        for (int x = 0; x < Width; ++x)
                        {
                            var prog = FlowMode == FlowType.LeftRight ? x < fill : x >= Width - fill;
                            _dpMap.Fill(prog ? ProgressFill : BackFill, Rect2DInt.Vertical(x, Height));
                        }
                    }
                    else
                    {
                        for (int y = 0; y < Height; ++y)
                        {
                            var prog = FlowMode == FlowType.TopDown ? y < fill : y >= Height - fill;
                            _dpMap.Fill(prog ? ProgressFill : BackFill, Rect2DInt.Horizontal(y, Width));
                        }
                    }
                }

                renderQueued = false;
                valueQueued = false;
            }

            return _dpMap;
        }
    }
}
