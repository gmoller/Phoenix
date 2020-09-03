using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class MoveUnitCommand : Command
    {
        public override void Execute()
        {
            var payload = ((Stack stack, PointI hexToMoveTo))Payload;
            var stack = payload.stack;
            var hexToMoveTo = payload.hexToMoveTo;

            stack.MoveTo(hexToMoveTo);
        }
    }
}