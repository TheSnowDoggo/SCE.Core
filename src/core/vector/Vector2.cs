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
        public static Vector2 Zero { get; } = new(0.0f, 0.0f);

        public static Vector2 Up { get; } = new(0.0f, 1.0f);

        public static Vector2 Down { get; } = new(0.0f, -1.0f);

        public static Vector2 Right { get; } = new(1.0f, 0.0f);

        public static Vector2 Left { get; } = new(-1.0f, 0.0f);

        public static Vector2 UpLeft { get; } = new(-1.0f, 1.0f);

        public static Vector2 UpRight { get; } = new(1.0f, 1.0f);

        public static Vector2 DownLeft { get; } = new(-1.0f, -1.0f);

        public static Vector2 DownRight { get; } = new(1.0f, -1.0f);
        #endregion

        #region Properties

        public Vector2 Inverse() => new(Y, X);

        public float ScalarProduct() => X * Y;

        public Vector2 Midpoint() => new Vector2(X, Y) / 2.0f;

        public float Magnitude()
        {
            return MathF.Sqrt((X * X) + (Y * Y));
        }

        public Vector2 Normalize()
        {
            var magnitude = Magnitude();
            if (magnitude == 0)
                throw new DivideByZeroException("Magnitude cannot be zero.");
            return new Vector2(X, Y) / magnitude;
        }

        public bool IsNormalized()
        {
            return Magnitude() == 1;
        }

        #endregion

        #region Operators

        #region Conversion

        public static explicit operator Vector2Int(Vector2 v) => v.ToVector2Int();

        #endregion

        #region Equality

        public static bool operator ==(Vector2 v1, Vector2 v2) => v1.Equals(v2);

        public static bool operator !=(Vector2 v1, Vector2 v2) => !(v1 == v2);

        #endregion

        #region Innequality

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

        #endregion

        #region Addition

        // Addition
        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new(v1.X + v2.X, v1.Y + v2.Y);

        public static Vector2 operator +(Vector2 v1, float num) => new(v1.X + num, v1.Y + num);

        #endregion

        #region Subtraction

        // Subtraction
        public static Vector2 operator -(Vector2 v1) => new(-v1.X, -v1.Y);

        public static Vector2 operator -(Vector2 v1, Vector2 v2) => new(v1.X - v2.X, v1.Y - v2.Y);

        public static Vector2 operator -(Vector2 v1, float num) => new(v1.X - num, v1.Y - num);

        #endregion

        #region Multiplication

        // Multiplication
        public static Vector2 operator *(Vector2 v1, Vector2 v2) => new(v1.X * v2.X, v1.Y * v2.Y);

        public static Vector2 operator *(Vector2 v1, float num) => new(v1.X * num, v1.Y * num);

        #endregion

        #region Division

        // Division
        public static Vector2 operator /(Vector2 v1, Vector2 v2) => new(v1.X / v2.X, v1.Y / v2.Y);

        public static Vector2 operator /(Vector2 v1, float num) => new(v1.X / num, v1.Y / num);

        #endregion

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

        #region ToString

        public string ToString(string? format)
        {
            return $"{X.ToString(format)},{Y.ToString(format)}";
        }

        public string ToString(string? format, IFormatProvider? provider)
        {
            if (provider is not null)
                throw new NotImplementedException("Providers not implemented.");
            return ToString(format);
        }

        #endregion

        #region OrInnequalities

        public static bool OrLess(Vector2 v1, Vector2 v2)
        {
            return v1.X < v2.X || v1.Y < v2.Y;
        }

        public static bool OrGreater(Vector2 v1, Vector2 v2)
        {
            return v1.X > v2.X || v1.Y > v2.Y;
        }

        public static bool OrLessEqual(Vector2 v1, Vector2 v2)
        {
            return v1.X <= v2.X || v1.Y <= v2.Y;
        }

        public static bool OrGreaterEqual(Vector2 v1, Vector2 v2)
        {
            return v1.X >= v2.X || v1.Y >= v2.Y;
        }

        #endregion

        #region ReadVectorString

        /// <summary>
        /// Returns the vector representation of the given string vector.
        /// </summary>
        /// <param name="vectorStr">The vector string to read.</param>
        /// <returns>The vector representation of the given string vector.</returns>
        /// <exception cref="ArgumentException">Thrown if the given string vector is invalid.</exception>
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

        #region Expose

        public void Expose(out float x, out float y)
        {
            x = X;
            y = Y;
        }

        #endregion

        #region Conversion

        public Vector2Int ToVector2Int()
        {
            return new((int)X, (int)Y);
        }

        #endregion

        #region Math

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

        #endregion

        #region InRange

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

        #endregion

        #region DistanceFrom

        public double DistanceFrom(Vector2 other)
        {
            return (this - other).Abs().Magnitude();
        }

        #endregion
    }
}
