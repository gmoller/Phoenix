using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Settlements : IEnumerable<Settlement>
    {
        private readonly World _world;

        private readonly List<Settlement> _settlements;

        public int FoodProducedThisTurn { get; private set; }

        public Settlement this[int index] => _settlements[index];

        internal Settlements(World world)
        {
            _world = world;
            _settlements = new List<Settlement>();
        }

        internal void Update(float deltaTime)
        {
            int foodProducedThisTurn = 0;
            foreach (var settlement in _settlements)
            {
                foodProducedThisTurn += settlement.FoodSurplus;
            }

            FoodProducedThisTurn = foodProducedThisTurn;
        }

        internal void AddSettlement(string name, string raceTypeName, Point hexLocation, CellGrid cellGrid)
        {
            var settlement = new Settlement(_world, name, raceTypeName, hexLocation, 4, cellGrid, "Builders Hall", "Barracks", "Smithy");
            _settlements.Add(settlement);
        }

        internal void EndTurn()
        {
            foreach (var settlement in _settlements)
            {
                settlement.EndTurn();
            }
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_settlements.Count}}}";

        public IEnumerator<Settlement> GetEnumerator()
        {
            foreach (var item in _settlements)
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