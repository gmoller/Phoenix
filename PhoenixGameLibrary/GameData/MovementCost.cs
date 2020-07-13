using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct MovementCost
    {
        private readonly string _unitStackMovementType;

        public UnitStackMovementType UnitStackMovementType => Globals.Instance.UnitStackMovementTypes[_unitStackMovementType];
        public float Cost { get; set; }

        public MovementCost(string unitStackMovementType, float cost)
        {
            _unitStackMovementType = unitStackMovementType;
            Cost = cost;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{UnitStackMovementType={_unitStackMovementType},Cost={Cost}}}";
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class MovementCosts
    {
        private readonly List<MovementCost> _movementCosts;

        public MovementCosts(params MovementCost[] movementCosts)
        {
            _movementCosts = new List<MovementCost>();
            foreach (var item in movementCosts)
            {
                _movementCosts.Add(item);
            }
        }

        public MovementCost this[string unitStackMovementTypeName]
        {
            get
            {
                foreach (var item in _movementCosts)
                {
                    if (item.UnitStackMovementType.Name == unitStackMovementTypeName)
                    {
                        return item;
                    }
                }

                throw new Exception($"Item {unitStackMovementTypeName} not found in _movementCosts");
            }
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_movementCosts.Count}}}";
    }
}