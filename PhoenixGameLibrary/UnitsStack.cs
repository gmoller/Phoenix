namespace PhoenixGameLibrary
{
    public class UnitsStack
    {
        private readonly Units _units;

        internal UnitsStack(Units units)
        {
            _units = units;
        }

        internal string DetermineMovementType()
        {
            // flying
            bool flying = false;
            foreach (var unit in _units)
            {
                flying = unit.UnitTypeMoves.Contains("Air");
                if (!flying) break;
            }

            if (flying) return "Flying";

            return "Walking";
        }
    }
}