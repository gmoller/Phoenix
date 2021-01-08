using PhoenixGameLibrary.GameData;
using Zen.Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddUnitCommand : Command
    {
        public override void Execute()
        {
            var payload = ((PointI position, UnitType unitType, Stacks stacks))Payload;

            var stacks = payload.stacks;

            var newStack = new Stack(1, payload.position);
            var unit = new Unit(payload.unitType, newStack.Id);

            stacks.Add(newStack);
        }
    }
}