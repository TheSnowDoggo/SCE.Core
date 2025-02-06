namespace SCE
{
    public static class AnchorUtils
    {
        #region DimensionAnchor

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

        #endregion

        #region LineAnchor

        public static string GetHorizontalAnchoredMessage(HorizontalAnchor anchor, string msg, int width)
        {
            return anchor switch
            {
                HorizontalAnchor.Left => StringUtils.PostFitToLength(msg, width),
                HorizontalAnchor.Right => StringUtils.PreFitToLength(msg, width),
                HorizontalAnchor.CenterLeftBias => StringUtils.Copy(' ', HorizontalAnchoredStart(HorizontalAnchor.CenterLeftBias, msg.Length, width))
                + msg + StringUtils.Copy(' ', HorizontalAnchoredStart(HorizontalAnchor.CenterRightBias, msg.Length, width)),
                HorizontalAnchor.CenterRightBias => StringUtils.Copy(' ', HorizontalAnchoredStart(HorizontalAnchor.CenterRightBias, msg.Length, width))
                + msg + StringUtils.Copy(' ', HorizontalAnchoredStart(HorizontalAnchor.CenterLeftBias, msg.Length, width)),
                _ => throw new NotImplementedException()
            };
        }

        public static int HorizontalAnchoredStart(HorizontalAnchor anchor, int lineLength, int width)
        {
            return anchor switch
            {
                HorizontalAnchor.Left => 0,
                HorizontalAnchor.Right => width - lineLength,
                HorizontalAnchor.CenterLeftBias => (width - lineLength) / 2,
                HorizontalAnchor.CenterRightBias => (int)Math.Ceiling((width - lineLength) / 2.0),
                _ => throw new NotImplementedException()
            };
        }

        public static int VerticalAnchoredStart(VerticalAnchor anchor, int lines, int height)
        {
            return anchor switch
            {
                VerticalAnchor.Top => height,
                VerticalAnchor.Bottom => lines,
                VerticalAnchor.MiddleTopBias => lines + (int)Math.Ceiling((height - lines) / 2.0),
                VerticalAnchor.MiddleBottomBias => lines + (height - lines) / 2,
                _ => throw new NotImplementedException()
            };
        }

        #endregion
    }
}
