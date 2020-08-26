using Utilities;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        public World World { get; }

        public PhoenixGame()
        {
            World = new World(Constants.WORLD_MAP_COLUMNS, Constants.WORLD_MAP_ROWS);

            var context = CallContext<GlobalContext>.GetData("AmbientGlobalContext");
            var unitTypes = context.GameMetadata.UnitTypes;

            World.AddSettlement(new Point(12, 9), "Barbarians");
            World.AddUnit(new Point(12, 9), unitTypes["Test Dude"]);
            World.AddUnit(new Point(15, 7), unitTypes["Barbarian Settlers"]);
            World.AddUnit(new Point(12, 9), unitTypes["Barbarian Spearmen"]);
        }

        public void Update(float deltaTime)
        {
            World.Update(deltaTime);
        }
    }
}