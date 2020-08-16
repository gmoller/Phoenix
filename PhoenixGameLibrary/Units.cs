using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Units : IEnumerable<Unit>
    {
        private readonly List<Unit> _units;

        public Unit this[int index] => _units[index];

        internal int Count => _units.Count;

        internal Units()
        {
            _units = new List<Unit>();
        }

        internal void Add(World world, UnitType unitType, Point point)
        {
            var unit = new Unit(world, unitType, point);
            _units.Add(unit);
        }

        internal void Remove(Unit unit)
        {
            _units.Remove(unit);
        }

        internal void DoPatrolAction()
        {
            foreach (var unit in _units)
            {
                unit.DoPatrolAction();
            }
        }

        internal void DoFortifyAction()
        {
            foreach (var unit in _units)
            {
                unit.DoFortifyAction();
            }
        }

        internal void DoExploreAction()
        {
            foreach (var unit in _units)
            {
                unit.DoExploreAction();
            }
        }

        internal void SetStatusToNone()
        {
            foreach (var unit in _units)
            {
                unit.SetStatusToNone();
            }
        }

        internal List<Unit> GetUnitsByAction(string actionType)
        {
            return _units.Where(unit => unit.Actions.Contains(actionType)).ToList();
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