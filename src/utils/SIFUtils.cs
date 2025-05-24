using CSUtils;
using System.Diagnostics.CodeAnalysis;
using System.Text;
namespace SCE
{
    public static class SIFUtils
    {
        public static readonly KeyMap<char, SCEColor> _sifMap = new()
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
            var elementArr = new char[grid.Size()];
            var fgArr = new char[grid.Size()];
            var bgArr = new char[grid.Size()];

            int i = 0;
            for (int y = grid.Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < grid.Width; ++x)
                {
                    elementArr[i] = grid[x, y].Element;
                    fgArr[i] = ToSIFCode(grid[x, y].FgColor);
                    bgArr[i] = ToSIFCode(grid[x, y].BgColor);
                    ++i;
                }
            }

            string elementComp = Utils.RLCompress(new(elementArr));
            string fgComp = Utils.RLCompress(new(fgArr));
            string bgComp = Utils.RLCompress(new(bgArr));

            return Format(grid.Width, grid.Height, fgComp, bgComp, elementComp);
        }

        public static Grid2D<Pixel> ToGrid2D(string sif)
        {
            DeformatMetadata(sif, out int width, out int height);

            Grid2D<Pixel> grid = new(width, height);

            DeformatData(sif, out string fgColors, out string bgColors, out string elements);

            string fgDecomp = Utils.RLDecompress(fgColors);
            string bgDecomp = Utils.RLDecompress(bgColors);
            string elementDecomp = Utils.RLDecompress(elements);

            int i = 0;
            for (int y = height - 1; y >= 0; --y)
            {
                for (int x = 0; x < width; ++x)
                {
                    grid[x, y] = new Pixel(elementDecomp[i], ToSCEColor(fgDecomp[i]), ToSCEColor(bgDecomp[i]));
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
