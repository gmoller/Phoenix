using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddUnitCommand : Command
    {
        public override void Execute()
        {
            var payload = ((Point position, UnitType unitType, Units units))Payload;
            var unitType = payload.unitType;
            var position = payload.position;
            var units = payload.units;
            units.AddUnit(unitType, position);
        }
    }
}
