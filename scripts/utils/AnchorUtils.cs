namespace SCE
{
    public static class AnchorUtils
    {
        public static Vector2Int AnchoredDimension(Anchor anchor, Vector2Int dimensions)
        {
            return anchor switch
            {
                Anchor.BottomLeft => Vector2Int.Zero,
                Anchor.BottomCenter => new(dimensions.X / 2, 0),
                Anchor.BottomRight => new(dimensions.X, 0),
                Anchor.MiddleLeft => new(0, dimensions.Y / 2),
                Anchor.MiddleCenter => dimensions / 2,
                Anchor.MiddleRight => new(dimensions.X, dimensions.Y / 2),
                Anchor.TopLeft => new(0, dimensions.Y),
                Anchor.TopCenter => new(dimensions.X / 2, dimensions.Y),
                Anchor.TopRight => dimensions,
                _ => throw new NotImplementedException("Unknown anchor type.")
            };
        }

        public static Vector2Int AnchorTo(Anchor anchor, Vector2Int to, Vector2Int d)
        {
            return AnchoredDimension(anchor, to) - AnchoredDimension(anchor, d);
        }
    }
}
