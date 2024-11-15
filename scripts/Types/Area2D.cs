namespace SCECore.Types
{
    /// <summary>
    /// A struct representing a 2D rectangular area defined by start and end corner vectors.
    /// </summary>
    public struct Area2D : IEquatable<Area2D>
    {
        private Vector2 start;
        private Vector2 end;

        /// <summary>
        /// Initializes a new instance of the <see cref="Area2D"/> struct given a start and end vector.
        /// </summary>
        /// <param name="start">The bottom left inclusive corner vector of the area.</param>
        /// <param name="end">The top right exclusive corner vector of the area.</param>
        /// <exception cref="ArgumentException">Throws exception if the end vector is less than or equal the start vector.</exception>
        public Area2D(Vector2 start, Vector2 end)
        {
            if (end <= start)
            {
                throw new ArgumentException("End vector must be greater than start vector.");
            }

            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// Gets the bottom left start position of the area.
        /// </summary>
        public Vector2 Start
        {
            get => start;
            set => start = value;
        }

        /// <summary>
        /// Gets the top right end position of the area.
        /// </summary>
        public Vector2 End
        {
            get => end;
            set
            {
                if (value <= Start)
                {
                    throw new ArgumentException("End vector must be greater than start vector.");
                }

                end = value;
            }
        }

        /// <summary>
        /// Gets the width of the area.
        /// </summary>
        public double Width { get => End.X - Start.X; }

        /// <summary>
        /// Gets the height of the area.
        /// </summary>
        public double Height { get => End.Y - Start.Y; }

        /// <summary>
        /// Gets the dimensions of the area.
        /// </summary>
        public Vector2 Dimensions { get => new(Width, Height); }

        // Conversion
        public static explicit operator Area2DInt(Area2D a) => a.ToArea2DInt();

        // Equality
        public static bool operator ==(Area2D a1, Area2D a2) => a1.Start == a2.Start && a1.End == a2.End;

        public static bool operator !=(Area2D a1, Area2D a2) => !(a1 == a2);

        // Greater than
        public static bool operator >(Area2D a1, Area2D a2) => a1.Start > a2.Start && a1.End > a2.End;

        // Less than
        public static bool operator <(Area2D a1, Area2D a2) => a1.Start < a2.Start && a1.End < a2.End;

        // Greater or equal than
        public static bool operator >=(Area2D a1, Area2D a2) => a1.Start >= a2.Start && a1.End >= a2.End;

        // Less or equal than
        public static bool operator <=(Area2D a1, Area2D a2) => a1.Start <= a2.Start && a1.End <= a2.End;

        // Addition
        public static Area2D operator +(Area2D a1, Area2D a2) => new(a1.Start + a2.Start, a1.End + a2.End);

        public static Area2D operator +(Area2D a1, Vector2 v) => new(a1.Start + v, a1.End + v);

        public static Area2D operator +(Area2D a1, double num) => a1 + new Vector2(num, num);

        public static Area2D operator +(Area2D a1, int num) => a1 + (double)num;

        // Subtraction
        public static Area2D operator -(Area2D a1, Area2D a2) => new(a1.Start - a2.Start, a1.End - a2.End);

        public static Area2D operator -(Area2D a1, Vector2 v) => new(a1.Start - v, a1.End - v);

        public static Area2D operator -(Area2D a1, double num) => a1 - new Vector2(num, num);

        public static Area2D operator -(Area2D a1, int num) => a1 - (double)num;

        public static Area2D operator -(Area2D a) => new(-a.Start, -a.End);

        // Multiplication
        public static Area2D operator *(Area2D a1, Area2D a2) => new(a1.Start * a2.Start, a1.End * a2.End);

        public static Area2D operator *(Area2D a1, Vector2 v) => new(a1.Start * v, a1.End + v);

        public static Area2D operator *(Area2D a1, double num) => a1 * new Vector2(num, num);

        public static Area2D operator *(Area2D a1, int num) => a1 * (double)num;

        // Division
        public static Area2D operator /(Area2D a1, Area2D a2) => new(a1.Start / a2.Start, a1.End / a2.End);

        public static Area2D operator /(Area2D a1, Vector2 v) => new(a1.Start / v, a1.End + v);

        public static Area2D operator /(Area2D a1, double num) => a1 / new Vector2(num, num);

        public static Area2D operator /(Area2D a1, int num) => a1 / (double)num;

        public static bool Overlaps(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
        {
            // X sides don't overlap
            if (end1.X <= start2.X || start1.X >= end2.X)
            {
                return false;
            }
            // Y sides don't overlap
            if (end1.Y <= start2.Y || start1.Y >= end2.Y)
            {
                return false;
            }

            return true;
        }

        public static bool Overlaps(Area2D a1, Area2D a2)
        {
            return Overlaps(a1.Start, a1.End, a2.Start, a2.End);
        }

        public static Area2D GetOverlap(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
        {
            if (!Overlaps(start1, end1, start2, end2))
            {
                throw new ArgumentException("Areas do not overlap.");
            }

            Vector2 v1 = new(Math.Max(start1.X, start2.X), Math.Max(start1.Y, start2.Y));

            Vector2 v2 = new(Math.Min(end1.X, end2.X), Math.Min(end1.Y, end2.Y));

            Vector2 start = Vector2.Min(v1, v2);

            Vector2 end = Vector2.Max(v1, v2);

            return new(start, end);
        }

        public static Area2D GetOverlap(Area2D a1, Area2D a2)
        {
            return GetOverlap(a1.Start, a1.End, a2.Start, a2.End);
        }

        public bool Equals(Area2D other)
        {
            return other.Start == Start && other.End == End;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj != null && this == (Area2D)obj;
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

        /// <summary>
        /// Exposes the Start and End properties in this instance.
        /// </summary>
        /// <param name="start">The bottom left corner of the area.</param>
        /// <param name="end">The top right corner of the area.</param>
        public void Expose(out Vector2 start, out Vector2 end)
        {
            start = Start;
            end = End;
        }

        /// <summary>
        /// Returns the converted <see cref="Area2DInt"/> based on the current area.
        /// </summary>
        /// <returns>The converted <see cref="Area2DInt"/> based on the current area.</returns>
        public Area2DInt ToArea2DInt()
        {
            Vector2Int end = (Vector2Int)End;
            Vector2Int start = (Vector2Int)Start;

            if (end <= start)
            {
                throw new InvalidAreaException("Converted area is invalid.");
            }

            return new(start, end);
        }

        /// <summary>
        /// Returns the given <paramref name="area"/> trimmed from this instance.
        /// </summary>
        /// <param name="area">The area to trim.</param>
        /// <returns>The given <paramref name="area"/> trimmed from this instance.</returns>
        public Area2D TrimArea(Area2D area)
        {
            area.Expose(out Vector2 trimStart, out Vector2 trimEnd);

            if (trimEnd > Start)
            {
                if (trimStart.X < Start.X)
                {
                    trimStart.X = Start.X;
                }

                if (trimStart.Y < Start.Y)
                {
                    trimStart.Y = Start.Y;
                }
            }

            if (trimStart < End)
            {
                if (trimEnd.X > End.X)
                {
                    trimEnd.X = End.X;
                }

                if (trimEnd.Y > End.Y)
                {
                    trimEnd.Y = End.Y;
                }
            }

            return new(trimStart, trimEnd); 
        }

        /// <summary>
        /// Returns the given <paramref name="area"/> trimmed from this instance.
        /// </summary>
        /// <param name="area">The area to trim.</param>
        /// <param name="hasFixed">Outputs if the returned area has been trimmed.</param>
        /// <returns>The given <paramref name="area"/> trimmed from this instance.</returns>
        public Area2D TrimArea(Area2D area, out bool hasFixed)
        {
            Area2D newArea = TrimArea(area);

            hasFixed = newArea == area;

            return newArea;
        }

        /// <summary>
        /// Returns the specified area realigned to be bound inside this area.
        /// </summary>
        /// <param name="area">The area to bound inside this area.</param>
        /// <returns>The specified area realigned to be bound inside this area.</returns>
        public Area2D Bound(Area2D area)
        {
            Vector2 offset = Vector2.Zero;

            if (area.Start.X < Start.X)
            {
                offset.X += Start.X - area.Start.X;
            }

            if (area.End.X > End.X)
            {
                offset.X += End.X - area.End.X;
            }

            if (area.Start.Y < Start.Y)
            {
                offset.Y += Start.Y - area.Start.Y;
            }

            if (area.End.Y > End.Y)
            {
                offset.Y += End.Y - area.End.Y;
            }

            return area + offset;
        }

        /// <summary>
        /// Returns the specified area realigned to be bound inside this area.
        /// </summary>
        /// <param name="area">The area to bound inside this area.</param>
        /// <param name="hasModified">Outputs <see langword="true"/> if the <paramref name="area"/> has been modified; otherwise, <see langword="false"/>.</param>
        /// <returns>The specified area realigned to be bound inside this area.</returns>
        public Area2D Bound(Area2D area, out bool hasModified)
        {
            Area2D boundArea = Bound(area);

            hasModified = boundArea != area;

            return boundArea;
        }

        /// <summary>
        /// Determines whether the specified area is contained inside this area.
        /// </summary>
        /// <param name="area">The area to check.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="area"/> is contained inside this area; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Area2D area)
        {
            return area.Start >= Start && area.End <= End;
        }

        /// <summary>
        /// Determines whether the specified position is contained inside this area.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="position"/> is contained inside this area; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Vector2 position)
        {
            return position >= Start && position < End;
        }

        /// <summary>
        /// Returns the ceiled area.
        /// </summary>
        /// <returns>The ceiled area.</returns>
        public Area2D Ceil()
        {
            return new(Start.Ceil(), End.Ceil());
        }

        /// <summary>
        /// Returns the floored area.
        /// </summary>
        /// <returns>The floored area.</returns>
        public Area2D Floor()
        {
            return new(Start.Floor(), End.Floor());
        }

        /// <summary>
        /// Returns the away from zero rounded area.
        /// </summary>
        /// <returns>The away from zero rounded area.</returns>
        public Area2D Round()
        {
            return new(Start.Round(), End.Round());
        }
    }
}
