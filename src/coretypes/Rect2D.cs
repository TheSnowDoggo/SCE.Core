namespace SCE
{
    /// <summary>
    /// A struct representing a 2D rectangular area defined by start and end corner.
    /// </summary>
    public readonly struct Rect2D : IEquatable<Rect2D>
    {
        public Rect2D(float left, float top, float right, float bottom)
        {
            if (left > right)
            {
                (left, right) = (right, left);
            }
            if (top > bottom)
            {
                (top, bottom) = (bottom, top);
            }
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Rect2D(float right, float top)
            : this(0.0f, 0.0f, right, top)
        {
        }

        public Rect2D(Vector2 start, Vector2 end)
            : this(start.X, start.Y, end.X, end.Y)
        {
        }

        public Rect2D(Vector2 end)
            : this(Vector2.Zero, end)
        {
        }

        public float Left { get; }

        public float Top { get; }

        public float Right { get; }

        public float Bottom { get; }

        public float Width { get => Right - Left; }

        public float Height { get => Bottom - Top; }

        public Vector2 Dimensions { get => new(Width, Height); }

        public float Size { get => Width * Height; }

        public Vector2 Start { get => new(Left, Top); }

        public Vector2 End { get => new(Right, Bottom); }

        public Rect2DInt ToRect2DInt()
        {
            return new((int)Left, (int)Top, (int)Right, (int)Bottom);
        }

        public override string ToString()
        {
            return string.Join(",", Left, Top, Right, Bottom);
        }

        #region Equality

        public bool Equals(Rect2D area)
        {
            return Left == area.Left && Top == area.Top && Right == area.Right && Bottom == area.Bottom;
        }

        public override bool Equals(object? obj)
        {
            return obj is Rect2D area && Equals(area);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Top, Right, Bottom);
        }

        #endregion

        #region Operators

        public static explicit operator Rect2DInt(Rect2D r) => r.ToRect2DInt();

        public static bool operator ==(Rect2D left, Rect2D right) => left.Equals(right);

        public static bool operator !=(Rect2D left, Rect2D right) => !(left == right);

        public static bool operator >(Rect2D a1, Rect2D a2) => a1.Size > a2.Size;

        public static bool operator <(Rect2D a1, Rect2D a2) => a2 > a1;

        public static bool operator >=(Rect2D a1, Rect2D a2) => !(a1 < a2);

        public static bool operator <=(Rect2D a1, Rect2D a2) => !(a1 > a2);

        public static Rect2D operator +(Rect2D a1, Rect2D a2) => new(a1.Left + a2.Left, a1.Top + a2.Top, a1.Right + a2.Right, a1.Bottom + a2.Bottom);

        public static Rect2D operator +(Rect2D a1, Vector2 v) => new(a1.Left + v.X, a1.Top + v.Y, a1.Right + v.X, a1.Bottom + v.Y);

        public static Rect2D operator -(Rect2D a1, Rect2D a2) => new(a1.Left - a2.Left, a1.Top - a2.Top, a1.Right - a2.Right, a1.Bottom - a2.Bottom);

        public static Rect2D operator -(Rect2D a1, Vector2 v) => new(a1.Left - v.X, a1.Top - v.Y, a1.Right - v.X, a1.Bottom - v.Y);

        public static Rect2D operator -(Rect2D a) => new(-a.Left, -a.Top, -a.Right, -a.Bottom);

        public static Rect2D operator *(Rect2D a1, Rect2D a2) => new(a1.Left * a2.Left, a1.Top * a2.Top, a1.Right * a2.Right, a1.Bottom * a2.Bottom);

        public static Rect2D operator *(Rect2D a1, Vector2 v) => new(a1.Left * v.X, a1.Top * v.Y, a1.Right * v.X, a1.Bottom * v.Y);

        public static Rect2D operator /(Rect2D a1, Rect2D a2) => new(a1.Left / a2.Left, a1.Top / a2.Top, a1.Right / a2.Right, a1.Bottom / a2.Bottom);

        public static Rect2D operator /(Rect2D a1, Vector2 v) => new(a1.Left / v.X, a1.Top / v.Y, a1.Right / v.X, a1.Bottom / v.Y);

        #endregion

        #region Utility

        public static bool Overlaps(float l1, float t1, float r1, float b1, float l2, float t2, float r2, float b2)
        {
            if (r1 <= l2 || l1 >= r2) // X sides don't overlap
            {
                return false;
            }
            if (b1 <= t2 || t1 >= b2) // Y sides don't overlap
            {
                return false;
            }
            return true;
        }

        public bool Overlaps(float l, float t, float r, float b)
        {
            return Overlaps(Left, Top, Right, Bottom, l, t, r, b);
        }

        public static bool Overlaps(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
        {
            return Overlaps(start1.X, start1.Y, end1.X, end1.Y, start2.X, start2.Y, end2.X, end2.Y);
        }

        public bool Overlaps(Vector2 start, Vector2 end)
        {
            return Overlaps(Left, Top, Right, Bottom, start.X, start.Y, end.X, end.Y);
        }

        public static bool Overlaps(Rect2D a1, Rect2D a2)
        {
            return Overlaps(a1.Left, a1.Top, a1.Right, a1.Bottom, a2.Left, a2.Top, a2.Right, a2.Bottom);
        }

        public bool Overlaps(Rect2D other)
        {
            return Overlaps(this, other);
        }

        public static Rect2D GetOverlap(float l1, float t1, float r1, float b1, float l2, float t2, float r2, float b2)
        {
            if (!Overlaps(l1, t1, r1, b1, l2, t2, r2, b2))
            {
                throw new ArgumentException("Areas do not overlap.");
            }

            Vector2 v1 = new(Math.Max(l1, l2), Math.Max(t1, t2));

            Vector2 v2 = new(Math.Min(r1, r2), Math.Min(b1, b2));

            var start = Vector2.Min(v1, v2);

            var end = Vector2.Max(v1, v2);

            return new(start, end);
        }

        public static Rect2D GetOverlap(Rect2D a1, Rect2D a2)
        {
            return GetOverlap(a1.Left, a1.Top, a1.Right, a1.Bottom, a2.Left, a2.Top, a2.Right, a2.Bottom);
        }

        /// <summary>
        /// Returns the given <paramref name="area"/> trimmed from this instance.
        /// </summary>
        /// <param name="area">The area to trim.</param>
        /// <returns>The given <paramref name="area"/> trimmed from this instance.</returns>
        public Rect2D TrimArea(Rect2D area)
        {
            if (!Overlaps(this, area))
            {
                throw new ArgumentException("Areas do not overlap.");
            }

            return new(area.Left < Left ? Left : area.Left,
                area.Top < Top ? Top : area.Top,
                area.Right > Right ? Right : area.Right,
                area.Bottom > Bottom ? Bottom : area.Bottom);
        }

        /// <summary>
        /// Returns the specified area realigned to be bound inside this area.
        /// </summary>
        /// <param name="area">The area to bound inside this area.</param>
        /// <returns>The specified area realigned to be bound inside this area.</returns>
        public Rect2D Bound(Rect2D area)
        {
            var offset = Vector2.Zero;

            if (area.Left < Left)
            {
                offset.X += Left - area.Left;
            }
            if (area.Top < Top)
            {
                offset.Y += Top - area.Top;
            }
            if (area.Right > Right)
            {
                offset.X += Right - area.Right;
            }
            if (area.Bottom > Bottom)
            {
                offset.Y += Bottom - area.Bottom;
            }

            return area + offset;
        }

        public bool Contains(Rect2D other)
        {
            return Left <= other.Left && Top <= other.Top && other.Right <= Right && other.Bottom <= Bottom;
        }

        public bool Contains(Vector2 position)
        {
            return Left <= position.X && Top <= position.Y && position.X < Right && position.Y < Bottom;
        }

        public void Deconstruct(out float left, out float top, out float right, out float bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }

        public void Deconstruct(out Vector2 start, out Vector2 end)
        {
            start = Start;
            end = End;
        }

        #endregion
    }
}
