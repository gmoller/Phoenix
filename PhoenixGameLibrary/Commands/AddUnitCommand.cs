using GameLogic;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddUnitCommand : Command
    {
        internal override void Execute()
        {
            var payload = ((Point position, UnitType unitType))Payload;
            var unitType = payload.unitType;
            var position = payload.position;
            Globals.Instance.World.Units.AddUnit(unitType, position);
        }
    }
}
