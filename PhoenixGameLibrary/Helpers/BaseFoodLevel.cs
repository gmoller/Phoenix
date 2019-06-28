using Microsoft.Xna.Framework;
using GameLogic;
using HexLibrary;

namespace PhoenixGameLibrary.Helpers
{
    public static class BaseFoodLevel
    {
        public static float DetermineBaseFoodLevel(Point location, CellGrid cellGrid) // used by Surveyor
        {
            // Each city has a base food level of Food it can produce
            var catchment = HexOffsetCoordinates.GetAllNeighbors(location.X, location.Y);

            float foodOutput = 0.0f;
            foreach (var tile in catchment)
            {
                var cell = cellGrid.GetCell(tile.Col, tile.Row);
                var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
                foodOutput += terrainType.FoodOutput;
            }

            return foodOutput;
        }
    }
}