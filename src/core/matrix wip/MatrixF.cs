namespace SCE
{
    public class MatrixF : MatrixGeneric<float>
    {
        public MatrixF(int rows, int columns)
            : base(rows, columns)
        {}

        public MatrixF(float[,] data)
            : base(data)
        {}

        public static explicit operator MatrixF(Vector2 v) => ToMatrix(v);

        public static Vector2 operator *(MatrixF m, Vector2 v) => ((MatrixF)(m * ToMatrix(v))).ToVector2();

        public static MatrixF ToMatrix(Vector2 v) { return new(new float[,] { { v.Y, v.X } }); }

        public Vector2 ToVector2()
        {
            if (Rows != 2 || Columns != 1)
                throw new ArgumentException("Matrix size invalid.");
            return new(this[0, 2], this[0, 1]);
        }

        protected override float Add(float a, float b) { return a + b; }

        protected override float Multiply(float a, float b) { return a * b; }
    }
}
