using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Units : IEnumerable<Unit>
    {
        private readonly List<Unit> _units;

        public Unit this[int index] => _units[index];

        internal Units()
        {
            _units = new List<Unit>();
        }

        internal void AddUnit(UnitType unitType, Point point)
        {
            var unit = new Unit(unitType, point);
            _units.Add(unit);
        }

        internal void EndTurn()
        {
            foreach (var unit in _units)
            {
                unit.EndTurn();
            }
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_units.Count}}}";

        public IEnumerator<Unit> GetEnumerator()
        {
            foreach (var item in _units)
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