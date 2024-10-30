namespace SCECore.Objects
{
    using SCECore.Utils;

    /// <summary>
    /// Representation of floating point 2D vectors.
    /// </summary>
    public readonly struct Vector2
    {
        private const char VectorStringSplitChar = ',';

        private readonly double x;

        private readonly double y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> struct.
        /// </summary>
        /// <param name="x">The x component of the vector.</param>
        /// <param name="y">The y component of the vector.</param>
        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets a shorthand for writing Vector2(0, 0).
        /// </summary>
        public static Vector2 Zero { get; } = new(0, 0);

        /// <summary>
        /// Gets a shorthand for writing Vector2(0, 1).
        /// </summary>
        public static Vector2 Up { get; } = new(0, 1);

        /// <summary>
        /// Gets a shorthand for writing Vector2(0, -1).
        /// </summary>
        public static Vector2 Down { get; } = new(0, -1);

        /// <summary>
        /// Gets a shorthand for writing Vector2(1, 0).
        /// </summary>
        public static Vector2 Right { get; } = new(1, 0);

        /// <summary>
        /// Gets a shorthand for writing Vector2(-1, 0).
        /// </summary>
        public static Vector2 Left { get; } = new(-1, 0);

        /// <summary>
        /// Gets the X component of this instance.
        /// </summary>
        public double X { get => x; }

        /// <summary>
        /// Gets the Y component of this instance.
        /// </summary>
        public double Y { get => y; }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        public double Magnitude { get => Math.Sqrt((X * X) + (Y * Y)); }

        /// <summary>
        /// Gets the normalized (unit) vector based on the current vector if the magnitude is not equal to 0.
        /// </summary>
        public Vector2 Normalized { get => Normalize(); }

        /// <summary>
        /// Gets the inverse vector with swapped x and y components based on the current vector.
        /// </summary>
        public Vector2 Inverse { get => new(Y, X); }

        /// <summary>
        /// Gets the scalar (dot) product of the vector based on the current vector.
        /// </summary>
        public double ScalarProduct { get => X * Y; }

        /// <summary>
        /// Gets the zero-based vector midpoint based of the current vector.
        /// </summary>
        public Vector2 Midpoint { get => (new Vector2(X, Y) - 1) / 2; }

        // Conversion
        public static explicit operator Vector2Int(Vector2 v) => v.ToVector2Int();

        // Equality
        public static bool operator ==(Vector2 v1, Vector2 v2) => v1.Equals(v2);

        public static bool operator !=(Vector2 v1, Vector2 v2) => !(v1 == v2);

        // Addition
        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new(v1.X + v2.X, v1.Y + v2.Y);

        public static Vector2 operator +(Vector2 v1, double num) => new(v1.X + num, v1.Y + num);

        public static Vector2 operator +(Vector2 v1, int num) => new(v1.X + num, v1.Y + num);

        // Subtraction
        public static Vector2 operator -(Vector2 v1) => new(-v1.X, -v1.Y);

        public static Vector2 operator -(Vector2 v1, Vector2 v2) => new(v1.X - v2.X, v1.Y - v2.Y);

        public static Vector2 operator -(Vector2 v1, double num) => new(v1.X - num, v1.Y - num);

        public static Vector2 operator -(Vector2 v1, int num) => new(v1.X - num, v1.Y - num);

        // Multiplication
        public static Vector2 operator *(Vector2 v1, Vector2 v2) => new(v1.X * v2.X, v1.Y * v2.Y);

        public static Vector2 operator *(Vector2 v1, double num) => new(v1.X * num, v1.Y * num);

        public static Vector2 operator *(Vector2 v1, int num) => new(v1.X * num, v1.Y * num);

        // Division
        public static Vector2 operator /(Vector2 v1, Vector2 v2) => new(v1.X / v2.X, v1.Y / v2.Y);

        public static Vector2 operator /(Vector2 v1, double num) => new(v1.X / num, v1.Y / num);

        public static Vector2 operator /(Vector2 v1, int num) => new(v1.X / num, v1.Y / num);

        // Greater than
        public static bool operator >(Vector2 v1, Vector2 v2) => v1.X > v2.X && v1.Y > v2.Y;

        public static bool operator >(Vector2 v1, double num) => v1.X > num && v1.Y > num;

        public static bool operator >(Vector2 v1, int num) => v1 > (double)num;

        // Less than
        public static bool operator <(Vector2 v1, Vector2 v2) => v1.X < v2.X && v1.Y < v2.Y;

        public static bool operator <(Vector2 v1, double num) => v1.X < num && v1.Y < num;

        public static bool operator <(Vector2 v1, int num) => v1 < (double)num;

        // Greater or equal than
        public static bool operator >=(Vector2 v1, Vector2 v2) => v1.X >= v2.X && v1.Y >= v2.Y;

        public static bool operator >=(Vector2 v1, double num) => v1.X >= num && v1.Y >= num;

        public static bool operator >=(Vector2 v1, int num) => v1 >= (double)num;

        // Less or equal than
        public static bool operator <=(Vector2 v1, Vector2 v2) => v1.X <= v2.X && v1.Y <= v2.Y;

        public static bool operator <=(Vector2 v1, double num) => v1.X <= num && v1.Y <= num;

        public static bool operator <=(Vector2 v1, int num) => v1 <= (double)num;

        /// <summary>
        /// Returns the vector representation of the given string vector.
        /// </summary>
        /// <param name="vectorStr">The vector string to read.</param>
        /// <returns>The vector representation of the given string vector.</returns>
        /// <exception cref="ArgumentException">Thrown if the given string vector is invalid.</exception>
        public static Vector2 ReadVectorString(string vectorStr)
        {
            if (vectorStr.Length < 3)
            {
                throw new ArgumentException("String vector must be atleast 3 characters long");
            }

            vectorStr = SCEString.RemoveInstancesOf(vectorStr, ' ');

            int splitIndex = vectorStr.IndexOf(VectorStringSplitChar), lastIndex = vectorStr.LastIndexOf(VectorStringSplitChar);

            if (splitIndex == -1)
            {
                throw new ArgumentException("String vector didn't contain a valid split char");
            }

            if (lastIndex != splitIndex)
            {
                throw new ArgumentException("String vector contained multiple split chars");
            }

            int yStartIndex = splitIndex + 1;

            string xStr = vectorStr[..splitIndex], yStr = vectorStr[yStartIndex..];

            if (!double.TryParse(xStr, out double x))
            {
                throw new ArgumentException("Found x was not valid");
            }

            if (!double.TryParse(yStr, out double y))
            {
                throw new ArgumentException("Found y was not valid");
            }

            return new(x, y);
        }

        /// <summary>
        /// Returns the smallest of the two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>The smallest of the two vectors.</returns>
        public static Vector2Int Min(Vector2Int v1, Vector2Int v2)
        {
            return v1 <= v2 ? v1 : v2;
        }

        /// <summary>
        /// Returns the largest of the two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>The largest of the two vectors.</returns>
        public static Vector2Int Max(Vector2Int v1, Vector2Int v2)
        {
            return v1 >= v2 ? v1 : v2;
        }

        /// <summary>
        /// Indicates whether this vector and the specified vector are equal.
        /// </summary>
        /// <param name="v">The vector to compare with this vector.</param>
        /// <returns><see langword="true"/> if this <paramref name="v"/> and this instance represent the same value; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Vector2 v)
        {
            return X == v.X && Y == v.Y;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj != null && Equals((Vector2)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{X},{Y}";
        }

        /// <inheritdoc cref="double.ToString(string?)"/>
        public string ToString(string? format)
        {
            return $"{X.ToString(format)},{Y.ToString(format)}";
        }

        /// <summary>
        /// Exposes the x and y components of the vector.
        /// </summary>
        /// <param name="x">The x component of the vector.</param>
        /// <param name="y">The y component of the vector.</param>
        public void Expose(out double x, out double y)
        {
            x = X;
            y = Y;
        }

        /// <summary>
        /// Returns the converted <see cref="Vector2Int"/> based on the current vector.
        /// </summary>
        /// <returns>The converted <see cref="Vector2Int"/> based on the current vector.</returns>
        public Vector2Int ToVector2Int()
        {
            return new((int)X, (int)Y);
        }

        /// <summary>
        /// Returns the absolute value of the vector.
        /// </summary>
        /// <returns>The absolute value of the vector.</returns>
        public Vector2 Abs()
        {
            return new(Math.Abs(X), Math.Abs(Y));
        }

        /// <summary>
        /// Returns the ceiled vector.
        /// </summary>
        /// <returns>The ceiled vector.</returns>
        public Vector2 Ceil()
        {
            return new(Math.Ceiling(X), Math.Ceiling(Y));
        }

        /// <summary>
        /// Returns the floored vector.
        /// </summary>
        /// <returns>The floored vector.</returns>
        public Vector2 Floor()
        {
            return new(Math.Floor(X), Math.Floor(Y));
        }

        /// <summary>
        /// Returns the away from zero rounded vector.
        /// </summary>
        /// <returns>The away from zero rounded vector.</returns>
        public Vector2 Round()
        {
            return new(Math.Round(X, 0, MidpointRounding.AwayFromZero), Math.Round(Y, 0, MidpointRounding.AwayFromZero));
        }

        /// <summary>
        /// Indicates wheter this vector is within the top exclusive range of the specified minimum and maximum vectors.
        /// </summary>
        /// <param name="min">The minimum vector.</param>
        /// <param name="max">The maximum vector.</param>
        /// <returns><see langword="true"/> if this vector is within the top exclusive range of the <paramref name="min"/> and <paramref name="max"/> vectors; otherwise, <see langword="false"/>.</returns>
        public bool InRange(Vector2 min, Vector2 max)
        {
            return this >= min && this < max;
        }

        /// <summary>
        /// Indicates whether the specified integer is within the top exclusive range of this vector.
        /// </summary>
        /// <param name="num">The integer to check.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="num"/> is within the top exclusive range of this vector; otherwise, <see langword="false"/>.</returns>
        public bool InRange(int num)
        {
            return InRange((double)num);
        }

        /// <summary>
        /// Indicates whether the specified double is within the top exclusive range of this vector.
        /// </summary>
        /// <param name="num">The double to check.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="num"/> is within the top exclusive range of this vector; otherwise, <see langword="false"/>.</returns>
        public bool InRange(double num)
        {
            return num >= X && num < Y;
        }

        /// <summary>
        /// Indicates wheter this vector is within the inclusive range of the specified minimum and maximum vectors.
        /// </summary>
        /// <param name="min">The minimum vector.</param>
        /// <param name="max">The maximum vector.</param>
        /// <returns><see langword="true"/> if this vector is within the inclusive range of the <paramref name="min"/> and <paramref name="max"/> vectors; otherwise, <see langword="false"/>.</returns>
        public bool InFullRange(Vector2 min, Vector2 max)
        {
            return this >= min && this <= max;
        }

        /// <summary>
        /// Indicates whether the specified integer is within the inclusive range of this vector.
        /// </summary>
        /// <param name="num">The integer to check.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="num"/> is within the inclusive range of this vector; otherwise, <see langword="false"/>.</returns>
        public bool InFullRange(int num)
        {
            return InFullRange((double)num);
        }

        /// <summary>
        /// Indicates whether the specified double is within the inclusive range of this vector.
        /// </summary>
        /// <param name="num">The double to check.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="num"/> is within the inclusive range of this vector; otherwise, <see langword="false"/>.</returns>
        public bool InFullRange(double num)
        {
            return num >= X && num <= Y;
        }

        /// <summary>
        /// Returns a normalized (unit) vector based on the current vector if the magnitude is not equal to 0.
        /// </summary>
        /// <returns>A normalized (unit) vector based on the current vector if the magnitude is not equal to 0.</returns>
        /// <exception cref="DivideByZeroException">Thrown if the magnitude is 0.</exception>
        public Vector2 Normalize()
        {
            if (Magnitude == 0)
            {
                throw new DivideByZeroException("Magnitude cannot be zero.");
            }

            return new Vector2(X, Y) / Magnitude;
        }

        /// <summary>
        /// Returns the distance between this vector and the specified vector.
        /// </summary>
        /// <param name="other">The other vector to get the distance from.</param>
        /// <returns>The distance between this vector and the specified vector.</returns>
        public double DistanceFrom(Vector2 other)
        {
            return (this - other).Abs().Magnitude;
        }
    }
}
