using PhoenixGameLibrary.GameData2;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    public static class PhoenixGame
    {
        public static World MakeWorld()
        {
            var world = new World(Constants.WORLD_MAP_COLUMNS, Constants.WORLD_MAP_ROWS);
            CallContext<World>.SetData("GameWorld", world);

            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var unitTypes = gameMetadata.UnitTypes;

            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            var faction = new FactionRecord(0); // barbarians
            gameDataRepository.Add(faction);
            
            var stack1 = new StackRecord(faction.Id, new PointI(12, 9));
            gameDataRepository.Add(stack1);
            var stack2 = new StackRecord(faction.Id, new PointI(15, 7));
            gameDataRepository.Add(stack2);
            var stack3 = new StackRecord(faction.Id, new PointI(12, 9));
            gameDataRepository.Add(stack3);

            var unit1 = new UnitRecord(100, stack1.Id); // test dude
            gameDataRepository.Add(unit1);
            var unit2 = new UnitRecord(100, stack1.Id); // barbarian settlers
            gameDataRepository.Add(unit2);
            var unit3 = new UnitRecord(100, stack1.Id); // barbarian spearmen
            gameDataRepository.Add(unit3);

            world.AddSettlement(new PointI(12, 9), "Barbarians");
            world.AddUnit(new PointI(12, 9), unitTypes["Test Dude"]);
            world.AddUnit(new PointI(15, 7), unitTypes["Barbarian Settlers"]);
            world.AddUnit(new PointI(12, 9), unitTypes["Barbarian Spearmen"]);

            return world;
        }
    }
}