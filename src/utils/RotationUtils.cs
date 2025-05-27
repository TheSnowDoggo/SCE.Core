namespace SCE
{
    public static class RotationUtils
    {
        public const float RADIAN_EULAR_CONVERSION_FACTOR = 180 / MathF.PI;

        #region Conversion

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

        #region Rotation

        public static Vector2 Rotate90CW(Vector2 pos)
        {
            return new(pos.Y * -1.0f, pos.X);
        }

        public static Vector2 Rotate90CW(Vector2 pos, Vector2 axis)
        {
            return Rotate90CW(pos - axis) + axis;
        }

        public static Vector2 Rotate90ACW(Vector2 pos)
        {
            return new(pos.Y, pos.X * -1.0f);
        }

        public static Vector2 Rotate90ACW(Vector2 pos, Vector2 axis)
        {
            return Rotate90ACW(pos - axis) + axis;
        }

        public static Vector2 Rotate180(Vector2 pos)
        {
            return pos * -1.0f;
        }

        public static Vector2 Rotate180(Vector2 pos, Vector2 axis)
        {
            return Rotate180(pos - axis) + axis;
        }

        #endregion
    }
}
