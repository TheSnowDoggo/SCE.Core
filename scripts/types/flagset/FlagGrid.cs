namespace SCE
{
    public class FlagGrid
    {
        private FlagSet16 flagSet16;

        public FlagGrid()
        {
            flagSet16 = new();
        }

        public static explicit operator Grid2D<bool>(FlagGrid fG2D) => fG2D.ToGrid2D();

        public static int Width { get; } = 4;

        public static int Height { get; } = 4;

        public static Vector2Int Dimensions { get; } = new(Width, Height);

        public static Area2DInt GridArea { get; } = new(Vector2Int.Zero, Dimensions);

        public bool this[int x, int y]
        {
            get
            {
                if (!PositionValid(x, y))
                    throw new InvalidPositionException();
                return flagSet16[TranslateToBit(x, y)];
            }
            set
            {
                if (!PositionValid(x, y))
                    throw new InvalidPositionException();
                flagSet16[TranslateToBit(x, y)] = value;
            }
        }

        public bool this[Vector2Int pos]
        {
            get => this[pos.X, pos.Y];
            set => this[pos.X, pos.Y] = value;
        }

        public static void StaticCycle(Action<int, int> action)
        {
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                    action.Invoke(x, y);
            }
        }

        public static void StaticCycleArea(Action<int, int> action, Area2DInt area)
        {
            if (!Area2DInt.Overlaps(area, GridArea))
                throw new AreaOutOfBoundsException("Specified area doesn't overlap grid area.");

            area = GridArea.TrimArea(area);

            for (int x = area.Start.X; x < area.End.X; ++x)
            {
                for (int y = area.Start.Y; y < area.End.Y; ++y)
                    action.Invoke(x, y);
            }
        }

        public void Flip(int x, int y)
        {
            if (!PositionValid(x, y))
                throw new InvalidPositionException();
            flagSet16.Flip(TranslateToBit(x, y));
        }

        public void Flip(Vector2Int pos)
        {
            Flip(pos.X, pos.Y);
        }

        public void Fill(bool fill)
        {
            StaticCycle((x,y) => this[x,y] = fill);
        }

        public void FillArea(bool fill, Area2DInt area)
        {
            StaticCycleArea((x, y) => this[x, y] = fill, area);
        }

        public void Clear()
        {
            flagSet16.Clear();
        }

        public Grid2D<bool> ToGrid2D()
        {
            Grid2D<bool> grid = new(Width, Height);
            void CycleAction(Vector2Int pos)
            {
                grid[pos] = this[pos];
            }
            grid.GenericCycle(CycleAction);
            return grid;
        }

        private static int TranslateToBit(int x, int y)
        {
            return x % Width + y * Height;
        }

        private static bool PositionValid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
    }
}
