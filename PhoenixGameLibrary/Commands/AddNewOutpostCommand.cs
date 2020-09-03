using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddNewOutpostCommand : Command
    {
        public override void Execute()
        {
            var payload = ((PointI position, string name, string raceTypeName, Settlements settlements, World world))Payload;

            var settlements = payload.settlements;

            var newSettlement = new Settlement(payload.world, payload.name, payload.raceTypeName, payload.position, 4, payload.world.OverlandMap.CellGrid, "Builders Hall", "Barracks", "Smithy");

            settlements.Add(newSettlement);
        }
    }
}