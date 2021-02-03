using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameConfig
{
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
                    if (item.MovementType == movementTypeName)
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

        public override string ToString() => DebuggerDisplay;

        private string DebuggerDisplay => $"{{Count={_movementCosts.Count}}}";
    }
}