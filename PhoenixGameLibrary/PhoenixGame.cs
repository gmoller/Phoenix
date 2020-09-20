using Utilities;

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

            World.AddSettlement(new PointI(12, 9), "Barbarians");
            World.AddUnit(new PointI(12, 9), unitTypes["Test Dude"]);
            World.AddUnit(new PointI(15, 7), unitTypes["Barbarian Settlers"]);
            World.AddUnit(new PointI(12, 9), unitTypes["Barbarian Spearmen"]);
        }
    }
}