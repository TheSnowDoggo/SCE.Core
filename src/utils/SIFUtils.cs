using CSUtils;
using System.Diagnostics.CodeAnalysis;
using System.Text;
namespace SCE
{
    public static class SIFUtils
    {
        private static readonly KeyMap<char, SCEColor> _sifMap = new()
        {
            { 'K', SCEColor.Black },
            { 'B', SCEColor.DarkBlue },
            { 'G', SCEColor.DarkGreen },
            { 'C', SCEColor.DarkCyan },
            { 'R', SCEColor.DarkRed },
            { 'M', SCEColor.DarkMagenta },
            { 'Y', SCEColor.DarkYellow },
            { 's', SCEColor.Gray },
            { 'S', SCEColor.DarkGray },
            { 'b', SCEColor.Blue },
            { 'g', SCEColor.Green },
            { 'c', SCEColor.Cyan },
            { 'r', SCEColor.Red },
            { 'm', SCEColor.Magenta },
            { 'y', SCEColor.Yellow },
            { 'W', SCEColor.White },
            { 'N', SCEColor.Transparent },
        };

        #region Conversion

        public static string ToSIF(Grid2D<Pixel> grid)
        {
            var size = grid.Size();
            var elementArr = new char[size];
            var fgArr = new char[size];
            var bgArr = new char[size];

            int i = 0;
            for (int y = 0; y < grid.Height; ++y)
            {
                for (int x = 0; x < grid.Width; ++x)
                {
                    elementArr[i] = grid[x, y].Element;
                    fgArr[i] = ToSIFCode(grid[x, y].FgColor);
                    bgArr[i] = ToSIFCode(grid[x, y].BgColor);
                    ++i;
                }
            }

            var elementComp = Utils.RLCompress(new(elementArr));
            var fgComp = Utils.RLCompress(new(fgArr));
            var bgComp = Utils.RLCompress(new(bgArr));

            return Format(grid.Width, grid.Height, fgComp, bgComp, elementComp);
        }

        public static Grid2D<Pixel> ToGrid2D(string sif)
        {
            DeformatMetadata(sif, out int width, out int height);

            Grid2D<Pixel> grid = new(width, height);

            DeformatData(sif, out var fgColors, out var bgColors, out var elements);

            var fgDecomp = Utils.RLDecompress(fgColors);
            var bgDecomp = Utils.RLDecompress(bgColors);
            var elementDecomp = Utils.RLDecompress(elements);

            int i = 0;
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    grid[x, y] = new(elementDecomp[i], ToSCEColor(fgDecomp[i]), ToSCEColor(bgDecomp[i]));
                    ++i;
                }
            }

            return grid;
        }

        public static bool TryToGrid2D(string sif, [NotNullWhen(true)] out Grid2D<Pixel>? grid)
        {
            try
            {
                grid = ToGrid2D(sif);
                return true;
            }
            catch
            {
                grid = null;
                return false;
            }
        }

        public static DisplayMap ToDisplayMap(string sif)
        {
            return new(ToGrid2D(sif));
        }

        public static bool TryToDisplayMap(string sif, [NotNullWhen(true)] out DisplayMap? dpMap)
        {
            try
            {
                dpMap = ToDisplayMap(sif);
                return true;
            }
            catch
            {
                dpMap = null;
                return false;
            }
        }

        #endregion

        #region Formatting

        public static string Format(int width, int height, string fgColors, string bgColors, string elements)
        {
            StringBuilder sb = new((width * height * 3) + 20);
            sb.AppendFormat("<SIF>({0},{1})[{2}:{3}:{4}]", width, height, fgColors, bgColors, elements);
            return sb.ToString();
        }

        public static void DeformatMetadata(string sif, out int width, out int height)
        {
            width = Convert.ToInt32(StringUtils.TakeBetweenFF(sif, '(', ','));
            height = Convert.ToInt32(StringUtils.TakeBetweenFF(sif, ',', ')'));
        }

        public static void DeformatData(string sif, out string fgColors, out string bgColors, out string elements)
        {
            fgColors = StringUtils.TakeBetweenFF(sif, '[', ':');
            bgColors = StringUtils.TakeBetweenFL(sif, ':', ':');
            elements = StringUtils.TakeBetweenLL(sif, ':', ']');
        }

        #endregion

        #region CodeConversion

        public static char ToSIFCode(SCEColor color)
        {
            return _sifMap.GetT(color);
        }

        public static SCEColor ToSCEColor(char sifCode)
        {
            return _sifMap.GetU(sifCode);
        }

        #endregion
    }
}
