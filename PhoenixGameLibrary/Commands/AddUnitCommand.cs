using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddUnitCommand : Command
    {
        public override void Execute()
        {
            var payload = ((Point position, UnitType unitType, UnitsStacks stacks))Payload;

            var position = payload.position;
            var unitType = payload.unitType;
            var stacks = payload.stacks;

            var units = new Units();
            units.AddUnit(Globals.Instance.World, unitType, position);
            var newStack = new UnitsStack(units);

            stacks.Add(newStack);
        }
    }
}