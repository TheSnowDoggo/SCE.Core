namespace SCE
{
    using System.Text;

    public static class StringUtils
    {
        #region RunLengthEncoding

        public static string RLCompress(string str, char seperator = '§')
        {
            if (str.Length == 0)
                return str;

            StringBuilder strBuilder = new(str.Length);
            char last = str[0];
            int count = 1;
            for (int i = 1; i <= str.Length; ++i)
            {
                char chr = i < str.Length ? str[i] : (char)(str[^1] + 1);
                if (chr == last)
                    ++count;
                else
                {
                    if (count > 1)
                        strBuilder.Append(count);
                    strBuilder.Append(char.IsDigit(last) ? $"{seperator}{last}{seperator}" : last.ToString());

                    last = chr;
                    count = 1;
                }
            }
            return strBuilder.ToString();
        }

        public static string RLDecompress(string str, char seperator = '§')
        {
            StringBuilder strBuilder = new(str.Length);
            StringBuilder digitBuilder = new();
            bool inDigit = false;
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] == seperator)
                {
                    inDigit = !inDigit;
                    continue;
                }

                if (!inDigit && char.IsDigit(str[i]))
                    digitBuilder.Append(str[i]);
                else if (digitBuilder.Length > 0)
                {
                    strBuilder.Append(Copy(str[i], Convert.ToInt32(digitBuilder.ToString())));
                    digitBuilder.Clear();
                }
                else
                {
                    strBuilder.Append(str[i]);
                }
            }
            return strBuilder.ToString();
        }

        #endregion

        #region ContainsLessThan

        public static bool ContainsLessThan(string str, char of, int amount)
        {
            int count = 0;
            foreach (var c in str)
            {
                if (c == of && ++count >= amount)
                    return false;
            }
            return true;
        }

        #endregion

        #region Substring

        public static int LongestSubstringBetween(string str, char seperator)
        {
            int longest = 0;
            int length = 0;
            for (int i = 0;  i < str.Length; ++i)
            {
                if (str[i] != seperator)
                    ++length;
                else
                {
                    if (length > longest)
                        longest = length;
                    length = 0;
                    if (str.Length - i < longest)
                        break;
                }
            }
            return length > longest ? length : longest;
        }

        #endregion

        #region Matching

        public static bool Matches(string entry, string suggestion)
        {
            int matching = MatchingCharacters(entry, suggestion);
            if (matching == entry.Length || (matching == suggestion.Length && entry[matching] == ' '))
                return true;
            return false;
        }

        public static int MatchingCharacters(string str1, string str2)
        {
            int length = Math.Min(str1.Length, str2.Length);
            int matching = 0;
            for (int i = 0; i < length; ++i)
            {
                if (str1[i] == str2[i])
                    matching++;
                else
                    break;
            }
            return matching;
        }

        #endregion

        #region Array

        public static void TrimFirst(ref string[] arr)
        {
            if (arr.Length == 0)
                return;
            string[] temp = new string[arr.Length - 1];
            for (int i = 0; i < temp.Length; ++i)
                temp[i] = arr[i + 1];
            arr = temp;
        }

        public static char[] StringArrayToCharArray(string[] strArray)
        {
            char[] chrArray = new char[strArray.Length];

            for (int i = 0; i < strArray.Length; i++)
            {
                string str = strArray[i];

                if (str.Length != 1)
                    throw new InvalidOperationException("String contains multiple characters.");

                chrArray[i] = str[0];
            }

            return chrArray;
        }

        #endregion

        #region Merge

        public static string MergeString(string mainStr, string underLayedStr, char mergeChr = ' ')
        {
            if (underLayedStr is null)
                return mainStr;
            if (underLayedStr.Length < mainStr.Length)
                throw new ArgumentException("Under layed string length cannot be less than main string.");

            StringBuilder strBuilder = new(underLayedStr.Length);
            for (int i = 0; i < mainStr.Length; i++)
                strBuilder.Append(mainStr[i] != mergeChr ? mainStr[i] : underLayedStr[i]);
            return strBuilder.ToString();
        }

        #endregion

        #region PluralSwitch

        public static string PlrSwt(int quantity, string singular = "", string plural = "s")
        {
            return quantity == 1 ? singular : plural;
        }

        #endregion

        #region Reverse

        public static string Reverse(string str)
        {
            char[] charArr = new char[str.Length];
            for (int i = 0; i < charArr.Length; ++i)
                charArr[i] = str[str.Length - i - 1];
            return new(charArr);
        }

        #endregion

        #region Timespan

        public static string TimeSpanToString(TimeSpan timeSpan)
        {
            StringBuilder strBuilder = new();

            if (timeSpan.Days > 0)
                strBuilder.Append(timeSpan.Days + " day" + PlrSwt(timeSpan.Days) + ' ');
            if (timeSpan.Hours > 0)
                strBuilder.Append(timeSpan.Hours + " hour" + PlrSwt(timeSpan.Hours) + ' ');
            if (timeSpan.Minutes > 0)
                strBuilder.Append(timeSpan.Minutes + " minute" + PlrSwt(timeSpan.Minutes) + ' ');
            if (timeSpan.Seconds > 0)
                strBuilder.Append(timeSpan.Seconds + " second" + PlrSwt(timeSpan.Seconds) + ' ');

            if (strBuilder.Length > 0)
                strBuilder.Remove(strBuilder.Length - 1, 1);

            return strBuilder.ToString();
        }

        #endregion

        #region PadTo
        public static string PadAfterToEven(string str)
        {
            return str.Length % 2 == 0 ? str : $"{str} ";
        }

        public static string PadBeforeToEven(string str)
        {
            return str.Length % 2 == 0 ? str : $" {str}";
        }
        #endregion

        #region FitToLength
        private static string FitToLength(bool postFit, string str, int length, char fill = ' ')
        {
            if (length < 0)
                throw new ArgumentException("Length cannot be less than 0.");

            int difference = length - str.Length;
            switch (difference)
            {
                case 0:  return str;
                case >0: return postFit ? str + Copy(fill, difference) : Copy(fill, difference) + str;
                case <0: return str[..length];
            }
        }

        public static string PostFitToLength(string str, int length, char fill = ' ')
        {
            return FitToLength(true, str, length, fill);
        }

        public static string PreFitToLength(string str, int length, char fill = ' ')
        {
            return FitToLength(false, str, length, fill);
        }
        #endregion

        #region TakeBetween
        public static string TakeBetweenFF(string str, char leftBound, char rightBound)
        {
            return str[(str.IndexOf(leftBound) + 1)..str.IndexOf(rightBound)];
        }

        public static string TakeBetweenFL(string str, char leftBound, char rightBound)
        {
            return str[(str.IndexOf(leftBound) + 1)..str.LastIndexOf(rightBound)];
        }

        public static string TakeBetweenLF(string str, char leftBound, char rightBound)
        {
            return str[(str.LastIndexOf(leftBound) + 1)..str.IndexOf(rightBound)];
        }

        public static string TakeBetweenLL(string str, char leftBound, char rightBound)
        {
            return str[(str.LastIndexOf(leftBound) + 1)..str.LastIndexOf(rightBound)];
        }

        #endregion

        #region Bounds

        public static string InsertBetween(string str, string insert, char leftBound, char rightBound)
        {
            RangeBetween(str, leftBound, rightBound).Expose(out int leftIndex, out int rightIndex);
            return str.Remove(leftIndex, rightIndex - leftIndex).Insert(leftIndex, insert);
        }

        public static string InsertBetween(string str, string insert, char bound)
        {
            return InsertBetween(str, insert, bound, bound);
        }

        public static Vector2Int RangeBetween(string str, char leftBound, char rightBound)
        {
            int leftIndex = str.IndexOf(leftBound) + 1;

            if (leftIndex == -1)
                throw new ArgumentException("Left bound not found.");

            int rightIndex = str.IndexOf(rightBound, leftIndex);

            if (rightIndex == -1)
                throw new ArgumentException("Right bound not found.");

            return new(leftIndex, rightIndex);
        }

        public static Vector2Int RangeBetween(string str, char bound)
        {
            return RangeBetween(str, bound);
        }

        public static string[] SplitExcludingBounds(string str, char split, char[] leftBoundArray, char[] rightBoundArray)
        {
            List<string> stringList = new();

            StringBuilder strBuilder = new();

            int boundLayer = 0;

            for (int i = 0; i < str.Length; ++i)
            {
                bool last = i == str.Length - 1;

                char chr = str[i];

                if (leftBoundArray.Contains(chr))
                    boundLayer++;
                else if (boundLayer > 0 && rightBoundArray.Contains(chr))
                    boundLayer--;

                if (boundLayer > 0 || chr != split)
                    strBuilder.Append(chr);

                if (last || (boundLayer == 0 && chr == split))
                {
                    stringList.Add(strBuilder.ToString());

                    strBuilder.Clear();
                }
            }

            return stringList.ToArray();
        }

        public static string[] SplitExcludingBounds(string str, char split, char[] boundArray)
        {
            return SplitExcludingBounds(str, split, boundArray, boundArray);
        }

        public static string[] SplitExcludingBounds(string str, char split, char leftBound, char rightBound)
        {
            return SplitExcludingBounds(str, split, new[] { leftBound }, new[] { rightBound });
        }
        #endregion

        #region StringCharManipulation
        public static int CountOf(string str, char countChr)
        {
            int total = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == countChr)
                    total++;
            }
            return total;
        }

        public static string RemoveInstancesOf(string str, char chr)
        {
            StringBuilder strBuilder = new(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != chr)
                    strBuilder.Append(str[i]);
            }
            return strBuilder.ToString();
        }
        #endregion

        #region LineSplitting
        public static string[] BasicSplitLineArray(string str, int maxLines)
        {
            string[] lineArray = str.Split('\n');
            if (lineArray.Length > maxLines)
                Array.Resize(ref lineArray, maxLines);
            return lineArray;
        }

        public static string[] SmartSplitLineArray(string str, int maxLineLength, int maxLines)
        {
            if (maxLineLength < 0)
                throw new ArgumentException("Max line length cannot be less than 0.");
            if (maxLines < 0)
                throw new ArgumentException("Max lines cannot be less than 0.");

            List<string> lineList = new();

            StringBuilder strBuilder = new();

            int i = 0;
            do
            {
                bool last = i == str.Length - 1;

                char chr = str[i];

                if (chr != '\n')
                    strBuilder.Append(chr);

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
        #endregion

        #region Copy
        public static string Copy(string str, int copies)
        {
            if (copies < 0)
                throw new ArgumentException("Copies cannot be less than 0.");
            if (copies == 1)
                return str;
            StringBuilder strBuilder = new(str.Length * copies);
            for (int i = 0; i < copies; i++)
                strBuilder.Append(str);
            return strBuilder.ToString();
        }

        public static string Copy(char chr, int copies)
        {
            if (copies < 0)
                throw new ArgumentException("Copies cannot be less than 0.");
            if (copies == 1)
                return chr.ToString();
            char[] chrArr = new char[copies];
            for (int i = 0; i < copies; ++i)
                chrArr[i] = chr;
            return new(chrArr);
        }
        #endregion
    }
}
