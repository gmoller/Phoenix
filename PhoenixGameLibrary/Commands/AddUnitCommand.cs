using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddUnitCommand : Command
    {
        public override void Execute()
        {
            var payload = ((Point position, UnitType unitType, UnitsStacks stacks))Payload;

            var position = payload.position;
            var unitType = payload.unitType;
            var stacks = payload.stacks;

            var units = new Units();
            units.AddUnit(unitType, position);
            var cell = Globals.Instance.World.OverlandMap.CellGrid.GetCell(position.X, position.Y);
            var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
            var newStack = new UnitsStack(units, terrainType);

            stacks.Add(newStack);
        }
    }
}