namespace SCECore.Objects
{
    /// <summary>
    /// A struct representing a 2D rectangular area defined by start and end corner vectors.
    /// </summary>
    public readonly struct Area2D
    {
        private readonly Vector2 start;
        private readonly Vector2 end;

        /// <summary>
        /// Initializes a new instance of the <see cref="Area2D"/> struct given a start and end vector.
        /// </summary>
        /// <param name="start">The bottom left inclusive corner vector of the area.</param>
        /// <param name="end">The top right exclusive corner vector of the area.</param>
        /// <exception cref="ArgumentException">Throws exception if the end vector is less than or equal the start vector.</exception>
        public Area2D(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end > start ? end : throw new ArgumentException("End vector cannot be less than start vector");
        }

        /// <summary>
        /// Gets the bottom left inclusive corner of the area.
        /// </summary>
        public Vector2 Start => start;

        /// <summary>
        /// Gets the top right exclusive corner of the area.
        /// </summary>
        public Vector2 End => end;

        /// <summary>
        /// Gets the dimensions of the area.
        /// </summary>
        public Vector2 Dimensions => new(Width, Height);

        /// <summary>
        /// Gets the width of the area.
        /// </summary>
        public double Width => End.X + 1 - Start.X;

        /// <summary>
        /// Gets the height of the area.
        /// </summary>
        public double Height => End.Y + 1 - Start.Y;

        // Conversion
        public static implicit operator Area2DInt(Area2D a) => a.ToArea2DInt();

        // Equality
        public static bool operator ==(Area2D a1, Area2D a2) => a1.Start == a2.Start && a1.End == a2.End;

        public static bool operator !=(Area2D a1, Area2D a2) => !(a1 == a2);

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

        // Greater than
        public static bool operator >(Area2D a1, Area2D a2) => a1.Start > a2.Start && a1.End > a2.End;

        // Less than
        public static bool operator <(Area2D a1, Area2D a2) => a1.Start < a2.Start && a1.End < a2.End;

        // Greater or equal than
        public static bool operator >=(Area2D a1, Area2D a2) => a1.Start >= a2.Start && a1.End >= a2.End;

        // Less or equal than
        public static bool operator <=(Area2D a1, Area2D a2) => a1.Start <= a2.Start && a1.End <= a2.End;

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
            return TrimArea(area, out _);
        }

        /// <summary>
        /// Returns the given <paramref name="area"/> trimmed from this instance.
        /// </summary>
        /// <param name="area">The area to trim.</param>
        /// <param name="hasFixed">Outputs if the returned area has been trimmed.</param>
        /// <returns>The given <paramref name="area"/> trimmed from this instance.</returns>
        public Area2D TrimArea(Area2D area, out bool hasFixed)
        {
            area.Expose(out Vector2 trimStart, out Vector2 trimEnd);

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

            Area2D trimArea = new(trimStart, trimEnd);

            hasFixed = trimArea == area;

            return trimArea;
        }

        /// <summary>
        /// Indicates whether a given <paramref name="area"/> and this instance overlap eachother.
        /// </summary>
        /// <param name="area">Area to check with.</param>
        /// <returns><see langword="true"/> if <paramref name="area"/> and this instance overlap; otherwise, <see langword="false"/>.</returns>
        public bool OverlapsWith(Area2D area)
        {
            return !(area.End.X <= Start.X || area.End.Y <= Start.Y || area.Start.X >= End.X || area.Start.Y >= End.Y);
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
        /// Returns the ceiled area.
        /// </summary>
        /// <returns>The ceiled area.</returns>
        public Area2D Ceil() => new(Start.Ceil(), End.Ceil());

        /// <summary>
        /// Returns the floored area.
        /// </summary>
        /// <returns>The floored area.</returns>
        public Area2D Floor() => new(Start.Floor(), End.Floor());

        /// <summary>
        /// Returns the away from zero rounded area.
        /// </summary>
        /// <returns>The away from zero rounded area.</returns>
        public Area2D Round() => new(Start.Round(), End.Round());

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj != null && this == (Area2D)obj;

        /// <inheritdoc/>
        public override int GetHashCode() => base.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => $"({Start}),({End})";
    }
}
