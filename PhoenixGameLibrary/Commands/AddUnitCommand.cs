using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddUnitCommand : Command
    {
        public override void Execute()
        {
            var payload = ((PointI position, UnitType unitType, Stacks stacks, World world))Payload;

            var stacks = payload.stacks;

            var unit = new Unit(payload.world, payload.unitType, payload.position);
            var units = new Units { unit };
            var newStack = new Stack(payload.world, units);

            stacks.Add(newStack);
        }
    }
}