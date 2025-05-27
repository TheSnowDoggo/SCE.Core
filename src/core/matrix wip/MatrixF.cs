using CSUtils;

namespace SCE
{
    public class MatrixF
    {
        #region Reflect

        public static MatrixF ReflectXAxis { get; } = new(new[,]
        {
            { +1.0f, +0.0f },
            { +0.0f, -1.0f },
        });

        public static MatrixF ReflectYAxis { get; } = new(new[,]
        {
            { -1.0f, +0.0f },
            { +0.0f, +1.0f },
        });

        public static MatrixF ReflectYX { get; } = new(new[,]
        {
            { +0.0f, +1.0f },
            { +1.0f, +0.0f },
        });

        public static MatrixF ReflectYMX { get; } = new(new[,]
        {
            { +0.0f, -1.0f },
            { -1.0f, +0.0f },
        });

        #endregion

        #region Rotation

        /// <summary>
        /// Clockwise 90 rotation (when Y value increases downwards)
        /// </summary>
        public static MatrixF RotateCW90 { get; } = new(new[,]
        {
            { +0.0f, -1.0f },
            { +1.0f, +0.0f },
        });
        
        /// <summary>
        /// Anti-clockwise 90 rotation (when Y value increases downwards)
        /// </summary>
        public static MatrixF RotateACW90 { get; } = new(new[,]
        {
            { +0.0f, +1.0f },
            { -1.0f, +0.0f },
        });

        public static MatrixF Rotate180 { get; } = new(new[,]
        {
            { -1.0f, +0.0f },
            { +0.0f, -1.0f },
        });

        #endregion

        private readonly float[,] _data;

        public MatrixF(float[,] data)
        {
            _data = data;
        }

        public MatrixF(int rows, int columns)
            : this(new float[rows, columns])
        {
        }

        public int Rows { get => _data.GetLength(0); }

        public int Columns { get => _data.GetLength(1); }

        public float this[int row, int col]
        {
            get => _data[row, col];
            set => _data[row, col] = value;
        }

        public static explicit operator MatrixF(Vector2 v) => ToMatrix(v);

        public static MatrixF operator *(MatrixF m, float scale) => m.ScaleMultiply(scale);

        public static MatrixF operator *(MatrixF m1, MatrixF m2) => m1.MatrixMultiply(m2);

        public static Vector2 operator *(MatrixF m, Vector2 v) => (m * ToMatrix(v)).ToVector2();

        public static MatrixF ScaleMatrix(int size, float scale = 1.0f)
        {
            MatrixF m = new(size, size);
            for (int i = 0; i < size; ++i)
                m[i, i] = scale;
            return m;
        }

        public static MatrixF ToMatrix(Vector2 v)
        {
            return new(new float[,] { { v.X }, { v.Y } });
        }

        public static Vector2 RotateAboutCW90(Vector2 pos, Vector2 axis)
        {
            return (RotateCW90 * (pos - axis)) + axis;
        }

        public static Vector2 RotateAboutACW90(Vector2 pos, Vector2 axis)
        {
            return (RotateACW90 * (pos - axis)) + axis;
        }

        public static Vector2 RotateAbout180(Vector2 pos, Vector2 axis)
        {
            return (Rotate180 * (pos - axis)) + axis;
        }

        public Vector2 ToVector2()
        {
            if (Rows != 2 || Columns != 1)
                throw new ArgumentException("Matrix size invalid.");
            return new(this[0, 0], this[1, 0]);
        }

        public MatrixF ScaleMultiply(float scale)
        {
            MatrixF m = new(Rows, Columns);
            for (int row = 0; row < Rows; ++row)
                for (int col = 0; col < Columns; ++col)
                    m[row, col] = this[row, col] * scale;
            return m;
        }

        public MatrixF MatrixMultiply(MatrixF other)
        {
            if (Columns != other.Rows)
                throw new ArgumentException("Matrix is incompatible.");
            MatrixF m = new(Rows, other.Columns);
            for (int row = 0; row < m.Rows; ++row)
            {
                for (int col = 0; col < m.Columns; ++col)
                {
                    float sum = 0.0f;
                    for (int i = 0; i < m.Rows; ++i)
                        sum += this[row, i] * other[i, col];
                    m[row, col] = sum;
                }
            }
            return m;
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
