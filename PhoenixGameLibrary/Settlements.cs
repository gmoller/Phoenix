using System.Collections;
using System.Collections.Generic;
using Utilities;

namespace PhoenixGameLibrary
{
    public class Settlements : IEnumerable<Settlement>
    {
        private readonly List<Settlement> _settlements;

        public int FoodProducedThisTurn { get; private set; }

        public Settlement this[int index]
        {
            get
            {
                return _settlements[index];
            }
        }

        public Settlements()
        {
            _settlements = new List<Settlement>();
        }

        public void Update(float deltaTime)
        {
            int foodProducedThisTurn = 0;
            foreach (var settlement in _settlements)
            {
                settlement.Update(deltaTime);
                foodProducedThisTurn += settlement.FoodSurplus;
            }

            FoodProducedThisTurn = foodProducedThisTurn;
        }

        internal bool HasSettlementOnCell(int column, int row)
        {
            foreach (var item in _settlements)
            {
                if (item.Location.X == column && item.Location.Y == row)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddSettlement(string name, string raceTypeName, Point hexLocation, CellGrid cellGrid)
        {
            var settlement = new Settlement(name, raceTypeName, hexLocation, 4, cellGrid, "Builders Hall", "Barracks", "Smithy");
            _settlements.Add(settlement);
        }

        public void EndTurn()
        {
            foreach (var settlement in _settlements)
            {
                settlement.EndTurn();
            }
        }

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