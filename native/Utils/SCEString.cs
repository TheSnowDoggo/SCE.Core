namespace SCECore.Utils
{
    using System.Text;

    public static class SCEString
    {
        public static string PadAfterToEven(string str)
        {
            return str.Length % 2 == 0 ? str : $"{str} ";
        }

        public static string PadBeforeToEven(string str)
        {
            return str.Length % 2 == 0 ? str : $" {str}";
        }

        public static string FitToLength(string str, int length, char fill = ' ')
        {
            if (length < 0)
            {
                throw new ArgumentException("Length cannot be less than 0.");
            }

            int dif = length - str.Length;

            return dif switch
            {
                0 => str,
                > 0 => str + Copy(fill, dif),
                < 0 => str[..length]
            };
        }

        public static string FormatNumberToLength(string numStr, int digitLength, char digitFill = '0')
        {
            if (digitLength < 0)
            {
                throw new ArgumentException("Length cannot be less than 0.");
            }

            int dif = digitLength - numStr.Length;

            return dif switch
            {
                0 => numStr,
                > 0 => Copy(digitFill, dif) + numStr,
                < 0 => throw new InvalidOperationException("Number exceeds digit length.")
            };
        }

        public static string MergeString(string mainStr, string underLayedStr, char mergeChr = ' ')
        {
            if (underLayedStr.Length < mainStr.Length)
            {
                throw new ArgumentException("Under layed string length cannot be less than main string.");
            }

            StringBuilder strBuilder = new();

            for (int i = 0; i < mainStr.Length; i++)
            {
                strBuilder.Append(mainStr[i] != mergeChr ? mainStr[i] : underLayedStr[i]);
            }

            return strBuilder.ToString();
        }

        public static int CountOf(string str, char countChr)
        {
            int total = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == countChr)
                {
                    total++;
                }
            }

            return total;
        }

        public static string RemoveInstancesOf(string str, char chr)
        {
            StringBuilder strBuilder = new();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != chr)
                {
                    strBuilder.Append(str[i]);
                }
            }

            return strBuilder.ToString();
        }

        // Line split functions
        public static string[] BasicSplitLineArray(string str, int maxLines)
        {
            string[] lineArray = str.Split('\n');

            if (lineArray.Length > maxLines)
            {
                Array.Resize(ref lineArray, maxLines);
            }

            return lineArray;
        }

        public static string[] SmartSplitLineArray(string str, int maxLineLength, int maxLines)
        {
            if (maxLineLength < 0)
            {
                throw new ArgumentException("Max line length cannot be less than 0.");
            }

            if (maxLines < 0)
            {
                throw new ArgumentException("Max lines cannot be less than 0.");
            }

            List<string> lineList = new();

            StringBuilder strBuilder = new();

            int i = 0;
            do
            {
                bool last = i == str.Length - 1;

                char chr = str[i];

                if (chr != '\n')
                {
                    strBuilder.Append(chr);
                }

                if (chr == '\n' || last || (strBuilder.Length == maxLineLength && str[i + 1] != '\n'))
                {
                    lineList.Add(strBuilder.ToString());

                    strBuilder.Clear();
                }

                i++;
            }
            while (i < str.Length && lineList.Count < maxLines);

            return lineList.ToArray();
        }

        // Copy functions
        public static string Copy(string str, int copies)
        {
            if (copies < 0)
            {
                throw new ArgumentException("Copies cannot be less than 0.");
            }

            StringBuilder strBuilder = new(copies * str.Length);

            for (int i = 0; i < copies; i++)
            {
                strBuilder.Append(str);
            }

            return strBuilder.ToString();
        }

        public static string Copy(char chr, int copies)
        {
            return Copy(chr.ToString(), copies);
        }
    }
}
