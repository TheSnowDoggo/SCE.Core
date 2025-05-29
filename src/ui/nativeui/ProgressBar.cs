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
            set
            {
                this.value = value;
                renderQueued = true;
            }
        }

        public float Min
        {
            get => min;
            set
            {
                min = value;
                renderQueued = true;
            }
        }

        public float Max
        {
            get => max;
            set
            {
                max = value;
                renderQueued = true;
            }
        }

        public Pixel BackFill
        {
            get => backFill;
            set
            {
                backFill = value;
                renderQueued = true;
            }
        }

        public Pixel ProgressFill
        {
            get => progressFill; 
            set
            {
                progressFill = value;
                renderQueued = true;
            }
        }

        public FlowType FlowMode
        {
            get => flowMode;
            set
            {
                flowMode = value;
                renderQueued = true;
            }
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
