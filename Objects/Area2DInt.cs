namespace SCECore.Objects
{
    /// <summary>
    /// A struct representing a 2D rectangular area defined by start and end corner vectors.
    /// </summary>
    public readonly struct Area2DInt
    {
        private readonly Vector2Int start;
        private readonly Vector2Int end;

        /// <summary>
        /// Initializes a new instance of the <see cref="Area2DInt"/> struct given a start and end vector.
        /// </summary>
        /// <param name="start">The bottom left inclusive corner vector of the area.</param>
        /// <param name="end">The top right exclusive corner vector of the area.</param>
        /// <exception cref="ArgumentException">Throws exception if the end vector is less than or equal the start vector.</exception>
        public Area2DInt(Vector2Int start, Vector2Int end)
        {
            this.start = start;
            this.end = end > start ? end : throw new ArgumentException("End vector cannot be less than start vector");
        }

        /// <summary>
        /// Gets the bottom left inclusive corner of the area.
        /// </summary>
        public Vector2Int Start => start;

        /// <summary>
        /// Gets the top right exclusive corner of the area.
        /// </summary>
        public Vector2Int End => end;

        /// <summary>
        /// Gets the dimensions of the area.
        /// </summary>
        public Vector2Int Dimensions => new(Width, Height);

        /// <summary>
        /// Gets the width of the area.
        /// </summary>
        public int Width => End.X + 1 - Start.X;

        /// <summary>
        /// Gets the height of the area.
        /// </summary>
        public int Height => End.Y + 1 - Start.Y;

        // Conversion
        public static explicit operator Area2D(Area2DInt a) => a.ToArea2D();

        // Equality
        public static bool operator ==(Area2DInt a1, Area2DInt a2) => a1.Start == a2.Start && a1.End == a2.End;

        public static bool operator !=(Area2DInt a1, Area2DInt a2) => !(a1 == a2);

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

        // Greater than
        public static bool operator >(Area2DInt a1, Area2DInt a2) => a1.Start > a2.Start && a1.End > a2.End;

        // Less than
        public static bool operator <(Area2DInt a1, Area2DInt a2) => a1.Start < a2.Start && a1.End < a2.End;

        // Greater or equal than
        public static bool operator >=(Area2DInt a1, Area2DInt a2) => a1.Start >= a2.Start && a1.End >= a2.End;

        // Less or equal than
        public static bool operator <=(Area2DInt a1, Area2DInt a2) => a1.Start <= a2.Start && a1.End <= a2.End;

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj != null && this == (Area2DInt)obj;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"({Start}),({End})";
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
                {
                    trimStart = new(Start.X, trimStart.Y);
                }

                if (trimStart.Y < Start.Y)
                {
                    trimStart = new(trimStart.X, Start.Y);
                }
            }

            if (trimStart < End)
            {
                if (trimEnd.X >= End.X)
                {
                    trimEnd = new(End.X, trimEnd.Y);
                }

                if (trimEnd.Y >= End.Y)
                {
                    trimEnd = new(trimEnd.X, End.Y);
                }
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
        /// Indicates whether a given <paramref name="area"/> and this instance overlap eachother.
        /// </summary>
        /// <param name="area">Area to check with.</param>
        /// <returns><see langword="true"/> if <paramref name="area"/> and this instance overlap; otherwise, <see langword="false"/>.</returns>
        public bool OverlapsWith(Area2DInt area)
        {
            return !(area.End.X <= Start.X || area.End.Y <= Start.Y || area.Start.X >= End.X || area.Start.Y >= End.Y);
        }

        /// <summary>
        /// Returns the specified area realigned to be bound inside this area.
        /// </summary>
        /// <param name="area">The area to bound inside this area.</param>
        /// <returns>The specified area realigned to be bound inside this area.</returns>
        public Area2DInt Bound(Area2DInt area)
        {
            int xOffset = 0, yOffset = 0;

            if (area.Start.X < Start.X)
            {
                xOffset += Start.X - area.Start.X;
            }

            if (area.End.X > End.X)
            {
                xOffset += End.X - area.End.X;
            }

            if (area.Start.Y < Start.Y)
            {
                yOffset += Start.Y - area.Start.Y;
            }

            if (area.End.Y > End.Y)
            {
                yOffset += End.Y - area.End.Y;
            }

            Vector2Int offset = new(xOffset, yOffset);

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
        /// Returns the area of overlap of this area and the specified area.
        /// </summary>
        /// <param name="area">The other overlapping area.</param>
        /// <returns>The area of overlap of this area and the specified area.</returns>
        /// <exception cref="ArgumentException">Thrown if the specified <paramref name="area"/> doesn't overlap with this area.</exception>
        public Area2DInt GetOverlap(Area2DInt area)
        {
            if (OverlapsWith(area))
            {
                Vector2Int v1 = new(Math.Max(Start.X, area.Start.X), Math.Max(Start.Y, area.Start.Y));

                Vector2Int v2 = new(Math.Min(End.X, area.End.X), Math.Min(End.Y, area.End.Y));

                Vector2Int start = Vector2Int.Min(v1, v2);

                Vector2Int end = Vector2Int.Max(v1, v2);

                return new(start, end);
            }

            throw new ArgumentException("Areas do not overlap.");
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
    }
}
