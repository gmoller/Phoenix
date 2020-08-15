using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddUnitCommand : Command
    {
        public override void Execute()
        {
            var payload = ((Point position, UnitType unitType, Stacks stacks, World world))Payload;

            var position = payload.position;
            var unitType = payload.unitType;
            var stacks = payload.stacks;
            var world = payload.world;

            var units = new Units();
            units.AddUnit(world, unitType, position);
            var newStack = new Stack(world, units);

            stacks.Add(newStack);
        }
    }
}