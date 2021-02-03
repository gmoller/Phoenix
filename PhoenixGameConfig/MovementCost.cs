using System.Diagnostics;
using Zen.Utilities.ExtensionMethods;

namespace PhoenixGameConfig
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct MovementCost
    {
        public string MovementType { get; }
        public float Cost { get; }

        public MovementCost(string movement, float cost)
        {
            MovementType = movement;
            Cost = cost;
        }

        #region Overrides and Overloads

        public override bool Equals(object obj)
        {
            return obj is MovementCost point && this == point;
        }

        public static bool operator == (MovementCost a, MovementCost b)
        {
            return a.MovementType == b.MovementType && a.Cost.AboutEquals(b.Cost);
        }

        public static bool operator != (MovementCost a, MovementCost b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return MovementType.GetHashCode() ^ Cost.GetHashCode();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{MovementType={MovementType},Cost={Cost}}}";

        #endregion
    }
}