using System.Text;
namespace SCE
{
    public static class StringUtils
    {
        public static bool ContainsLessThan(string str, char of, int amount)
        {
            int count = 0;
            foreach (var c in str)
                if (c == of && ++count >= amount)
                    return false;
            return true;
        }

        public static int LongestSubstringBetween(string str, char seperator)
        {
            int longest = 0;
            int length = 0;
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] != seperator)
                {
                    ++length;
                }
                else
                {
                    if (length > longest)
                    {
                        longest = length;
                    }
                    length = 0;
                    if (str.Length - i < longest)
                    {
                        break;
                    }
                }
            }
            return length > longest ? length : longest;
        }

        public static string RemoveInstancesOf(string str, char chr)
        {
            StringBuilder sb = new(str.Length);
            for (int i = 0; i < str.Length; i++)
                if (str[i] != chr)
                    sb.Append(str[i]);
            return sb.ToString();
        }

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
                {
                    boundLayer++;
                }
                else if (boundLayer > 0 && rightBoundArray.Contains(chr))
                {
                    boundLayer--;
                }

                if (boundLayer > 0 || chr != split)
                {
                    strBuilder.Append(chr);
                }

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
    }
}
