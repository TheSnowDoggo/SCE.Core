using CSUtils;
using System.Collections;

namespace SCE
{
    /// <summary>
    /// A struct representing a 2D rectangular area defined by start and end corner.
    /// </summary>
    public readonly struct Rect2DInt : IEnumerable<Vector2Int>, IEquatable<Rect2DInt>
    {
        public Rect2DInt(int left, int top, int right, int bottom)
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

        public Rect2DInt(int right, int bottom)
            : this(0, 0, right, bottom)
        {
        }

        public Rect2DInt(Vector2Int start, Vector2Int end)
            : this(start.X, start.Y, end.X, end.Y)
        {
        }

        public Rect2DInt(Vector2Int end)
            : this(Vector2Int.Zero, end)
        {
        }

        public int Left { get; }

        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }

        public int Width { get => Right - Left; }

        public int Height { get => Top - Bottom; }

        public Vector2Int Dimensions { get => new(Width, Height); }

        public int Size { get => Width * Height; }

        public Vector2Int Start { get => new(Left, Bottom); }

        public Vector2Int End { get => new(Right, Top); }

        public Rect2D ToRect2D()
        {
            return new(Left, Top, Right, Bottom);
        }

        public override string ToString()
        {
            return string.Join(",", Left, Top, Right, Bottom);
        }

        #region Equality

        public bool Equals(Rect2DInt area)
        {
            return Left == area.Left && Top == area.Top && Right == area.Right && Bottom == area.Bottom;
        }

