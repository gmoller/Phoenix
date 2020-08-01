using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class MoveUnitCommand : Command
    {
        public override void Execute()
        {
            var payload = ((Unit unit, Point hexToMoveTo, string unitStackMovementType))Payload;
            var unit = payload.unit;
            var hexToMoveTo = payload.hexToMoveTo;
            var unitStackMovementType = payload.unitStackMovementType;

            unit.MoveTo(hexToMoveTo, unitStackMovementType);
        }
    }
}