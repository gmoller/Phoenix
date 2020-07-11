using System;
using System.Collections.Generic;

namespace PhoenixGameLibrary.GameData
{
    public struct MovementCost
    {
        private readonly string _movementType;

        public MovementType MovementType => Globals.Instance.MovementTypes[_movementType];
        public float Moves { get; set; }

        public MovementCost(string movementType, float moves)
        {
            _movementType = movementType;
            Moves = moves;
        }
    }

    public class MovementCosts
    {
        private readonly List<MovementCost> _movementCost;

        public MovementCosts(params MovementCost[] movementCosts)
        {
            _movementCost = new List<MovementCost>();
            foreach (var item in movementCosts)
            {
                _movementCost.Add(item);
            }
        }

        public MovementCost this[string movementTypeName]
        {
            get
            {
                foreach (var item in _movementCost)
                {
                    if (item.MovementType.Name == movementTypeName)
                    {
                        return item;
                    }
                }

                throw new Exception($"Item {movementTypeName} not found in _movementCost");
            }
        }
    }
}