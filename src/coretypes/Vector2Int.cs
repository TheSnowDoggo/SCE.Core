namespace SCE
{
    /// <summary>
    /// Representation of integer 2D vectors.
    /// </summary>
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        private const char VectorStringSplitChar = ',';

        private static readonly Random _rand = new();

        public int X;

        public int Y;

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Shorthands

        public static Vector2Int Zero { get; } = new(+0, +0);

        public static Vector2Int Up { get; } = new(+0, -1);

        public static Vector2Int Down { get; } = new(+0, +1);

        public static Vector2Int Right { get; } = new(+1, +0);

        public static Vector2Int Left { get; } = new(-1, +0);

        public static Vector2Int UpLeft { get; } = new(-1, -1);

        public static Vector2Int UpRight { get; } = new(+1, -1);

        public static Vector2Int DownLeft { get; } = new(-1, +1);

        public static Vector2Int DownRight { get; } = new(+1, +1);

        #endregion

        #region Property

        public Vector2Int Inverse() => new(Y, X);

        public int ScalarProduct() => X * Y;

        public Vector2Int Midpoint() => (new Vector2Int(X, Y) - 1) / 2;

        #endregion

        #region Equality

        public bool Equals(Vector2Int other)
        {
            return other.X == X && other.Y == Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector2Int vector && Equals(vector);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        #endregion

        #region Operators

        public static implicit operator Vector2(Vector2Int v) => v.ToVector2();

        // Equality
        public static bool operator ==(Vector2Int v1, Vector2Int v2) => v1.Equals(v2);

        public static bool operator !=(Vector2Int v1, Vector2Int v2) => !(v1 == v2);

        // Greater than
        public static bool operator >(Vector2Int v1, Vector2Int v2) => v1.X > v2.X && v1.Y > v2.Y;

        public static bool operator >(Vector2Int v1, int num) => v1.X > num && v1.Y > num;

        // Less than
        public static bool operator <(Vector2Int v1, Vector2Int v2) => v2 > v1;

        public static bool operator <(Vector2Int v1, int num) => v1.X < num && v1.Y < num;

        // Greater or equal than
        public static bool operator >=(Vector2Int v1, Vector2Int v2) => !(v1 < v2);

        public static bool operator >=(Vector2Int v1, int num) => !(v1 < num);

        // Less or equal than
        public static bool operator <=(Vector2Int v1, Vector2Int v2) => !(v1 > v2);

        public static bool operator <=(Vector2Int v1, int num) => !(v1 > num);

        // Addition
        public static Vector2Int operator +(Vector2Int v1, Vector2Int v2) => new(v1.X + v2.X, v1.Y + v2.Y);

        public static Vector2Int operator +(Vector2Int v1, int num) => new(v1.X + num, v1.Y + num);

        // Subtraction
        public static Vector2Int operator -(Vector2Int v1) => new(-v1.X, -v1.Y);

        public static Vector2Int operator -(Vector2Int v1, Vector2Int v2) => new(v1.X - v2.X, v1.Y - v2.Y);

        public static Vector2Int operator -(Vector2Int v1, int num) => new(v1.X - num, v1.Y - num);

        // Multiplication
        public static Vector2Int operator *(Vector2Int v1, Vector2Int v2) => new(v1.X * v2.X, v1.Y * v2.Y);

        public static Vector2Int operator *(Vector2Int v1, int num) => new(v1.X * num, v1.Y * num);

        // Division
        public static Vector2Int operator /(Vector2Int v1, Vector2Int v2) => new(v1.X / v2.X, v1.Y / v2.Y);

        public static Vector2Int operator /(Vector2Int v1, int num) => new(v1.X / num, v1.Y / num);

        #endregion

        #region OrInnequalities

        public bool OrLess(Vector2Int v)
        {
            return X < v.X || Y < v.Y;
        }

        public bool OrGreater(Vector2Int v)
        {
            return X > v.X || Y > v.Y;
        }

        public bool OrLessEqual(Vector2Int v)
        {
            return X <= v.X || Y <= v.Y;
        }

        public bool OrGreaterEqual(Vector2Int v)
        {
            return X >= v.X || Y >= v.Y;
        }

        #endregion

        #region ReadVectorString

        /// <summary>
        /// Bad, do not use
        /// </summary>
        public static Vector2Int ReadVectorString(string vectorStr)
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

            if (!int.TryParse(xStr, out int x))
                throw new ArgumentException("Found x was not valid");
            if (!int.TryParse(yStr, out int y))
                throw new ArgumentException("Found y was not valid");

            return new(x, y);
        }

        #endregion

        #region Utility

        public Vector2 ToVector2()
        {
            return new(X, Y);
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public static Vector2Int Min(Vector2Int v1, Vector2Int v2)
        {
            return v1 <= v2 ? v1 : v2;
        }

        public static Vector2Int Max(Vector2Int v1, Vector2Int v2)
        {
            return v1 >= v2 ? v1 : v2;
        }

        public Vector2Int Abs()
        {
            return new(Math.Abs(X), Math.Abs(Y));
        }

        public bool InRange(Vector2Int min, Vector2Int max)
        {
            return this >= min && this < max;
        }

        public bool InRange(int num)
        {
            return num >= X && num < Y;
        }

        public bool InFullRange(Vector2Int min, Vector2Int max)
        {
            return this >= min && this <= max;
        }

        public bool InFullRange(int num)
        {
            return num >= X && num <= Y;
        }

        /// <summary>
        /// Returns a random vector from the specified min and max vectors.
        /// </summary>
        /// <param name="min">The min vector.</param>
        /// <param name="max">The max vector.</param>
        /// <returns>A random vector from the specified min and max vectors.</returns>
        public static Vector2Int Rand(Vector2Int min, Vector2Int max)
        {
            return new(_rand.Next(min.X, max.X), _rand.Next(min.Y, max.Y));
        }

        /// <summary>
        /// Returns a random vector from the specified max x and y value.
        /// </summary>
        /// <param name="xMax">The maximum x value.</param>
        /// <param name="yMax">The maximum y value.</param>
        /// <returns>A random vector from the specified max x and y value.</returns>
        public static Vector2Int Rand(int xMax, int yMax)
        {
            return Rand(Zero, new Vector2Int(xMax, yMax));
        }

        /// <summary>
        /// Returns a random vector from the specified max-range.
        /// </summary>
        /// <param name="maxRange">The maximum x value.</param>
        /// <returns>A random vector from the specified max-range.</returns>
        public static Vector2Int Rand(Vector2Int maxRange)
        {
            return Rand(maxRange.X, maxRange.Y);
        }

        // Clamp
        public Vector2Int ClampMin(Vector2Int min)
        {
            return new(Math.Max(X, min.X), Math.Max(Y, min.Y));
        }

        public Vector2Int ClampMax(Vector2Int max)
        {
            return new(Math.Min(X, max.X), Math.Min(Y, max.Y));
        }

        public Vector2Int ClampRange(Vector2Int min, Vector2Int max)
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
    }
}
