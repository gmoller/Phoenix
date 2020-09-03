using System.Diagnostics;

namespace Utilities
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct PointI
    {
        #region State
        public int X { get; }
        public int Y { get; }
        #endregion

        public PointI(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static PointI Empty => new PointI(0, 0);
        public static PointI Zero => new PointI(0, 0);

        #region Overrides and Overloads

        public override bool Equals(object obj)
        {
            return obj is PointI point && this == point;
        }

        public static bool operator ==(PointI a, PointI b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(PointI a, PointI b)
        {
            return !(a == b);
        }

        public static PointI operator +(PointI a, PointI b)
        {
            return new PointI(a.X + b.X, a.Y + b.Y);
        }

        public static PointI operator -(PointI a, PointI b)
        {
            return new PointI(a.X - b.X, a.Y - b.Y);
        }

        public static PointI operator *(PointI a, PointI b)
        {
            return new PointI(a.X * b.X, a.Y * b.Y);
        }

        public static PointI operator *(PointI a, int scalar)
        {
            return new PointI(a.X * scalar, a.Y * scalar);
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