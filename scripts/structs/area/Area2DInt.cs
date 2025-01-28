namespace SCE
{
    /// <summary>
    /// A struct representing a 2D rectangular area defined by start and end corner vectors.
    /// </summary>
    public struct Area2DInt : IEquatable<Area2DInt>
    {
        private Vector2Int start;
        private Vector2Int end;

        /// <summary>
        /// Initializes a new instance of the <see cref="Area2DInt"/> struct given a start and end vector.
        /// </summary>
        /// <param name="start">The bottom left inclusive corner vector of the area.</param>
        /// <param name="end">The top right exclusive corner vector of the area.</param>
        /// <exception cref="ArgumentException">Throws exception if the end vector is less than or equal the start vector.</exception>
        public Area2DInt(Vector2Int start, Vector2Int end)
        {
            if (Vector2Int.OrLessEqual(end, start))
                throw new ArgumentException("End vector must be greater than start vector.");
            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// Gets the bottom left start position of the area.
        /// </summary>
        public Vector2Int Start
        {
            get => start;
            set
            {
                if (Vector2Int.OrLessEqual(End, value))
                    throw new ArgumentException("End vector must be greater than start vector.");
                start = value;
            }
        }

        /// <summary>
        /// Gets the top right end position of the area.
        /// </summary>
        public Vector2Int End
        {
            get => end;
            set
            {
                if (Vector2Int.OrLessEqual(value, Start))
                    throw new ArgumentException("End vector must be greater than start vector.");
                end = value;
            }
        }

        /// <summary>
        /// Gets the width of the area.
        /// </summary>
        public int Width { get => End.X - Start.X; }

        /// <summary>
        /// Gets the height of the area.
        /// </summary>
        public int Height { get => End.Y - Start.Y; }

        /// <summary>
        /// Gets the dimensions of the area.
        /// </summary>
        public Vector2Int Dimensions { get => new(Width, Height); }

        // Conversion
        public static implicit operator Area2D(Area2DInt a) => a.ToArea2D();

        // Equality
        public static bool operator ==(Area2DInt left, Area2DInt right) => left.Equals(right);

        public static bool operator !=(Area2DInt left, Area2DInt right) => !(left == right);

        // Greater than
        public static bool operator >(Area2DInt a1, Area2DInt a2) => a1.Start > a2.Start && a1.End > a2.End;

        // Less than
        public static bool operator <(Area2DInt a1, Area2DInt a2) => a1.Start < a2.Start && a1.End < a2.End;

        // Greater or equal than
        public static bool operator >=(Area2DInt a1, Area2DInt a2) => a1.Start >= a2.Start && a1.End >= a2.End;

        // Less or equal than
        public static bool operator <=(Area2DInt a1, Area2DInt a2) => a1.Start <= a2.Start && a1.End <= a2.End;

        // Addition
        public static Area2DInt operator +(Area2DInt a1, Area2DInt a2) => new(a1.Start + a2.Start, a1.End + a2.End);

        public static Area2DInt operator +(Area2DInt a1, Vector2Int v) => new(a1.Start + v, a1.End + v);

        public static Area2DInt operator +(Area2DInt a1, int num) => a1 + new Vector2Int(num, num);

        // Subtraction
        public static Area2DInt operator -(Area2DInt a1, Area2DInt a2) => new(a1.Start - a2.Start, a1.End - a2.End);

        public static Area2DInt operator -(Area2DInt a1, Vector2Int v) => new(a1.Start - v, a1.End - v);

        public static Area2DInt operator -(Area2DInt a1, int num) => a1 - new Vector2Int(num, num);

        public static Area2DInt operator -(Area2DInt a) => new(-a.Start, -a.End);

        // Multiplication
        public static Area2DInt operator *(Area2DInt a1, Area2DInt a2) => new(a1.Start * a2.Start, a1.End * a2.End);

        public static Area2DInt operator *(Area2DInt a1, Vector2Int v) => new(a1.Start * v, a1.End + v);

        public static Area2DInt operator *(Area2DInt a1, int num) => a1 * new Vector2Int(num, num);

        // Division
        public static Area2DInt operator /(Area2DInt a1, Area2DInt a2) => new(a1.Start / a2.Start, a1.End / a2.End);

        public static Area2DInt operator /(Area2DInt a1, Vector2Int v) => new(a1.Start / v, a1.End + v);

        public static Area2DInt operator /(Area2DInt a1, int num) => a1 / new Vector2Int(num, num);

        /// <inheritdoc/>
        public bool Equals(Area2DInt area)
        {
            return area.Start == Start && area.End == End;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj != null && this == (Area2DInt)obj;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Start.GetHashCode(), End.GetHashCode());
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"({Start}),({End})";
        }

        public static bool Overlaps(Vector2Int start1, Vector2Int end1, Vector2Int start2, Vector2Int end2)
        {
            if (end1.X <= start2.X || start1.X >= end2.X) // X sides don't overlap
                return false;          
            if (end1.Y <= start2.Y || start1.Y >= end2.Y) // Y sides don't overlap
                return false;
            return true;
        }

        public static bool Overlaps(Area2DInt a1, Area2DInt a2)
        {
            return Overlaps(a1.Start, a1.End, a2.Start, a2.End);
        }

        public static Area2DInt GetOverlap(Vector2Int start1, Vector2Int end1, Vector2Int start2, Vector2Int end2)
        {
            if (!Overlaps(start1, end1, start2, end2))
                throw new ArgumentException("Areas do not overlap.");

            Vector2Int v1 = new(Math.Max(start1.X, start2.X), Math.Max(start1.Y, start2.Y));

            Vector2Int v2 = new(Math.Min(end1.X, end2.X), Math.Min(end1.Y, end2.Y));

            Vector2Int start = Vector2Int.Min(v1, v2);

            Vector2Int end = Vector2Int.Max(v1, v2);

            return new(start, end);
        }

        public static Area2DInt GetOverlap(Area2DInt a1, Area2DInt a2)
        {
            return GetOverlap(a1.Start, a1.End, a2.Start, a2.End);
        }

        /// <summary>
        /// Exposes the Start and End properties in this instance.
        /// </summary>
        /// <param name="start">The bottom left corner of the area.</param>
        /// <param name="end">The top right corner of the area.</param>
        public void Expose(out Vector2Int start, out Vector2Int end)
        {
            start = Start;
            end = End;
        }

        /// <summary>
        /// Returns the converted <see cref="Area2D"/> based on the current area.
        /// </summary>
        /// <returns>The converted <see cref="Area2D"/> based on the current area.</returns>
        public Area2D ToArea2D()
        {
            return new((Vector2)Start, (Vector2)End);
        }

        /// <summary>
        /// Returns the given <paramref name="area"/> trimmed from this instance.
        /// </summary>
        /// <param name="area">The area to trim.</param>
        /// <returns>The given <paramref name="area"/> trimmed from this instance.</returns>
        public Area2DInt TrimArea(Area2DInt area)
        {
            area.Expose(out Vector2Int trimStart, out Vector2Int trimEnd);

            if (trimEnd > Start)
            {
                if (trimStart.X < Start.X)
                    trimStart.X = Start.X;

                if (trimStart.Y < Start.Y)
                    trimStart.Y = Start.Y;
            }

            if (trimStart < End)
            {
                if (trimEnd.X >= End.X)
                    trimEnd.X = End.X;

                if (trimEnd.Y >= End.Y)
                    trimEnd.Y = End.Y;
            }

            Area2DInt trimArea = new(trimStart, trimEnd);

            return trimArea;
        }

        /// <summary>
        /// Returns the given <paramref name="area"/> trimmed from this instance.
        /// </summary>
        /// <param name="area">The area to trim.</param>
        /// <param name="hasModified">Outputs <see langword="true"/> if the <paramref name="area"/> has been modified; otherwise, <see langword="false"/>.</param>
        /// <returns>The given <paramref name="area"/> trimmed from this instance.</returns>
        public Area2DInt TrimArea(Area2DInt area, out bool hasModified)
        {
            Area2DInt trimmedArea = TrimArea(area);

            hasModified = trimmedArea != area;

            return trimmedArea;
        }

        /// <summary>
        /// Returns the specified area realigned to be bound inside this area.
        /// </summary>
        /// <param name="area">The area to bound inside this area.</param>
        /// <returns>The specified area realigned to be bound inside this area.</returns>
        public Area2DInt Bound(Area2DInt area)
        {
            Vector2Int offset = Vector2Int.Zero;

            if (area.Start.X < Start.X)
                offset.X += Start.X - area.Start.X;
            if (area.End.X > End.X)
                offset.X += End.X - area.End.X;
            if (area.Start.Y < Start.Y)
                offset.Y += Start.Y - area.Start.Y;
            if (area.End.Y > End.Y)
                offset.Y += End.Y - area.End.Y;

            return area + offset;
        }

        /// <summary>
        /// Returns the specified area realigned to be bound inside this area.
        /// </summary>
        /// <param name="area">The area to bound inside this area.</param>
        /// <param name="hasModified">Outputs <see langword="true"/> if the <paramref name="area"/> has been modified; otherwise, <see langword="false"/>.</param>
        /// <returns>The specified area realigned to be bound inside this area.</returns>
        public Area2DInt Bound(Area2DInt area, out bool hasModified)
        {
            Area2DInt boundArea = Bound(area);

            hasModified = boundArea != area;

            return boundArea;
        }

        /// <summary>
        /// Determines whether the specified area is contained inside this area.
        /// </summary>
        /// <param name="area">The area to check.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="area"/> is contained inside this area; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Area2DInt area)
        {
            return area.Start >= Start && area.End <= End;
        }

        /// <summary>
        /// Determines whether the specified position is contained inside this area.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="position"/> is contained inside this area; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Vector2Int position)
        {
            return position >= Start && position < End;
        }
    }
}
