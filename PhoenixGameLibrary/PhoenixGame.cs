using PhoenixGameConfig;
using PhoenixGameData;
using PhoenixGameData.Tuples;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    public static class PhoenixGame
    {
        public static World MakeWorld()
        {
            var gameConfigRepository = CallContext<GameConfigRepository>.GetData("GameConfigRepository");
            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");

            var factionRecord = new FactionRecord(1, 0, 0); // barbarians
            gameDataRepository.Add(factionRecord);
            
            var stackRecord1 = new StackRecord(factionRecord.Id, new PointI(12, 9));
            gameDataRepository.Add(stackRecord1);
            var stackRecord2 = new StackRecord(factionRecord.Id, new PointI(15, 7));
            gameDataRepository.Add(stackRecord2);
            var stackRecord3 = new StackRecord(factionRecord.Id, new PointI(12, 9));
            gameDataRepository.Add(stackRecord3);

            var unitRecord1 = new UnitRecord(100, stackRecord1.Id); // test dude
            gameDataRepository.Add(unitRecord1);
            var unitRecord2 = new UnitRecord(1, stackRecord2.Id); // barbarian settlers
            gameDataRepository.Add(unitRecord2);
            var unitRecord3 = new UnitRecord(2, stackRecord3.Id); // barbarian spearmen
            gameDataRepository.Add(unitRecord3);

            var faction = new Faction(factionRecord.Id);
            var world = new World(Constants.WORLD_MAP_COLUMNS, Constants.WORLD_MAP_ROWS, faction);
            CallContext<World>.SetData("GameWorld", world);

            world.AddSettlement(new PointI(12, 9), 1);
            var stack1 = new Stack(stackRecord1.Id);
            world.AddUnit(stack1);
            var stack2 = new Stack(stackRecord2.Id);
            world.AddUnit(stack2);
            var stack3 = new Stack(stackRecord3.Id);
            world.AddUnit(stack3);

            return world;
        }
    }
}