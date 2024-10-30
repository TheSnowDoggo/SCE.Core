namespace SCECore.Utils
{
    using System.Text;

    public static class SCEString
    {
        public static string PadToEven(string str)
        {
            return str + (str.Length % 2 != 0 ? " " : string.Empty);
        }

        public static string FitToLength(string str, int length, char fill = ' ')
        {
            if (length < 0)
            {
                throw new ArgumentException("Length cannot be less than 0.");
            }

            int difference = length - str.Length;
            return difference switch
            {
                0 => str,
                > 0 => str += Copy(fill, difference),
                < 0 => str[..length]
            };
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
        public static string[] BasicSplitLineArray(string write, int maxLines)
        {
            string[] lineArray = write.Split('\n');

            if (lineArray.Length > maxLines)
            {
                Array.Resize(ref lineArray, maxLines);
            }

            return lineArray;
        }

        public static string[] SmartSplitLineArray(string write, int maxLineLength, int maxLines)
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
                bool last = i == write.Length - 1;

                char chr = write[i];

                if (chr != '\n')
                {
                    strBuilder.Append(chr);
                }

                if (chr == '\n' || last || (strBuilder.Length == maxLineLength && write[i + 1] != '\n'))
                {
                    lineList.Add(strBuilder.ToString());

                    strBuilder.Clear();
                }

                i++;
            }
            while (i < write.Length && lineList.Count < maxLines);

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
            if (copies < 0)
            {
                throw new ArgumentException("Copies cannot be less than 0.");
            }

            StringBuilder strBuilder = new(copies);

            for (int i = 0; i < copies; i++)
            {
                strBuilder.Append(chr);
            }

            return strBuilder.ToString();
        }
    }
}
