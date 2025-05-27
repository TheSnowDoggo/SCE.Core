namespace SCE
{
    /// <summary>
    /// A struct representing a 2D rectangular area defined by start and end corner.
    /// </summary>
    public readonly struct Rect2D : IEquatable<Rect2D>
    {
        public Rect2D(float left, float bottom, float right, float top)
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

        public Rect2D(float right, float top)
            : this(0, 0, right, top)
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

        public float Bottom { get; }

        public float Right { get; }

        public float Top { get; }

        public float Width { get => Right - Left; }

        public float Height { get => Top - Bottom; }

        public Vector2 Dimensions { get => new(Width, Height); }

        public float Size { get => Width * Height; }

        public Vector2 Start { get => new(Left, Bottom); }

        public Vector2 End { get => new(Right, Top); }

        public static explicit operator Rect2DInt(Rect2D r) => r.ToRect2DInt();

        public Rect2DInt ToRect2DInt()
        {
            return new((int)Left, (int)Bottom, (int)Right, (int)Top);
        }

        public override string ToString()
        {
            return string.Join(",", Left, Bottom, Right, Top);
        }

        #region Equality

        public static bool operator ==(Rect2D left, Rect2D right) => left.Equals(right);

        public static bool operator !=(Rect2D left, Rect2D right) => !(left == right);

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

        #region Innequalities

        public static bool operator >(Rect2D a1, Rect2D a2) => a1.Size > a2.Size;

        public static bool operator <(Rect2D a1, Rect2D a2) => a2 > a1;

        public static bool operator >=(Rect2D a1, Rect2D a2) => !(a1 < a2);

        public static bool operator <=(Rect2D a1, Rect2D a2) => !(a1 > a2);

        #endregion

        #region Addition

        public static Rect2D operator +(Rect2D a1, Rect2D a2) => new(a1.Left + a2.Left, a1.Bottom + a2.Bottom, a1.Right + a2.Right, a1.Top + a2.Top);

        public static Rect2D operator +(Rect2D a1, Vector2 v) => new(a1.Left + v.X, a1.Bottom + v.Y, a1.Right + v.X, a1.Top + v.Y);

        #endregion

        #region Subtraction

        public static Rect2D operator -(Rect2D a1, Rect2D a2) => new(a1.Left - a2.Left, a1.Bottom - a2.Bottom, a1.Right - a2.Right, a1.Top - a2.Top);

        public static Rect2D operator -(Rect2D a1, Vector2 v) => new(a1.Left - v.X, a1.Bottom - v.Y, a1.Right - v.X, a1.Top - v.Y);

        public static Rect2D operator -(Rect2D a) => new(-a.Left, -a.Bottom, -a.Right, -a.Top);

        #endregion

        #region Multiplication

        public static Rect2D operator *(Rect2D a1, Rect2D a2) => new(a1.Left * a2.Left, a1.Bottom * a2.Bottom, a1.Right * a2.Right, a1.Top * a2.Top);

        public static Rect2D operator *(Rect2D a1, Vector2 v) => new(a1.Left * v.X, a1.Bottom * v.Y, a1.Right * v.X, a1.Top * v.Y);

        #endregion

        #region Division

        public static Rect2D operator /(Rect2D a1, Rect2D a2) => new(a1.Left / a2.Left, a1.Bottom / a2.Bottom, a1.Right / a2.Right, a1.Top / a2.Top);

        public static Rect2D operator /(Rect2D a1, Vector2 v) => new(a1.Left / v.X, a1.Bottom / v.Y, a1.Right / v.X, a1.Top / v.Y);

        #endregion

        #region Deconstruct

        public void Deconstruct(out float left, out float bottom, out float right, out float top)
        {
            left = Left;
            bottom = Bottom;
            right = Right;
            top = Top;
        }

        public void Deconstruct(out Vector2 start, out Vector2 end)
        {
            start = Start;
            end = End;
        }

        #endregion

        #region Utility

        public static bool Overlaps(float l1, float b1, float r1, float t1, float l2, float b2, float r2, float t2)
        {
            if (r1 <= l2 || l1 >= r2) // X sides don't overlap
            {
                return false;
            }
            if (t1 <= b2 || b1 >= t2) // Y sides don't overlap
            {
                return false;
            }
            return true;
        }

        public bool Overlaps(float l, float b, float r, float t)
        {
            return Overlaps(Left, Bottom, Right, Top, l, b, r, t);
        }

        public static bool Overlaps(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
        {
            return Overlaps(start1.X, start1.Y, end1.X, end1.Y, start2.X, start2.Y, end2.X, end2.Y);
        }

        public bool Overlaps(Vector2 start, Vector2 end)
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

        public static Rect2D GetOverlap(float l1, float b1, float r1, float t1, float l2, float b2, float r2, float t2)
        {
            if (!Overlaps(l1, b1, r1, t1, l2, b2, r2, t2))
            {
                throw new ArgumentException("Areas do not overlap.");
            }

            Vector2 v1 = new(Math.Max(l1, l2), Math.Max(b1, b2));

            Vector2 v2 = new(Math.Min(r1, r2), Math.Min(t1, t2));

            var start = Vector2.Min(v1, v2);

            var end = Vector2.Max(v1, v2);

            return new(start, end);
        }

        public static Rect2D GetOverlap(Rect2D a1, Rect2D a2)
        {
            return GetOverlap(a1.Left, a1.Bottom, a1.Right, a1.Top, a2.Left, a2.Bottom, a2.Right, a2.Top);
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
                area.Bottom < Bottom ? Bottom : area.Bottom,
                area.Right > Right ? Right : area.Right,
                area.Top > Top ? Top : area.Top);
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
            if (area.Right > Right)
            {
                offset.X += Right - area.Right;
            }
            if (area.Bottom < Bottom)
            {
                offset.Y += Bottom - area.Bottom;
            }
            if (area.Top > Top)
            {
                offset.Y += Top - area.Top;
            }

            return area + offset;
        }

        public bool Contains(Rect2D other)
        {
            return Left <= other.Left && Bottom <= other.Bottom && other.Right <= Right && other.Top <= Top;
        }

        public bool Contains(Vector2 position)
        {
            return Left <= position.X && Bottom <= position.Y && position.X < Right && position.Y < Top;
        }

        #endregion
    }
}
