using CSUtils;

namespace SCE
{
    public static class RotateUtils
    {
        #region Conversion

        public static Vector2 AngleToVec(float angle)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle)).SafeNormalize();
        }

        public static float VecToRad(Vector2 vector)
        {
            vector = vector.SafeNormalize();
            var angle = MathF.Atan(vector.Y / vector.X);
            return vector.X > 0 ? angle : angle + MathF.PI;
        }

        public static float VecToDeg(Vector2 vector)
        {
            return Utils.RadToDeg(VecToRad(vector));
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
