using System;
using System.Collections.Generic;
using System.Diagnostics;
using PhoenixGameLibrary.GameData;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class UnitsStack
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

        public float GetMoves => DetermineMoves();
        public List<string> MovementTypeName => DetermineMovementType(_terrainType);

        private float DetermineMoves()
        {
            var moves = float.MaxValue;
            foreach (var unit in _units)
            {
                moves = Math.Min(moves, unit.MovementPoints);
            }

            return moves;
        }

        private List<string> DetermineMovementType(TerrainType terrainType)
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

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_units.Count}}}";
    }
}