using CSUtils;
namespace SCE
{
    public class MathUtils
    {
        public const float RADIAN_EULAR_CONVERSION_FACTOR = 180 / MathF.PI;

        public static int ClosestHigherMultiple(int x, int m)
        {
            int r = x % m;
            return r == 0 ? x : m - r + x;
        }

        #region Angles

        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle)).Normalize();
        }

        public static float VectorToRadians(Vector2 vector)
        {
            vector = vector.Normalize();
            var angle = MathF.Atan(vector.Y / vector.X);
            return vector.X > 0 ? angle : angle + MathF.PI;
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
    }
}
