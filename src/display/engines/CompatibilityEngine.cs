using System.Text;
namespace SCE
{
    public sealed class CompatibilityEngine : RenderEngine
    {
        private static readonly Lazy<CompatibilityEngine> _lazy = new(() => new());

        private CompatibilityEngine()
        {
        }

        public static CompatibilityEngine Instance { get => _lazy.Value; }

        /// <inheritdoc/>
        public override void Render(DisplayMapView dpMap)
        {
            Console.SetCursorPosition(0, 0);

            StringBuilder sb = new();

            var lastSet = ColorSet.Zero;
            bool first = true;

            for (int y = 0; y < dpMap.Height; ++y)
            {
                if (y != 0)
                {
                    sb.Append('\n');
                }
                for (int x = 0; x < dpMap.Width; ++x)
                {
                    if (first)
                    {
                        lastSet = dpMap[x, y].ColorSet();
                        first = false;
                    }
                    var set = dpMap[x, y].ColorSet();
                    if (lastSet == set)
                    {
                        sb.Append(dpMap[x, y].Element);
                    }
                    else
                    {
                        ColorUtils.SetConsoleColor(lastSet);
                        Console.Write(sb);
                        sb.Clear(); 

                        sb.Append(dpMap[x, y].Element);

                        lastSet = set;
                    }
                }
            }

            ColorUtils.SetConsoleColor(lastSet);

            Console.Write(sb);

            Console.ResetColor();
        }
    }
}
