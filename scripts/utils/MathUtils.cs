namespace SCE
{
    public class MathUtils
    {
        public static int ClosestHigherMultiple(int num, int multiplier)
        {
            int modulus = num % multiplier;
            return modulus == 0 ? num : multiplier - modulus + num;
        }

        public static int Mod(int a, int b)
        {
            int mod = a % b;
            return mod < 0 ? mod + b : mod;
        }

        public static int Cycle(Vector2Int range, int newValue)
        {
            if (range.Y <= range.X)
            {
                throw new ArgumentException("Range is invalid.");
            }

            return range.X + Mod(newValue, range.Y - range.X);
        }

        public static int CutShift(Vector2Int range, int current, int shift)
        {
            range.Expose(out int min, out int max);

            int result = current + shift;

            return range.InRange(result) ? result : (result >= max ? min : max - 1);
        }
    }
}
