using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class MoveUnitCommand : Command
    {
        internal override void Execute()
        {
            var payload = ((Unit unit, Point hexToMoveTo))Payload;
            var unit = payload.unit;
            var hexToMoveTo = payload.hexToMoveTo;
            unit.MoveTo(hexToMoveTo);
        }
    }
}