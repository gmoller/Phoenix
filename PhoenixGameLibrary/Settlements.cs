using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Settlements : IEnumerable<Settlement>
    {
        #region State
        private readonly List<Settlement> _settlements;

        public Settlement Selected { get; set; }
        #endregion

        public int FoodProducedThisTurn => DetermineFoodProduced();
        public Settlement this[int index] => _settlements[index];
        public int Count => _settlements.Count;

        internal Settlements()
        {
            _settlements = new List<Settlement>();
        }

        private int DetermineFoodProduced()
        {
            var foodProducedThisTurn = 0;
            foreach (var settlement in _settlements)
            {
                foodProducedThisTurn += settlement.FoodSurplus;
            }

            return foodProducedThisTurn;
        }

        internal void Add(Settlement settlement)
        {
            _settlements.Add(settlement);
        }

        internal void BeginTurn()
        {
            foreach (var settlement in _settlements)
            {
                settlement.BeginTurn();
            }
        }

        internal void EndTurn()
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

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_settlements.Count}}}";
    }
}