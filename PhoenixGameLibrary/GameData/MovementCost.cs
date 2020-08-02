using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct MovementCost
    {
        private readonly string _movementType;

        public MovementType MovementType => Globals.Instance.MovementTypes[_movementType];
        public float Cost { get; set; }

        public MovementCost(string movementType, float cost)
        {
            _movementType = movementType;
            Cost = cost;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{MovementType={_movementType},Cost={Cost}}}";
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class MovementCosts : IEnumerable<MovementCost>
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

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_movementCosts.Count}}}";

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
    }
}