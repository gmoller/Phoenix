using PhoenixGameLibrary.GameData2;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        public World World { get; }

        public PhoenixGame()
        {
            World = new World(Constants.WORLD_MAP_COLUMNS, Constants.WORLD_MAP_ROWS);

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

            World.AddSettlement(new PointI(12, 9), "Barbarians");
            World.AddUnit(new PointI(12, 9), unitTypes["Test Dude"]);
            World.AddUnit(new PointI(15, 7), unitTypes["Barbarian Settlers"]);
            World.AddUnit(new PointI(12, 9), unitTypes["Barbarian Spearmen"]);
        }
    }
}