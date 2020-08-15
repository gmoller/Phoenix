using System.Diagnostics;
using Utilities.ExtensionMethods;

namespace Utilities
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct GetCostToMoveIntoResult
    {
        public bool CanMoveInto { get; }
        public float CostToMoveInto { get; }

        public GetCostToMoveIntoResult(bool canMoveInto)
        {
            CanMoveInto = canMoveInto;
            CostToMoveInto = 0.0f;
        }

        public GetCostToMoveIntoResult(bool canMoveInto, float costToMoveInto)
        {
            CanMoveInto = canMoveInto;
            CostToMoveInto = costToMoveInto;
        }

        #region Overrides and Overloads

        public override bool Equals(object obj)
        {
            return obj is GetCostToMoveIntoResult costToMoveIntoResult && this == costToMoveIntoResult;
        }

        public override int GetHashCode()
        {
            return CanMoveInto.GetHashCode() ^ CostToMoveInto.GetHashCode();
        }

        public static bool operator == (GetCostToMoveIntoResult a, GetCostToMoveIntoResult b)
        {
            return a.CanMoveInto == b.CanMoveInto && a.CostToMoveInto.AboutEquals(b.CostToMoveInto);
        }

        public static bool operator != (GetCostToMoveIntoResult a, GetCostToMoveIntoResult b)
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