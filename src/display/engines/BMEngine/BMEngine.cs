namespace SCE
{
    /// <summary>
    /// Super fast windows color rendering by directly manipulating the console buffer.
    /// </summary>
    public sealed class BMEngine : RenderEngine
    {
        private static readonly Lazy<BMEngine> _lazy = new(() => new());

        public BMEngine()
        {
        }

        /// <summary>
        /// Gets the lazy instance of this class.
        /// </summary>
        public static BMEngine Instance { get => _lazy.Value; }

        private static short ToAttributes(ConsoleColor fg, ConsoleColor bg)
        {
            return (short)((int)fg | ((int)bg << 4));
        }

        public override void Render(DisplayMapView dpMap, Vector2Int start)
        {
            var arr = new CharInfo[dpMap.Size()];

            Coord size = new()
            {
                X = (short)dpMap.Width,
                Y = (short)dpMap.Height,
            };

            SmallRect rect = new()
            {
                Left   = (short)start.X,
                Top    = (short)start.Y,
                Right  = (short)(size.X + start.X),
                Bottom = (short)(size.Y + start.Y),
            };

            int i = 0;
            foreach (var pos in dpMap)
            {
                var fg = ColorUtils.ToConsoleColor(dpMap[pos].FgColor);
                var bg = ColorUtils.ToConsoleColor(dpMap[pos].BgColor);

                arr[i].Char.UnicodeChar = dpMap[pos].Element;
                arr[i].Attributes = ToAttributes(fg, bg);
                ++i;
            }

            BufferDrawer.Instance.WriteBuffer(arr, size, Coord.Zero, ref rect);
        }
    }
}
