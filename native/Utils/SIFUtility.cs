namespace SCECore.Utils
{
    using System.Text;

    public static class SIFUtility
    {
        private const char DigitCountDefine = '|';

        // Code library
        private static readonly ValueMap<byte, char> SifCodeLibrary = new()
        {
            { 0, 'K' },  // Black
            { 1, 'B' },  // DarkBlue
            { 2, 'G' },  // DarkGreen
            { 3, 'C' },  // DarkCyan
            { 4, 'R' },  // DarkRed
            { 5, 'M' },  // DarkMagenta
            { 6, 'Y' },  // DarkYellow
            { 7, 's' },  // Gray
            { 8, 'S' },  // DarkGray
            { 9, 'b' },  // Blue
            { 10, 'g' }, // Green
            { 11, 'c' }, // Cyan
            { 12, 'r' }, // Red
            { 13, 'm' }, // Magenta
            { 14, 'y' }, // Yellow
            { 15, 'W' }, // White
            { 16, 'N' }, // Transparent
        };

        // Convert to SIF
        public static string Format(DisplayMap displayMap)
        {
            string[] formatArray = ToGridData(displayMap);
            return string.Format("<SIF>[({0}){1}:{2}:{3}]", displayMap.Dimensions, formatArray[1], formatArray[2], formatArray[0]);
        }

        public static string[] BuildPixelGrid(Grid2D<Pixel> pixelGrid)
        {
            StringBuilder textBuild = new();

            StringBuilder fgColorBuild = new();

            StringBuilder bgColorBuild = new();

            for (int y = pixelGrid.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < pixelGrid.Width; x++)
                {
                    Pixel pixel = pixelGrid[x, y];
                    string text = pixel.Element ?? string.Empty;

                    textBuild.Append(SCEString.FitToLength(text, Pixel.PIXELWIDTH));

                    fgColorBuild.Append(SifCodeLibrary[pixel.FgColor]);

                    bgColorBuild.Append(SifCodeLibrary[pixel.BgColor]);
                }
            }

            return new[] { textBuild.ToString(), fgColorBuild.ToString(), bgColorBuild.ToString() };
        }

        public static string[] ToGridData(Grid2D<Pixel> pixelGrid)
        {
            return RLCompress(BuildPixelGrid(pixelGrid));
        }

        // Convert from SIF
        public static DisplayMap ToDisplayMap(string sif)
        {
            string data, fgData, bgData, textData;

            data = GetGridData(sif);

            int firstDiv = data.IndexOf(':'), lastDiv = data.IndexOf(':', firstDiv + 1);

            fgData = data[..firstDiv];
            bgData = data[(firstDiv + 1)..lastDiv];
            textData = data[(lastDiv + 1)..];

            Vector2Int dimensions = GetSIFDimensions(sif);

            return new(ToTextGrid(textData, dimensions), ToColorGrid(fgData, dimensions), ToColorGrid(bgData, dimensions));
        }

        public static Grid2D<byte> ToColorGrid(string gridData, Vector2Int dimensions)
        {
            Grid2D<byte> colorGrid = new(dimensions);

            string rawGridData = RLDecompress(gridData, false);
            int index = 0;

            for (int y = colorGrid.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < colorGrid.Width; x++)
                {
                    char code = rawGridData[index];

                    if (SifCodeLibrary.Contains(code))
                    {
                        colorGrid[x, y] = SifCodeLibrary[code];
                    }
                    else
                    {
                        x--;
                    }

                    index++;
                }
            }

            return colorGrid;
        }

        public static Grid2D<string> ToTextGrid(string gridData, Vector2Int dimensions)
        {
            Grid2D<string> textGrid = new(dimensions);

            string rawGridData = RLDecompress(gridData, true);
            int lengthDif = (dimensions.ScalarProduct * 2) - rawGridData.Length;

            if (lengthDif > 0)
            {
                rawGridData += SCEString.Copy(' ', lengthDif);
            }

            int index = 0;
            for (int y = textGrid.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < textGrid.Width; x++)
                {
                    textGrid[x, y] = rawGridData[index..(index + Pixel.PIXELWIDTH)];
                    index += Pixel.PIXELWIDTH;
                }
            }

            return textGrid;
        }

        // Compression functions      
        public static string RLCompress(string input, bool containedCount)
        {
            StringBuilder compressed = new();

            int count = 0;
            char lastChr = input[0];

            for (int i = 0; i < input.Length; i++)
            {
                char chr = input[i];
                bool last = i == input.Length - 1;

                if (chr == lastChr)
                {
                    count++;
                }

                if (chr != lastChr || last)
                {
                    string countStr = string.Empty;

                    if (count > 1)
                    {
                        countStr = containedCount ? $"{DigitCountDefine}{count}{DigitCountDefine}" : count.ToString();
                    }

                    compressed.Append(countStr + lastChr);

                    if (chr != lastChr && last)
                    {
                        compressed.Append(chr);
                    }

                    lastChr = chr;
                    count = 1;
                }
            }

            return compressed.ToString();
        }

        public static string[] RLCompress(string[] input)
        {
            string[] compressed = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                compressed[i] = RLCompress(input[i], i == 0);
            }

            return compressed;
        }

        // Decompression functions
        public static string RLDecompress(string input, bool containedCount)
        {
            StringBuilder strBuild = new();
            StringBuilder strDigitBuild = new();

            bool buildingDigit = false;

            foreach (char chr in input)
            {
                if (containedCount)
                {
                    if (chr == DigitCountDefine)
                    {
                        buildingDigit = !buildingDigit;
                    }
                }
                else
                {
                    buildingDigit = char.IsDigit(chr);
                }

                if (!containedCount || chr != DigitCountDefine)
                {
                    if (!buildingDigit)
                    {
                        int count = strDigitBuild.Length != 0 ? Convert.ToInt32(strDigitBuild.ToString()) : 1;

                        strBuild.Append(SCEString.Copy(chr, count));

                        strDigitBuild.Clear();
                    }
                    else if (char.IsDigit(chr))
                    {
                        strDigitBuild.Append(chr);
                    }
                }
            }

            return strBuild.ToString();
        }

        public static string[] RLDecompress(string[] input)
        {
            string[] decompressed = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                decompressed[i] = RLDecompress(input[i], i == 0);
            }

            return decompressed;
        }

        // Get data functions
        public static string GetSIFData(string sif)
        {
            return sif[sif.IndexOf('[')..sif.IndexOf(']')];
        }

        public static string GetGridData(string sif)
        {
            string sifData = GetSIFData(sif);

            return sifData[(sifData.IndexOf(')') + 1)..];
        }

        public static Vector2Int GetSIFDimensions(string sif)
        {
            string sifData = GetSIFData(sif);

            return Vector2Int.ReadVectorString(sifData[(sifData.IndexOf('(') + 1)..sifData.IndexOf(')')]);
        }
    }
}
