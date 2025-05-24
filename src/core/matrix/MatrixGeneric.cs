using CSUtils;
using System.Text;

namespace SCE
{
    internal class MatrixGeneric<T> where T : notnull
    {
        private readonly T[,] _data;

        public MatrixGeneric(T[,] data)
        {
            _data = data;
        }

        public MatrixGeneric(int rows, int columns)
            : this(new T[rows, columns])
        {
        }

        public int Rows { get => _data.GetLength(0); }

        public int Columns { get => _data.GetLength(1); }

        public T this[int row, int col]
        {
            get => _data[row, col];
            set => _data[row, col] = value;
        }

        public static MatrixGeneric<T> operator *(MatrixGeneric<T> m, T scale) => m.ScaleMultiply(scale);

        public static MatrixGeneric<T> operator *(MatrixGeneric<T> m1, MatrixGeneric<T> m2) => m1.MatrixMultiply(m2);

        public MatrixGeneric<T> ScaleMultiply(T scale)
        {
            MatrixGeneric<T> m = new(Rows, Columns);
            for (int row = 0; row < Rows; ++row)
                for (int col = 0; col < Columns; ++col)
                    m[row, col] = Multiply(this[row, col], scale);
            return m;
        }

        public MatrixGeneric<T> MatrixMultiply(MatrixGeneric<T> other)
        {
            if (Columns != other.Rows)
                throw new ArgumentException("Matrix is incompatible.");
            MatrixGeneric<T> m = new(Rows, other.Columns);
            for (int row = 0; row < m.Rows; ++row)
            {
                for (int col = 0; col < m.Columns; ++col)
                {
                    T sum = default(T) ?? throw new NullReferenceException("Default value is null.");
                    for (int i = 0; i < m.Rows; ++i)
                        sum = Add(sum, Multiply(this[row, i], other[i, col]));
                    m[row, col] = sum;
                }
            }
            return m;
        }

        protected virtual T Add(T a, T b)
        {
            throw new NotImplementedException("Add function undefined.");
        }

        protected virtual T Multiply(T a, T b)
        {
            throw new NotImplementedException("Multiply function undefined.");
        }

        public string BuildFlat()
        {
            return Utils.BuildGridFlat(_data);
        }

        public string Build2D()
        {
            return Utils.BuildGrid2D(_data);
        }

        public override string ToString()
        {
            return BuildFlat();
        }
    }
}
