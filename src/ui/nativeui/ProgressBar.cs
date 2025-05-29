using CSUtils;
namespace SCE
{
    public class ProgressBar : UIBaseExt
    {
        private bool renderQueued = true;

        private float value;

        private float min;

        private float max;

        private Pixel backFill = new(SCEColor.Black);

        private Pixel progressFill = new(SCEColor.Green);

        private FlowType flowMode = FlowType.LeftRight;

        public ProgressBar(int width, int height)
            : base(width, height)
        {
        }

        public ProgressBar(Vector2Int dimensions)
            : base(dimensions.X, dimensions.Y)
        {
        }

        public float Value
        {
            get => value;
            set => MiscUtils.QueueSet(ref this.value, value, ref renderQueued);
        }

        public float Min
        {
            get => min;
            set => MiscUtils.QueueSet(ref min, value, ref renderQueued);
        }

        public float Max
        {
            get => max;
            set => MiscUtils.QueueSet(ref max, value, ref renderQueued);
        }

        public Pixel BackFill
        {
            get => backFill;
            set => MiscUtils.QueueSet(ref backFill, value, ref renderQueued);
        }

        public Pixel ProgressFill
        {
            get => progressFill; 
            set => MiscUtils.QueueSet(ref progressFill, value, ref renderQueued);
        }

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

        public override DisplayMapView GetMapView()
        {
            if (renderQueued)
            {
                float t = Utils.Delerp(Value, Min, Max);

                bool h = FlowMode is FlowType.LeftRight or FlowType.RightLeft;

                int fill = (int)MathF.Round(Utils.Lerp(0, h ? Width : Height, t));

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

                renderQueued = false;
            }

            return (DisplayMapView)_dpMap;
        }
    }
}