        public override bool Equals(object? obj)
        {
            return obj is Rect2DInt area && Equals(area);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Top, Right, Bottom);
        }

        #endregion

        #region Operators

        public static implicit operator Rect2D(Rect2DInt r) => r.ToRect2D();

        public static bool operator ==(Rect2DInt left, Rect2DInt right) => left.Equals(right);

        public static bool operator !=(Rect2DInt left, Rect2DInt right) => !(left == right);

        public static bool operator >(Rect2DInt a1, Rect2DInt a2) => a1.Size > a2.Size;

        public static bool operator <(Rect2DInt a1, Rect2DInt a2) => a2 > a1;

        public static bool operator >=(Rect2DInt a1, Rect2DInt a2) => !(a1 < a2);

        public static bool operator <=(Rect2DInt a1, Rect2DInt a2) => !(a1 > a2);

        public static Rect2DInt operator +(Rect2DInt a1, Rect2DInt a2) => new(a1.Left + a2.Left, a1.Top + a2.Top, a1.Right + a2.Right, a1.Bottom + a2.Bottom);

        public static Rect2DInt operator +(Rect2DInt a1, Vector2Int v) => new(a1.Left + v.X, a1.Top + v.Y, a1.Right + v.X, a1.Bottom + v.Y);

        public static Rect2DInt operator -(Rect2DInt a1, Rect2DInt a2) => new(a1.Left - a2.Left, a1.Top - a2.Top, a1.Right - a2.Right, a1.Bottom - a2.Bottom);

        public static Rect2DInt operator -(Rect2DInt a1, Vector2Int v) => new(a1.Left - v.X, a1.Top - v.Y, a1.Right - v.X, a1.Bottom - v.Y);

        public static Rect2DInt operator -(Rect2DInt a) => new(-a.Left, -a.Top, -a.Right, -a.Bottom);

        public static Rect2DInt operator *(Rect2DInt a1, Rect2DInt a2) => new(a1.Left * a2.Left, a1.Top * a2.Top, a1.Right * a2.Right, a1.Bottom * a2.Bottom);

        public static Rect2DInt operator *(Rect2DInt a1, Vector2Int v) => new(a1.Left * v.X, a1.Top * v.Y, a1.Right * v.X, a1.Bottom * v.Y);

        public static Rect2DInt operator /(Rect2DInt a1, Rect2DInt a2) => new(a1.Left / a2.Left, a1.Top / a2.Top, a1.Right / a2.Right, a1.Bottom / a2.Bottom);

        public static Rect2DInt operator /(Rect2DInt a1, Vector2Int v) => new(a1.Left / v.X, a1.Top / v.Y, a1.Right / v.X, a1.Bottom / v.Y);

        #endregion

        #region Utility

        public static bool Overlaps(int l1, int t1, int r1, int b1, int l2, int t2, int r2, int b2)
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

        public bool Overlaps(int l, int t, int r, int b)
        {
            return Overlaps(Left, Top, Right, Bottom, l, t, r, b);
        }

        public static bool Overlaps(Vector2Int start1, Vector2Int end1, Vector2Int start2, Vector2Int end2)
        {
            return Overlaps(start1.X, start1.Y, end1.X, end1.Y, start2.X, start2.Y, end2.X, end2.Y);
        }

        public bool Overlaps(Vector2Int start, Vector2Int end)
        {
            return Overlaps(Left, Top, Right, Bottom, start.X, start.Y, end.X, end.Y);
        }

        public static bool Overlaps(Rect2DInt a1, Rect2DInt a2)
        {
            return Overlaps(a1.Left, a1.Top, a1.Right, a1.Bottom, a2.Left, a2.Top, a2.Right, a2.Bottom);
        }

        public bool Overlaps(Rect2DInt other)
        {
            return Overlaps(this, other);
        }

        public static Rect2DInt GetOverlap(int l1, int t1, int r1, int b1, int l2, int t2, int r2, int b2)
        {
            if (!Overlaps(l1, t1, r1, b1, l2, t2, r2, b2))
            {
                throw new ArgumentException("Areas do not overlap.");
            }

            return new(Math.Max(l1, l2), Math.Max(t1, t2), Math.Min(r1, r2), Math.Min(b1, b2));
        }

        public static Rect2DInt GetOverlap(Rect2DInt a1, Rect2DInt a2)
        {
            return GetOverlap(a1.Left, a1.Top, a1.Right, a1.Bottom, a2.Left, a2.Top, a2.Right, a2.Bottom);
        }

        public Rect2DInt GetOverlap(Rect2DInt other)
        {
            return GetOverlap(this, other);
        }

        private Rect2DInt[] SplitAway(Rect2DInt from, bool preferVertical = true)
        {
            if (!Overlaps(from))
            {
                return new[] { this };
            }

            if (from.Contains(this))
            {
                return Array.Empty<Rect2DInt>();
            }

            if (Contains(from))
            {
                return new Rect2DInt[]
                {
                    new(Left      , Top        , from.Left , from.Bottom),
                    new(from.Left , Top        , Right     , from.Top   ),
                    new(from.Right, from.Top   , Right     , Bottom     ),
                    new(Left      , from.Bottom, from.Right, Bottom     ),
                };
            }

            int hMid;
            int hOut;
            if (Utils.InMiddle(Left, from.Left, Right))
            {
                hMid = from.Left;
                hOut = Left;
            }
            else if (Utils.InMiddle(Left, from.Right, Right))
            {
                hMid = from.Right;
                hOut = Right;
            }
            else
            {
                if (Top < from.Top)
                {
                    return new Rect2DInt[] { new(Left, Top, Right, from.Top) };
                }
                return new Rect2DInt[] { new(Left, from.Bottom, Right, Bottom) };
            }

            int vMid;
            int vOut;
            if (Utils.InMiddle(Top, from.Top, Bottom))
            {
                vMid = from.Top;
                vOut = Top;
            }
            else if (Utils.InMiddle(Top, from.Bottom, Bottom))
            {
                vMid = from.Bottom;
                vOut = Bottom;
            }
            else
            {
                if (Left < from.Left)
                {
                    return new Rect2DInt[] { new(Left, Top, from.Left, Bottom) };
                }
                return new Rect2DInt[] { new(from.Right, Top, Right, Bottom) };
            }

            if (preferVertical)
            {
                return new Rect2DInt[]
                {
                    new(hOut, Top , hMid                       , Bottom),
                    new(hMid, vOut, hOut <= hMid ? Right : Left, vMid  ),
                };
            }

            return new Rect2DInt[]
            {
                new(Left, vOut, Right, vMid                       ),
                new(hMid, vMid, hOut , vOut <= vMid ? Bottom : Top),
            };
        }

        /// <summary>
        /// Returns the specified area realigned to be bound inside this area.
        /// </summary>
        /// <param name="area">The area to bound inside this area.</param>
        /// <returns>The specified area realigned to be bound inside this area.</returns>
        public Rect2DInt Bound(Rect2DInt area)
        {
            var offset = Vector2Int.Zero;

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

        public bool Contains(Rect2DInt other)
        {
            return Left <= other.Left && Top <= other.Top && other.Right <= Right && other.Bottom <= Bottom;
        }

        public bool Contains(Vector2Int position)
        {
            return Left <= position.X && Top <= position.Y && position.X < Right && position.Y < Bottom;
        }

        public void Deconstruct(out int left, out int top, out int right, out int bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }

        public void Deconstruct(out Vector2Int start, out Vector2Int end)
        {
            start = Start;
            end = End;
        }

        #endregion

        #region Lines

        public static Rect2DInt Horizontal(int y, int left, int right)
        {
            return new(left, y, right, y + 1);
        }

        public static Rect2DInt Horizontal(int y, int width)
        {
            return Horizontal(y, 0, width);
        }

        public static Rect2DInt Vertical(int x, int top, int bottom)
        {
            return new(x, top, x + 1, bottom);
        }

        public static Rect2DInt Vertical(int x, int height)
        {
            return Vertical(x, 0, height);
        }

        #endregion

        #region IEnumerable

        public IEnumerable<Vector2Int> Enumerate(bool rowMajor = true)
        {
            int s1 = rowMajor ? Top : Left;
            int s2 = rowMajor ? Left : Top;
            int e1 = rowMajor ? Bottom : Right;
            int e2 = rowMajor ? Right : Bottom;
            for (int i = s1; i < e1; ++i)
            {
                for (int j = s2; j < e2; ++j)
                {
                    yield return rowMajor ? new(j, i) : new(i, j);
                }
            }
        }

        public IEnumerable<Vector2Int> Enumerate(Rect2DInt area, bool rowMajor = true)
        {
            if (Overlaps(area))
            {
                foreach (var pos in GetOverlap(area).Enumerate(rowMajor))
                {
                    yield return pos;
                }
            }
        }

        public IEnumerator<Vector2Int> GetEnumerator()
        {
            return Enumerate().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
