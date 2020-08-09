using System.Diagnostics;

namespace Utilities
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct CostToMoveIntoResult
    {
        public bool CanMoveInto { get; }
        public float CostToMoveInto { get; }

        public CostToMoveIntoResult(bool canMoveInto)
        {
            CanMoveInto = canMoveInto;
            CostToMoveInto = 0.0f;
        }

        public CostToMoveIntoResult(bool canMoveInto, float costToMoveInto)
        {
            CanMoveInto = canMoveInto;
            CostToMoveInto = costToMoveInto;
        }

        #region Overrides and Overloads

        public override bool Equals(object obj)
        {
            return obj is CostToMoveIntoResult costToMoveIntoResult && this == costToMoveIntoResult;
        }

        public override int GetHashCode()
        {
            return CanMoveInto.GetHashCode() ^ CostToMoveInto.GetHashCode();
        }

        public static bool operator == (CostToMoveIntoResult a, CostToMoveIntoResult b)
        {
            return a.CanMoveInto == b.CanMoveInto && a.CostToMoveInto.AboutEquals(b.CostToMoveInto);
        }

        public static bool operator != (CostToMoveIntoResult a, CostToMoveIntoResult b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{CanMoveInto={CanMoveInto},CostToMoveInto={CostToMoveInto}}}";

        #endregion
    }
}