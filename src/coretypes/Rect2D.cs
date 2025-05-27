namespace SCE
{
    /// <summary>
    /// A struct representing a 2D rectangular area defined by start and end corner.
    /// </summary>
    public readonly struct Rect2D : IEquatable<Rect2D>
    {
        public readonly int Left;

        public readonly int Bottom;

        public readonly int Right;

        public readonly int Top;

        public Rect2D(int left, int bottom, int right, int top)
        {
            if (left >= right || bottom >= top)
            {
                throw new ArgumentException("Dimensions were invalid.");
            }
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
        }

        public Rect2D(int right, int top)
            : this(0, 0, right, top)
        {
        }

        public Rect2D(Vector2Int start, Vector2Int end)
            : this(start.X, start.Y, end.X, end.Y)
        {
        }

        public Rect2D(Vector2Int end)
            : this(Vector2Int.Zero, end)
        {
        }

        public Vector2Int Start() => new(Left, Bottom);

        public Vector2Int End() => new(Right, Top);

        public int Width() => Right - Left;

        public int Height() => Top - Bottom;

        public int Size() => Width() * Height();

        public Vector2Int Dimensions() => new(Width(), Height());

        #region Operators

        #region Equality

        public static bool operator ==(Rect2D left, Rect2D right) => left.Equals(right);

        public static bool operator !=(Rect2D left, Rect2D right) => !(left == right);

        #endregion

        #region Innequalities

        public static bool operator >(Rect2D a1, Rect2D a2) => a1.Size() > a2.Size();

        public static bool operator <(Rect2D a1, Rect2D a2) => a2 > a1;

        public static bool operator >=(Rect2D a1, Rect2D a2) => !(a1 < a2);

        public static bool operator <=(Rect2D a1, Rect2D a2) => !(a1 > a2);

        #endregion

        #region Addition

        public static Rect2D operator +(Rect2D a1, Rect2D a2) => new(a1.Left + a2.Left, a1.Bottom + a2.Bottom, a1.Right + a2.Right, a1.Top + a2.Top);

        public static Rect2D operator +(Rect2D a1, Vector2Int v) => new(a1.Left + v.X, a1.Bottom + v.Y, a1.Right + v.X, a1.Top + v.Y);

        #endregion

        #region Subtraction

        public static Rect2D operator -(Rect2D a1, Rect2D a2) => new(a1.Left - a2.Left, a1.Bottom - a2.Bottom, a1.Right - a2.Right, a1.Top - a2.Top);

        public static Rect2D operator -(Rect2D a1, Vector2Int v) => new(a1.Left - v.X, a1.Bottom - v.Y, a1.Right - v.X, a1.Top - v.Y);

        public static Rect2D operator -(Rect2D a) => new(-a.Left, -a.Bottom, -a.Right, -a.Top);

        #endregion

        #region Multiplication

        public static Rect2D operator *(Rect2D a1, Rect2D a2) => new(a1.Left * a2.Left, a1.Bottom * a2.Bottom, a1.Right * a2.Right, a1.Top * a2.Top);

        public static Rect2D operator *(Rect2D a1, Vector2Int v) => new(a1.Left * v.X, a1.Bottom * v.Y, a1.Right * v.X, a1.Top * v.Y);

        #endregion

        #region Division

        public static Rect2D operator /(Rect2D a1, Rect2D a2) => new(a1.Left / a2.Left, a1.Bottom / a2.Bottom, a1.Right / a2.Right, a1.Top / a2.Top);

        public static Rect2D operator /(Rect2D a1, Vector2Int v) => new(a1.Left / v.X, a1.Bottom / v.Y, a1.Right / v.X, a1.Top / v.Y);

        #endregion

        #endregion

        #region Equality

        public bool Equals(Rect2D area)
        {
            return Left == area.Left && Bottom == area.Bottom && Right == area.Right && Top == area.Top;
        }

        public override bool Equals(object? obj)
        {
            return obj is Rect2D area && Equals(area);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Bottom, Right, Top);
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return $"{Left},{Bottom},{Right},{Top}";
        }

        #endregion

        #region Deconstruct

        public void Deconstruct(out int left, out int bottom, out int right, out int top)
        {
            left = Left;
            bottom = Bottom;
            right = Right;
            top = Top;
        }

        public void Deconstruct(out Vector2Int start, out Vector2Int end)
        {
            start = Start();
            end = End();
        }

        #endregion

        #region Utility

        #region Lines

        public static Rect2D Horizontal(int y, int left, int right)
        {
            return new(left, y, right, y + 1);
        }

        public static Rect2D Horizontal(int y, int width)
        {
            return Horizontal(y, 0, width);
        }

        public static Rect2D Vertical(int x, int bottom, int top)
        {
            return new(x, bottom, x + 1, top);
        }

        public static Rect2D Vertical(int x, int height)
        {
            return Vertical(x, 0, height);
        }

        #endregion

        #region Overlaps

        public static bool Overlaps(int l1, int b1, int r1, int t1, int l2, int b2, int r2, int t2)
        {
            if (r1 <= l2 || l1 >= r2) // X sides don't overlap
                return false;          
            if (t1 <= b2 || b1 >= t2) // Y sides don't overlap
                return false;
            return true;
        }

        public bool Overlaps(int l, int b, int r, int t)
        {
            return Overlaps(Left, Bottom, Right, Top, l, b, r, t);
        }

        public static bool Overlaps(Vector2Int start1, Vector2Int end1, Vector2Int start2, Vector2Int end2)
        {
            return Overlaps(start1.X, start1.Y, end1.X, end1.Y, start2.X, start2.Y, end2.X, end2.Y);
        }

        public bool Overlaps(Vector2Int start, Vector2Int end)
        {
            return Overlaps(Left, Bottom, Right, Top, start.X, start.Y, end.X, end.Y);
        }

        public static bool Overlaps(Rect2D a1, Rect2D a2)
        {
            return Overlaps(a1.Left, a1.Bottom, a1.Right, a1.Top, a2.Left, a2.Bottom, a2.Right, a2.Top);
        }

        public bool Overlaps(Rect2D other)
        {
            return Overlaps(this, other);
        }

        #endregion

        #region GetOverlap

        public static Rect2D GetOverlap(int l1, int b1, int r1, int t1, int l2, int b2, int r2, int t2)
        {
            if (!Overlaps(l1, b1, r1, t1, l2, b2, r2, t2))
                throw new ArgumentException("Areas do not overlap.");

            Vector2Int v1 = new(Math.Max(l1, l2), Math.Max(b1, b2));

            Vector2Int v2 = new(Math.Min(r1, r2), Math.Min(t1, t2));

            Vector2Int start = Vector2Int.Min(v1, v2);

            Vector2Int end = Vector2Int.Max(v1, v2);

            return new(start, end);
        }

        public static Rect2D GetOverlap(Rect2D a1, Rect2D a2)
        {
            return GetOverlap(a1.Left, a1.Bottom, a1.Right, a1.Top, a2.Left, a2.Bottom, a2.Right, a2.Top);
        }

        #endregion

        #region Trim

        /// <summary>
        /// Returns the given <paramref name="area"/> trimmed from this instance.
        /// </summary>
        /// <param name="area">The area to trim.</param>
        /// <returns>The given <paramref name="area"/> trimmed from this instance.</returns>
        public Rect2D TrimArea(Rect2D other)
        {
            if (!Overlaps(this, other))
                throw new ArgumentException("Areas do not overlap.");

            return new(other.Left < Left ? Left : other.Left,
                other.Bottom < Bottom ? Bottom : other.Bottom,
                other.Right > Right ? Right : other.Right,
                other.Top > Top ? Top : other.Top);
        }

        #endregion

        #region Bound

        /// <summary>
        /// Returns the specified area realigned to be bound inside this area.
        /// </summary>
        /// <param name="area">The area to bound inside this area.</param>
        /// <returns>The specified area realigned to be bound inside this area.</returns>
        public Rect2D Bound(Rect2D area)
        {
            Vector2Int offset = Vector2Int.Zero;

            if (area.Left < Left)
                offset.X += Left - area.Left;
            if (area.Right > Right)
                offset.X += Right - area.Right;
            if (area.Bottom < Bottom)
                offset.Y += Bottom - area.Bottom;
            if (area.Top > Top)
                offset.Y += Top - area.Top;

            return area + offset;
        }

        #endregion

        #region Contains

        public bool Contains(Rect2D other)
        {
            return Left <= other.Left && Bottom <= other.Bottom && other.Right <= Right && other.Top <= Top;
        }

        public bool Contains(Vector2Int position)
        {
            return Left <= position.X && Bottom <= position.Y && position.X < Right && position.Y < Top;
        }

        #endregion

        public IEnumerable<Vector2Int> Enumerate(bool rowMajor = true)
        {
            int s1 = rowMajor ? Bottom : Left;
            int s2 = rowMajor ? Left   : Bottom;
            int e1 = rowMajor ? Top    : Right;
            int e2 = rowMajor ? Right  : Top;
            for (int i = s1; i < e1; ++i)
            {
                for (int j = s2; j < e2; ++j)
                {
                    yield return rowMajor ? new(j, i) : new(i, j);
                }
            }
        }

        #endregion
    }
}
