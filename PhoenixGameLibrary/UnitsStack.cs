using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class UnitsStack : IEnumerable<Unit>
    {
        private readonly Units _units;
        private readonly TerrainType _terrainType;

        public UnitsStack(Units units, TerrainType terrainType)
        {
            _units = units;
            _terrainType = terrainType;

            foreach (var unit in units)
            {
                unit.UnitsStack = this;
            }
        }

        public Unit this[int index] => _units[index];

        public Point Location => _units[0].Location;

        public float MovementPoints => DetermineMovementPoints();
        public List<string> MovementTypes => DetermineMovementTypes(_terrainType);

        public int Count => _units.Count;

        internal void EndTurn()
        {
            foreach (var unit in _units)
            {
                unit.EndTurn();
            }
        }

        private float DetermineMovementPoints()
        {
            var movementPoints = float.MaxValue;
            foreach (var unit in _units)
            {
                movementPoints = Math.Min(movementPoints, unit.MovementPoints);
            }

            return movementPoints;
        }

        internal bool CanSeeCell(Cell cell)
        {
            foreach (var unit in _units)
            {
                if (unit.CanSeeCell(cell))
                {
                    return true;
                }
            }

            return false;
        }

        private List<string> DetermineMovementTypes(TerrainType terrainType)
        {
            var movementTypes = new List<string> { "Walking" };

            if (IsSwimming(_units)) movementTypes.Add("Swimming");
            if (IsFlying(_units)) movementTypes.Add("Flying"); // if every unit has flying
            if (IsSailing(_units)) movementTypes.Add("Sailing");

            return movementTypes;
        }

        private bool IsSwimming(Units units)
        {
            // TODO: sailing, windwalking

            var swimming = false;
            foreach (var unit in units)
            {
                swimming = unit.UnitTypeMovementTypes.Contains("Swimming")  || unit.UnitTypeMovementTypes.Contains("Flying");
                if (!swimming) break;
            }

            return swimming;
        }

        private bool IsFlying(Units units)
        {
            // TODO: windwalking

            var flying = false;
            foreach (var unit in units)
            {
                flying = unit.UnitTypeMovementTypes.Contains("Flying");
                if (!flying) break;
            }

            return flying;
        }

        private bool IsSailing(Units units)
        {
            var sailing = false;
            foreach (var unit in units)
            {
                //sailing = unit.UnitTypeMoves.Contains()
            }

            return sailing;
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

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_units.Count}}}";
    }
}