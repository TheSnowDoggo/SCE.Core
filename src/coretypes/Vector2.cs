namespace SCE
{
    /// <summary>
    /// Representation of floating point 2D vectors.
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>, IFormattable
    {
        private const char VectorStringSplitChar = ',';

        public float X;

        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Shorthands

        public static Vector2 Zero { get; } = new(+0.0f, +0.0f);

        public static Vector2 Up { get; } = new(+0.0f, -1.0f);

        public static Vector2 Down { get; } = new(+0.0f, +1.0f);

        public static Vector2 Right { get; } = new(+1.0f, +0.0f);

        public static Vector2 Left { get; } = new(-1.0f, +0.0f);

        public static Vector2 UpLeft { get; } = new(-1.0f, -1.0f);

        public static Vector2 UpRight { get; } = new(+1.0f, -1.0f);

        public static Vector2 DownLeft { get; } = new(-1.0f, +1.0f);

        public static Vector2 DownRight { get; } = new(+1.0f, +1.0f);

        #endregion

        #region Property

        public Vector2 Inverse() => new(Y, X);

        public float ScalarProduct() => X * Y;

        public Vector2 Midpoint() => new Vector2(X, Y) / 2.0f;

        public float Magnitude()
        {
            return MathF.Sqrt((X * X) + (Y * Y));
        }

        public Vector2 Normalize()
        {
            return new Vector2(X, Y) / Magnitude();
        }

        public Vector2 SafeNormalize()
        {
            var m = Magnitude();
            return m != 0 ? new Vector2(X, Y) : Zero;
        }

        public bool IsNormalized()
        {
            return Magnitude() == 1;
        }

        public double DistanceFrom(Vector2 other)
        {
            return (this - other).Abs().Magnitude();
        }

        #endregion

        #region Equality

        public bool Equals(Vector2 other)
        {
            return other.X == X && other.Y == Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2 vector && Equals(vector);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        #endregion

        #region Operators

        // Convert
        public static explicit operator Vector2Int(Vector2 v) => v.ToVector2Int();

        // Equality
        public static bool operator ==(Vector2 v1, Vector2 v2) => v1.Equals(v2);

        public static bool operator !=(Vector2 v1, Vector2 v2) => !(v1 == v2);

        // Greater than
        public static bool operator >(Vector2 v1, Vector2 v2) => v1.X > v2.X && v1.Y > v2.Y;

        public static bool operator >(Vector2 v1, float num) => v1.X > num && v1.Y > num;

        // Less than
        public static bool operator <(Vector2 v1, Vector2 v2) => v2 > v1;

        public static bool operator <(Vector2 v1, float num) => v1.X < num && v1.Y < num;

        // Greater or equal than
        public static bool operator >=(Vector2 v1, Vector2 v2) => !(v1 < v2);

        public static bool operator >=(Vector2 v1, float num) => !(v1 < num);

        // Less or equal than
        public static bool operator <=(Vector2 v1, Vector2 v2) => !(v1 > v2);

        public static bool operator <=(Vector2 v1, float num) => !(v1 > num);

        // Addition
        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new(v1.X + v2.X, v1.Y + v2.Y);

        public static Vector2 operator +(Vector2 v1, float num) => new(v1.X + num, v1.Y + num);

        // Subtraction
        public static Vector2 operator -(Vector2 v1) => new(-v1.X, -v1.Y);

        public static Vector2 operator -(Vector2 v1, Vector2 v2) => new(v1.X - v2.X, v1.Y - v2.Y);

        public static Vector2 operator -(Vector2 v1, float num) => new(v1.X - num, v1.Y - num);

        // Multiplication
        public static Vector2 operator *(Vector2 v1, Vector2 v2) => new(v1.X * v2.X, v1.Y * v2.Y);

        public static Vector2 operator *(Vector2 v1, float num) => new(v1.X * num, v1.Y * num);

        // Division
        public static Vector2 operator /(Vector2 v1, Vector2 v2) => new(v1.X / v2.X, v1.Y / v2.Y);

        public static Vector2 operator /(Vector2 v1, float num) => new(v1.X / num, v1.Y / num);

        #endregion

        #region OrInnequalities

        public bool OrLess(Vector2 v)
        {
            return X < v.X || Y < v.Y;
        }

        public bool OrGreater(Vector2 v)
        {
            return X > v.X || Y > v.Y;
        }

        public bool OrLessEqual(Vector2 v)
        {
            return X <= v.X || Y <= v.Y;
        }

        public bool OrGreaterEqual(Vector2 v)
        {
            return X >= v.X || Y >= v.Y;
        }

        #endregion

        #region ReadVectorString

        /// <summary>
        /// Bad, do not use.
        /// </summary>
        public static Vector2 ReadVectorString(string vectorStr)
        {
            if (vectorStr.Length < 3)
                throw new ArgumentException("String vector must be atleast 3 characters long");

            vectorStr = StringUtils.RemoveInstancesOf(vectorStr, ' ');

            int splitIndex = vectorStr.IndexOf(VectorStringSplitChar), lastIndex = vectorStr.LastIndexOf(VectorStringSplitChar);

            if (splitIndex == -1)
                throw new ArgumentException("String vector didn't contain a valid split char");
            if (lastIndex != splitIndex)
                throw new ArgumentException("String vector contained multiple split chars");

            int yStartIndex = splitIndex + 1;

            string xStr = vectorStr[..splitIndex], yStr = vectorStr[yStartIndex..];

            if (!float.TryParse(xStr, out float x))
                throw new ArgumentException("Found x was not valid");
            if (!float.TryParse(yStr, out float y))
                throw new ArgumentException("Found y was not valid");

            return new(x, y);
        }

        #endregion

        #region Utility   

        public Vector2Int ToVector2Int()
        {
            return new((int)X, (int)Y);
        }

        public void Deconstruct(out float x, out float y)
        {
            x = X;
            y = Y;
        }

        // Math
        public static Vector2 Min(Vector2 v1, Vector2 v2)
        {
            return v1 <= v2 ? v1 : v2;
        }

        public static Vector2 Max(Vector2 v1, Vector2 v2)
        {
            return v1 >= v2 ? v1 : v2;
        }

        public Vector2 Abs()
        {
            return new(MathF.Abs(X), MathF.Abs(Y));
        }

        public Vector2 Ceil()
        {
            return new(MathF.Ceiling(X), MathF.Ceiling(Y));
        }

        public Vector2 Floor()
        {
            return new(MathF.Floor(X), MathF.Floor(Y));
        }

        public Vector2 Round()
        {
            return new(MathF.Round(X, 0, MidpointRounding.AwayFromZero), MathF.Round(Y, 0, MidpointRounding.AwayFromZero));
        }

        // InRange
        public bool InRange(Vector2 min, Vector2 max)
        {
            return this >= min && this < max;
        }

        public bool InRange(float num)
        {
            return num >= X && num < Y;
        }

        public bool InFullRange(Vector2 min, Vector2 max)
        {
            return this >= min && this <= max;
        }

        public bool InFullRange(float num)
        {
            return num >= X && num <= Y;
        }

        // Clamp
        public Vector2 ClampMin(Vector2 min)
        {
            return new(X < min.X ? min.X : X, Y < min.Y ? min.Y : Y);
        }

        public Vector2 ClampMax(Vector2 max)
        {
            return new(X > max.X ? max.X : X, Y > max.Y ? max.Y : Y);
        }

        public Vector2 Clamp(Vector2 min, Vector2 max)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum was greater than the Maximum.");
            }
            return ClampMin(min).ClampMax(max);
        }

        #endregion

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public string ToString(string? format)
        {
            return $"{X.ToString(format)},{Y.ToString(format)}";
        }

        public string ToString(string? format, IFormatProvider? provider)
        {
            return ToString(format);
        }
    }
}