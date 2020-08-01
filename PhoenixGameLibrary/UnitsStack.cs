using System;
using System.Runtime.Remoting.Messaging;

namespace PhoenixGameLibrary
{
    public class UnitsStack
    {
        private readonly Units _units;

        public UnitsStack(Units units)
        {
            _units = units;

            foreach (var unit in units)
            {
                unit.UnitsStack = this;
            }
        }

        public float GetMoves => DetermineMoves();
        public string MovementTypeName => DetermineMovementType();

        private float DetermineMoves()
        {
            var moves = float.MaxValue;
            foreach (var unit in _units)
            {
                moves = Math.Min(moves, unit.MovementPoints);
            }

            return moves;
        }

        private string DetermineMovementType()
        {
            if (IsSwimming(_units)) return "Swimming";
            if (IsFlying(_units)) return "Flying"; // if every unit has flying
            if (IsSailing(_units)) return "Sailing";

            // or the stack is walking
            return "Walking";
        }

        private bool IsSwimming(Units units)
        {
            // TODO: sailing, windwalking

            var swimming = false;
            foreach (var unit in units)
            {
                swimming = unit.UnitTypeMoves.Contains("Water")  || unit.UnitTypeMoves.Contains("Air");
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
                flying = unit.UnitTypeMoves.Contains("Air");
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
    }
}