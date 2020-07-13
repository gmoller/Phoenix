namespace PhoenixGameLibrary
{
    public class UnitsStack
    {
        private readonly Units _units;

        public UnitsStack(Units units)
        {
            _units = units;
        }

        public string DetermineMovementType()
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