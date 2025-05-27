namespace SCE
{
    public static class AnchorUtils
    {
        public const Anchor H_MASK = Anchor.Left | Anchor.Right  | Anchor.Center;
        public const Anchor V_MASK = Anchor.Top  | Anchor.Bottom | Anchor.Middle;

        public static int VerticalFix(Anchor anchor, int offset, int top = 0)
        {
            int off;
            bool mid = (anchor & Anchor.Middle) == Anchor.Middle;
            bool bottom = (anchor & Anchor.Bottom) == Anchor.Bottom;
            if (mid || bottom)
            {
                off = offset;
                if (mid)
                {
                    off = !bottom || off % 2 == 0 ? off / 2 : (off / 2) + 1;
                }
            }
            else
            {
                off = top;
            }
            return off;
        }

        public static int HorizontalFix(Anchor anchor, int offset, int left = 0)
        {
            int off;
            bool mid = (anchor & Anchor.Center) == Anchor.Center;
            bool right = (anchor & Anchor.Right) == Anchor.Right;
            if (mid || right)
            {
                off = offset;
                if (mid)
                {
                    off = !right || off % 2 == 0 ? off / 2 : (off / 2) + 1;
                }
            }
            else
            {
                off = left;
            }
            return off;
        }

        public static Vector2Int DimensionFix(Anchor anchor, Vector2Int dimensions)
        {
            return new(HorizontalFix(anchor, dimensions.X), VerticalFix(anchor, dimensions.Y));
        }

        public static Vector2Int AnchorTo(Anchor anchor, Vector2Int to, Vector2Int d)
        {
            return DimensionFix(anchor, to) - DimensionFix(anchor, d);
        }
    }
}
