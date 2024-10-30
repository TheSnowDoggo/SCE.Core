namespace SCECore.Utils
{
    using SCECore.Objects;

    /// <summary>
    /// A class containing functions useful for grid rotation.
    /// </summary>
    public static class SCERotation
    {
        private enum RotationType : byte
        {
            /// <summary>
            /// Top-right position relative to the rotation axis.
            /// </summary>
            TopRight,

            /// <summary>
            /// Bottom-right position relative to the rotation axis.
            /// </summary>
            BottomRight,

            /// <summary>
            /// Bottom-left position relative to the rotation axis.
            /// </summary>
            BottomLeft,

            /// <summary>
            /// Top-left position relative to the rotation axis.
            /// </summary>
            TopLeft,
        }

        public static Vector2Int RotationRange { get => new(0, 4); }

        public static Vector2 GetRotatedOffsetPosition(Vector2Int position, int rotationFactor, Vector2 rotationAxis)
        {
            if (Math.Abs(rotationFactor) != 1)
            {
                throw new ArgumentException("Rotation factor must have a magnitude of 1");
            }

            RotationType rotationType, newRotation;

            Vector2 offsetPos = position.ToVector2() - rotationAxis;

            rotationType = GetOffsetRotation(offsetPos);

            newRotation = GetNewRotation(rotationType, rotationFactor);

            Vector2 newSign = GetRotationOffsetSign(newRotation);

            return offsetPos.Abs().Inverse * newSign;
        }

        public static int GetNewRotation(int rotation, int direction)
        {
            return SCEMathUtility.CutShift(RotationRange, rotation, direction);
        }

        private static RotationType GetNewRotation(RotationType rotation, int rotationFactor)
        {
            return (RotationType)GetNewRotation((int)rotation, rotationFactor);
        }

        private static RotationType GetOffsetRotation(Vector2 offsetPosition)
        {
            return (offsetPosition.X >= 0, offsetPosition.Y >= 0) switch
            {
                (true, true) => RotationType.TopRight,
                (true, false) => RotationType.BottomRight,
                (false, false) => RotationType.BottomLeft,
                (false, true) => RotationType.TopLeft,
            };
        }

        private static Vector2 GetRotationOffsetSign(RotationType rotation)
        {
            return rotation switch
            {
                RotationType.TopRight => new(1, 1),
                RotationType.BottomRight => new(1, -1),
                RotationType.BottomLeft => new(-1, -1),
                RotationType.TopLeft => new(-1, 1),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
