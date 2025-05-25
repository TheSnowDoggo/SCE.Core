namespace SCE
{
    public class MatrixI : MatrixGeneric<int>
    {
        public MatrixI(int rows, int columns)
            : base(rows, columns)
        {}

        public MatrixI(int[,] data)
            : base(data)
        {}

        public static explicit operator MatrixI(Vector2Int v) => ToMatrix(v);

        public static Vector2Int operator *(MatrixI m, Vector2Int v) => ((MatrixI)(m * ToMatrix(v))).ToVector2Int();

        public static MatrixI ToMatrix(Vector2Int v) { return new(new int[,] { { v.Y, v.X } }); }

        public Vector2Int ToVector2Int()
        {
            if (Rows != 2 || Columns != 1)
                throw new ArgumentException("Matrix size invalid.");
            return new(this[0, 2], this[0, 1]);
        }

        protected override int Add(int a, int b) { return a + b; }

        protected override int Multiply(int a, int b) { return a * b; }
    }
}
