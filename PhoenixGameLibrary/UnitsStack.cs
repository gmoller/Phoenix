using System;

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

            return moves >= 0.0f ? moves : 0;
        }

        private string DetermineMovementType()
        {
            // either they're all flying
            var flying = false;
            foreach (var unit in _units)
            {
                flying = unit.UnitTypeMoves.Contains("Air");
                if (!flying) break;
            }

            if (flying) return "Flying";

            // or the stack is walking
            return "Walking";
        }
    }
}