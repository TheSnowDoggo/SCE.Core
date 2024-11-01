namespace SCECore.Utils
{
    public class SCEMath
    {
        public static int ClosestHigherMultiple(int num, int multiplier)
        {
            int modulus = num % multiplier;
            return modulus == 0 ? num : multiplier - modulus + num;
        }

        public static int CutShift(Vector2Int range, int current, int shift)
        {
            range.Expose(out int min, out int max);

            int result = current + shift;

            return range.InRange(result) ? result : (result >= max ? min : max - 1);
        }
    }
}
