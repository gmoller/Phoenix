using Zen.Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddNewOutpostCommand : Command
    {
        public override void Execute()
        {
            var payload =
                ((PointI position, string name, int raceId, Settlements settlements, World world)) Payload;

            var settlements = payload.settlements;

            var newSettlement = new Settlement(payload.name, payload.raceId, payload.position, 4, payload.world.OverlandMap.CellGrid, new [] {1, 6, 30} ); // "Builders Hall", "Barracks", "Smithy"

            settlements.Add(newSettlement);
        }
    }
}