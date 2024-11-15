namespace SCECore.Types
{
    using SCECore.Utils;

    /// <summary>
    /// Representation of integer 2D vectors.
    /// </summary>
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        private const char VectorStringSplitChar = ',';

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2Int"/> struct.
        /// </summary>
        /// <param name="x">The integer x component of the vector.</param>
        /// <param name="y">The integer y component of the vector.</param>
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets a shorthand for writing Vector2(0, 0).
        /// </summary>
        public static Vector2Int Zero { get; } = new(0, 0);

        /// <summary>
        /// Gets a shorthand for writing Vector2(0, 1).
        /// </summary>
        public static Vector2Int Up { get; } = new(0, 1);

        /// <summary>
        /// Gets a shorthand for writing Vector2(0, -1).
        /// </summary>
        public static Vector2Int Down { get; } = new(0, -1);

        /// <summary>
        /// Gets a shorthand for writing Vector2(1, 0).
        /// </summary>
        public static Vector2Int Right { get; } = new(1, 0);

        /// <summary>
        /// Gets a shorthand for writing Vector2(-1, 0).
        /// </summary>
        public static Vector2Int Left { get; } = new(-1, 0);

        public static Vector2Int UpLeft { get; } = new(-1, 1);

        public static Vector2Int UpRight { get; } = new(1, 1);

        public static Vector2Int DownLeft { get; } = new(-1, -1);

        public static Vector2Int DownRight { get; } = new(1, -1);

        /// <summary>
        /// Gets the integer x component of this instance.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets the integer y component of this instance.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets the inverse vector with swapped integer x and y components based on the current vector.
        /// </summary>
        public Vector2Int Inverse { get => new(Y, X); }

        /// <summary>
        /// Gets the scalar (dot) product of the vector based on the current vector.
        /// </summary>
        public int ScalarProduct { get => X * Y; }

        /// <summary>
        /// Gets the zero-based vector midpoint based of the current vector.
        /// </summary>
        public Vector2Int Midpoint { get => (new Vector2Int(X, Y) - 1) / 2; }

        // Conversion
        public static implicit operator Vector2(Vector2Int v) => v.ToVector2();

        // Equal
        public static bool operator ==(Vector2Int v1, Vector2Int v2) => v1.Equals(v2);

        public static bool operator !=(Vector2Int v1, Vector2Int v2) => !(v1 == v2);

        // Greater than
        public static bool operator >(Vector2Int v1, Vector2Int v2) => v1.X > v2.X && v1.Y > v2.Y;

        public static bool operator >(Vector2Int v1, int num) => v1.X > num && v1.Y > num;

        // Less than
        public static bool operator <(Vector2Int v1, Vector2Int v2) => v1.X < v2.X && v1.Y < v2.Y;

        public static bool operator <(Vector2Int v1, int num) => v1.X < num && v1.Y < num;

        // Greater or equal than
        public static bool operator >=(Vector2Int v1, Vector2Int v2) => v1.X >= v2.X && v1.Y >= v2.Y;

        public static bool operator >=(Vector2Int v1, int num) => v1.X >= num && v1.Y >= num;

        // Less or equal than
        public static bool operator <=(Vector2Int v1, Vector2Int v2) => v1.X <= v2.X && v1.Y <= v2.Y;

        public static bool operator <=(Vector2Int v1, int num) => v1.X <= num && v1.Y <= num;

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

        public bool Equals(Vector2Int other)
        {
            return other.X == X && other.Y == Y;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj != null && Equals((Vector2Int)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{X},{Y}";
        }

        /// <summary>
        /// Returns the vector representation of the given string vector.
        /// </summary>
        /// <param name="vectorStr">The vector string to read.</param>
        /// <returns>The vector representation of the given string vector.</returns>
        /// <exception cref="ArgumentException">Thrown if the given string vector is invalid.</exception>
        public static Vector2Int ReadVectorString(string vectorStr)
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

            if (!int.TryParse(xStr, out int x))
            {
                throw new ArgumentException("Found x was not valid");
            }

            if (!int.TryParse(yStr, out int y))
            {
                throw new ArgumentException("Found y was not valid");
            }

            return new(x, y);
        }

        /// <summary>
        /// Returns a random vector from the specified min and max vectors.
        /// </summary>
        /// <param name="min">The min vector.</param>
        /// <param name="max">The max vector.</param>
        /// <returns>A random vector from the specified min and max vectors.</returns>
        public static Vector2Int Rand(Vector2Int min, Vector2Int max)
        {
            Random rnd = new();

            int x = rnd.Next(min.X, max.X);
            int y = rnd.Next(min.Y, max.Y);

            return new(x, y);
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
        /// Exposes the integer x and y components of the vector.
        /// </summary>
        /// <param name="x">The integer x component of the vector.</param>
        /// <param name="y">The integer y component of the vector.</param>
        public void Expose(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        /// <summary>
        /// Returns the <see cref="Vector2"/> based on the current vector.
        /// </summary>
        /// <returns>The <see cref="Vector2"/> based on the current vector.</returns>
        public Vector2 ToVector2()
        {
            return new(X, Y);
        }

        /// <summary>
        /// Returns the absolute value of the vector.
        /// </summary>
        /// <returns>The absolute value of the vector.</returns>
        public Vector2Int Abs()
        {
            return new(Math.Abs(X), Math.Abs(Y));
        }

        /// <summary>
        /// Indicates whether this vector is within the top exclusive range of the specified minimum and maximum vectors.
        /// </summary>
        /// <param name="min">The minimum vector.</param>
        /// <param name="max">The maximum vector.</param>
        /// <returns><see langword="true"/> if this vector is within the top exclusive range of the <paramref name="min"/> and <paramref name="max"/> vectors; otherwise, <see langword="false"/>.</returns>
        public bool InRange(Vector2Int min, Vector2Int max)
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
            return num >= X && num < Y;
        }

        /// <summary>
        /// Indicates whether this vector is within the inclusive range of the specified minimum and maximum vectors.
        /// </summary>
        /// <param name="min">The minimum vector.</param>
        /// <param name="max">The maximum vector.</param>
        /// <returns><see langword="true"/> if this vector is within the inclusive range of the <paramref name="min"/> and <paramref name="max"/> vectors; otherwise, <see langword="false"/>.</returns>
        public bool InFullRange(Vector2Int min, Vector2Int max)
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
            return num >= X && num <= Y;
        }
    }
}
