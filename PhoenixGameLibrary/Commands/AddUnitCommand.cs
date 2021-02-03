using PhoenixGameData;
using PhoenixGameData.Tuples;
using Zen.Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddUnitCommand : Command
    {
        public override void Execute()
        {
            var payload = ((PointI position, int unitId, Stacks stacks))Payload;

            var stackRecord = new StackRecord(1, payload.position);
            var unitRecord = new UnitRecord(payload.unitId, stackRecord.Id);
            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            gameDataRepository.Add(stackRecord);
            gameDataRepository.Add(unitRecord);

            //var stacks = payload.stacks;
            //stacks.Add(stackRecord);
        }
    }
}