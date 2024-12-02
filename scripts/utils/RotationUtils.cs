namespace SCE
{
    /// <summary>
    /// A class containing functions useful for grid rotation.
    /// </summary>
    public static class RotationUtils
    {
        public static Vector2Int RotationRange { get => new(0, 4); }

        public static Vector2 RotatePositionBy(Vector2 position, int rotationFactor, Vector2 rotationAxis)
        {
            rotationFactor = MathUtils.Mod(rotationFactor, RotationRange.Y);

            int direction = 1;

            if (rotationFactor > 3)
            {
                rotationFactor = 1;
                direction = -1;
            }

            Vector2 newPos = position;
            for (int i = 0; i < rotationFactor; i++)
            {
                newPos = GetRotatedOffsetPosition(newPos, direction, rotationAxis);
            }

            return newPos;
        }

        public static Vector2 GetRotatedOffsetPosition(Vector2 position, int direction, Vector2 rotationAxis)
        {
            if (Math.Abs(direction) != 1)
            {
                throw new ArgumentException("Direction must be either -1 or 1");
            }

            RotationType rotationType, newRotation;

            Vector2 offsetPos = position - rotationAxis;

            rotationType = GetOffsetRotation(offsetPos);

            newRotation = GetNewRotation(rotationType, direction);

            Vector2 newSign = GetRotationOffsetSign(newRotation);

            return offsetPos.Abs().Inverse * newSign;
        }

        public static int GetNewRotation(int rotation, int direction)
        {
            return MathUtils.CutShift(RotationRange, rotation, direction);
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
    }
}
