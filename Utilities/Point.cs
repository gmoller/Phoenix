using System.Diagnostics;

namespace Utilities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct Point
    {
        #region State
        public int X { get; }
        public int Y { get; }
        #endregion

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point Empty => new Point(0, 0);

        #region Overrides and Overloads

        public override bool Equals(object obj)
        {
            return obj is Point point && this == point;
        }

        public static bool operator == (Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator != (Point a, Point b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{X={X},Y={Y}}}";

        #endregion

    }
}