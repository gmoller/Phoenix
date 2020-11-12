using System.Diagnostics;
using Zen.Utilities;
using Zen.Utilities.ExtensionMethods;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct MovementCost
    {
        #region State
        private readonly string _movementType;

        public float Cost { get; }
        #endregion

        public MovementType MovementType => GetMovementType();

        public MovementCost(string movementType, float cost)
        {
            _movementType = movementType;
            Cost = cost;
        }

        private MovementType GetMovementType()
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var movementTypes = gameMetadata.MovementTypes;

            return movementTypes[_movementType];
        }

        #region Overrides and Overloads

        public override bool Equals(object obj)
        {
            return obj is MovementCost point && this == point;
        }

        public static bool operator == (MovementCost a, MovementCost b)
        {
            return a._movementType == b._movementType && a.Cost.AboutEquals(b.Cost);
        }

        public static bool operator != (MovementCost a, MovementCost b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return _movementType.GetHashCode() ^ Cost.GetHashCode();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{MovementType={_movementType},Cost={Cost}}}";

        #endregion
    }
}