using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        public World World { get; }

        public PhoenixGame()
        {
            World = new World();

            World.AddSettlement(new Point(12, 9), "Fairhaven", "Barbarians");
            World.AddUnit(new Point(12, 9), Globals.Instance.UnitTypes["Test Dude"]);
            World.AddUnit(new Point(12, 9), Globals.Instance.UnitTypes["Barbarian Settlers"]);
            World.AddUnit(new Point(12, 9), Globals.Instance.UnitTypes["Barbarian Spearmen"]);
        }

        public void Update(float deltaTime)
        {
            World.Update(deltaTime);
        }
    }
}