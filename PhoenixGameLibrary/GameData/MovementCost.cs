using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using Utilities.ExtensionMethods;

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
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var movementTypes = context.GameMetadata.MovementTypes;

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

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class MovementCosts : IEnumerable<MovementCost>
    {
        #region State
        private readonly List<MovementCost> _movementCosts;
        #endregion

        public MovementCosts(params MovementCost[] movementCosts)
        {
            _movementCosts = new List<MovementCost>();
            foreach (var item in movementCosts)
            {
                _movementCosts.Add(item);
            }
        }

        public MovementCost this[string movementTypeName]
        {
            get
            {
                foreach (var item in _movementCosts)
                {
                    if (item.MovementType.Name == movementTypeName)
                    {
                        return item;
                    }
                }

                throw new Exception($"Item {movementTypeName} not found in _movementCosts");
            }
        }

        public IEnumerator<MovementCost> GetEnumerator()
        {
            foreach (var item in _movementCosts)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_movementCosts.Count}}}";
    }
}