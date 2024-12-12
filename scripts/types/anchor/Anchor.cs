namespace SCE
{
    public struct Anchor : IEquatable<Anchor>
    {
        public Anchor(AnchorMode mode, Vector2Int offset)
        {
            Mode = mode;
            Offset = offset;
        }

        public Anchor(AnchorMode mode)
            : this(mode, Vector2Int.Zero)
        {
        }

        public Anchor(Vector2Int offset)
            : this(default, offset)
        {
        }

        public static bool operator ==(Anchor left, Anchor right) => left.Equals(right);

        public static bool operator !=(Anchor left, Anchor right) => !(left == right);

        public AnchorMode Mode { get; set; }

        public Vector2Int Offset { get; set; }

        public bool Equals(Anchor other)
        {
            return other.Mode == Mode && other.Offset == Offset;
        }

        public override bool Equals(object? obj)
        {
            return obj is Anchor anchor && Equals(anchor);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Mode, Offset);
        }

        public override string ToString()
        {
            return $"{Mode} | {Offset}";
        }

        /// <summary>
        /// Returns the aligned offset based on the specified dimensions and the current anchor mode and offset.
        /// </summary>
        /// <param name="dimensions">The dimensions to anchor from.</param>
        /// <returns>The aligned offset based on the specified dimensions and the current anchor mode and offset.</returns>
        public Vector2Int GetAlignedOffset(Vector2Int dimensions)
        {
            Vector2Int offset = Mode switch
            {
                AnchorMode.BotttomLeft => Vector2Int.Zero,
                AnchorMode.BottomRight => new(dimensions.X - 1, 0),
                AnchorMode.TopLeft => new(0, dimensions.Y - 1),
                AnchorMode.TopRight => dimensions - 1,
                AnchorMode.Center => dimensions.Midpoint,
                _ => throw new NotImplementedException("Unknown anchor type.")
            };

            return -offset + Offset;
        }

        /// <summary>
        /// Represents the method used to anchor the offset.
        /// </summary>
        public enum AnchorMode : byte
        {
            /// <summary>
            /// Bottom left anchoring.
            /// </summary>
            BotttomLeft,

            /// <summary>
            /// Bottom right anchoring.
            /// </summary>
            BottomRight,

            /// <summary>
            /// Top left anchoring.
            /// </summary>
            TopLeft,

            /// <summary>
            /// Top right anchoring.
            /// </summary>
            TopRight,

            /// <summary>
            /// Center anchoring.
            /// </summary>
            Center,
        }
    }
}
