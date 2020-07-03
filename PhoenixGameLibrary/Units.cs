using System.Collections;
using System.Collections.Generic;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    public class Units : IEnumerable<Unit>
    {
        private readonly List<Unit> _units;

        public Unit this[int index]
        {
            get
            {
                return _units[index];
            }
        }

        public Units()
        {
            _units = new List<Unit>();
        }

        public void Update(float deltaTime)
        {
            foreach (var unit in _units)
            {
                unit.Update(deltaTime);
            }
        }

        public void AddUnit(UnitType unitType, Point point)
        {
            var unit = new Unit(unitType, point);
            _units.Add(unit);
        }

        public void EndTurn()
        {
            foreach (var unit in _units)
            {
                unit.EndTurn();
            }
        }

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