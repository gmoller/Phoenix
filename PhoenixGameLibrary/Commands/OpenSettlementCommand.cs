using GameLogic;
using HexLibrary;

namespace PhoenixGameLibrary.Commands
{
    public class OpenSettlementCommand : Command
    {
        internal override void Execute()
        {
            Globals.Instance.World.IsInSettlementView = true;

            var settlement = Globals.Instance.World.Settlements[0];
            Globals.Instance.World.Settlement = settlement; // TODO: get by settlementId
            var worldPixelLocation = HexOffsetCoordinates.OffsetCoordinatesToPixel(settlement.Location.X, settlement.Location.Y);
            Globals.Instance.World.Camera.LookAt(worldPixelLocation);
        }
    }
}