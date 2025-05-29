using CSUtils;

namespace SCE
{
    public static class AnchorUtils
    {
        public const Anchor H_MASK = Anchor.Right  | Anchor.Center;
        public const Anchor V_MASK = Anchor.Bottom | Anchor.Middle;

        public static FillType ToFillType(Anchor anchor)
        {
            bool right = (anchor & Anchor.Right) == Anchor.Right;
            bool mid = (anchor & Anchor.Center) == Anchor.Center;
            // Note the fill types are reversed to anchor the text, not the fill.
            if (mid)
            {
                return right ? FillType.CenterLB : FillType.CenterRB;
            }
            else
            {
                return right ? FillType.Left : FillType.Right;
            }
        }

        public static int VerticalFix(Anchor anchor, int offset, int top, bool invertBias = false)
        {
            int off;
            bool mid = (anchor & Anchor.Middle) == Anchor.Middle;
            bool bottom = (anchor & Anchor.Bottom) == Anchor.Bottom;
            if (mid || bottom)
            {
                off = offset;
                if (mid)
                {
                    off = (invertBias ? bottom : !bottom) || off % 2 == 0 ? off / 2 : (off / 2) + 1;
                }
            }
            else
            {
                off = top;
            }
            return off;
        }

        public static int VerticalFix(Anchor anchor, int offset, bool invertBias = false)
        {
            return VerticalFix(anchor, offset, 0, invertBias);
        }

        public static int HorizontalFix(Anchor anchor, int offset, int left, bool invertBias = false)
        {
            int off;
            bool mid = (anchor & Anchor.Center) == Anchor.Center;
            bool right = (anchor & Anchor.Right) == Anchor.Right;
            if (mid || right)
            {
                off = offset;
                if (mid)
                {
                    off = (invertBias ? right : !right) || off % 2 == 0 ? off / 2 : (off / 2) + 1;
                }
            }
            else
            {
                off = left;
            }
            return off;
        }

        public static int HorizontalFix(Anchor anchor, int offset, bool invertBias = false)
        {
            return HorizontalFix(anchor, offset, 0, invertBias);
        }

        public static float VerticalFix(Anchor anchor, float offset)
        {
            bool mid = (anchor & Anchor.Middle) == Anchor.Middle;
            bool bottom = (anchor & Anchor.Bottom) == Anchor.Bottom;
            if (bottom || mid)
            {
                return mid ? offset / 2 : offset;
            }
            return 0;
        }

        public static float HorizontalFix(Anchor anchor, float offset)
        {
            bool mid = (anchor & Anchor.Center) == Anchor.Center;
            bool right = (anchor & Anchor.Right) == Anchor.Right;
            if (right || mid)
            {
                return mid ? offset / 2 : offset;
            }
            return 0;
        }

        public static Vector2Int DimensionFix(Anchor anchor, Vector2Int dimensions, bool invertBias = false)
        {
            return new(HorizontalFix(anchor, dimensions.X, invertBias), VerticalFix(anchor, dimensions.Y, invertBias));
        }

        public static Vector2 DimensionFix(Anchor anchor, Vector2 dimensions)
        {
            return new(HorizontalFix(anchor, dimensions.X), VerticalFix(anchor, dimensions.Y));
        }

        public static Vector2Int AnchorTo(Anchor anchor, Vector2Int to, Vector2Int d)
        {
            return DimensionFix(anchor, to) - DimensionFix(anchor, d, true);
        }

        public static Vector2 AnchorTo(Anchor anchor, Vector2 to, Vector2 d)
        {
            return DimensionFix(anchor, to) - DimensionFix(anchor, d);
        }
    }
}
