namespace SCE
{
    public static class MathUtils
    {
        #region Middle

        /// <summary>
        /// Returns the middle number of the given values
        /// </summary>
        public static T Middle<T>(T a, T b, T c)
            where T : IComparable<T>
        {
            if (a.CompareTo(b) > 0)
            {
                if (a.CompareTo(c) < 0)
                {
                    return a;
                }
                return b.CompareTo(c) < 0 ? c : b;
            }
            if (b.CompareTo(c) < 0)
            {
                return b;
            }
            return a.CompareTo(c) < 0 ? c : a;
        }

        /// <summary>
        /// Reterns whether the given value is within the bounds.
        /// </summary>
        public static bool InMiddle<T>(T lower, T value, T upper)
            where T : IComparable<T>
        {
            if (lower.CompareTo(upper) > 0)
            {
                throw new ArgumentException("Lower bound cannot exceed upper bound.");
            }
            return lower.CompareTo(value) < 0 && upper.CompareTo(value) > 0;
        }

        #endregion

        public static float Square(float x)
        {
            return x * x;
        }

        public static bool QuadraticHasSolution(float a, float b, float c)
        {
            return Square(b) - (4.0f * a * c) >= 0.0f;
        }

        public static float[] Quadratic(float a, float b, float c)
        {
            var root = Square(b) - (4.0f * a * c);
            if (root < 0)
            {
                return Array.Empty<float>();
            }
            var s1 = (-b + MathF.Sqrt(root)) / (2.0f * a);
            var s2 = (-b - MathF.Sqrt(root)) / (2.0f * a);
            if (s1 == s2)
            {
                return new[] { s1 };
            }
            return new[] { s1, s2 };
        }

        #region Circle

        public static float[] CircleSolveX(float y, float a, float b, float r)
        {
            return Quadratic(1, -2.0f * a, Square(a) + Square(b) + (y * (y - (2.0f * b))) - Square(r));
        }

        public static float[] CircleSolveY(float x, float a, float b, float r)
        {
            return Quadratic(1, -2.0f * b, Square(a) + Square(b) + (x * (x - (2.0f * a))) - Square(r));
        }

        public static float[] CircleLineSolveX(float a, float b, float r, float m, float c)
        {
            return Quadratic(1.0f + Square(m), 2.0f * (m * (c - b) - a), Square(a) + Square(c) + b * (b - 2.0f * c) - Square(r));
        }

        public static Vector2[] CircleLineSolvePos(float a, float b, float r, float m, float c)
        {
            var xArr = CircleLineSolveX(a, b, r, m, c);
            var posArr = new Vector2[xArr.Length];
            for (int i = 0; i < xArr.Length; ++i)
            {
                posArr[i] = new(xArr[i], LineSolveY(xArr[i], m, c));
            }
            return posArr;
        }

        #endregion

        #region Line

        public static float LineSolveY(float x, float m, float c)
        {
            return (m * x) + c;
        }

        public static float LineSolveX(float y, float m, float c)
        {
            return (y - c) / m;
        }

        public static float LineSolveC(float x, float y, float m)
        {
            return y - (m * x);
        }

        public static float LineIntersectX(float m1, float c1, float m2, float c2)
        {
            return (c2 - c1) / (m1 - m2);
        }

        public static float LineVerticalY(float m, float c, float k)
        {
            return (m * k) + c;
        }

        public static float LineVerticalX(float m, float c, float k)
        {
            return LineSolveX(LineVerticalY(m, c, k), m, c);
        }

        #endregion
    }
}
