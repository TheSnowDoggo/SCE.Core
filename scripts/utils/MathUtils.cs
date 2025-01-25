namespace SCE
{
    public class MathUtils
    {
        private const float RADIAN_EULAR_CONVERSION_FACTOR = 180 / MathF.PI;

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

        #region Angles
        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle)).Normalize();
        }

        public static float VectorToRadians(Vector2 vector)
        {
            return MathF.Tan(vector.Y / vector.X);
        }

        public static float VectorToDegrees(Vector2 vector)
        {
            return RadiansToDegrees(VectorToRadians(vector));
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * RADIAN_EULAR_CONVERSION_FACTOR;
        }

        public static float DegreesToRadians(float degrees)
        {
            return degrees / RADIAN_EULAR_CONVERSION_FACTOR;
        }
        #endregion

        #region Cycle
        public static int Cycle(Vector2Int range, int newValue)
        {
            if (range.Y <= range.X)
                throw new ArgumentException("Range is invalid.");
            return range.X + Mod(newValue, range.Y - range.X);
        }

        public static int CutShift(Vector2Int range, int current, int shift)
        {
            range.Expose(out int min, out int max);

            int result = current + shift;

            return range.InRange(result) ? result : (result >= max ? min : max - 1);
        }
        #endregion
    }
}
