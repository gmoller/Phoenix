using System.Diagnostics;
using Utilities.ExtensionMethods;

namespace Utilities
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct PointF
    {
        #region State
        public float X { get;  }
        public float Y { get; }
        #endregion

        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static PointF Empty => new PointF(0.0f, 0.0f);

        public PointI ToPoint()
        {
            return new PointI((X.Round()), Y.Round());
        }

        #region Overrides and Overloads

        public override bool Equals(object obj)
        {
            return obj is PointF point && this == point;
        }

        public static bool operator ==(PointF a, PointF b)
        {
            return a.X.AboutEquals(b.X) && a.Y.AboutEquals(b.Y);
        }

        public static bool operator !=(PointF a, PointF b)
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