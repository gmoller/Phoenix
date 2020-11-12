using System.Collections.Generic;
using Zen.Utilities;

namespace PhoenixGameLibrary.Helpers
{
    public static class BaseFoodLevel
    {
        public static float DetermineBaseFoodLevel(PointI location, List<Cell> catchmentCells) // used by Surveyor
        {
            // https://masterofmagic.fandom.com/wiki/Food#Base_Food_Level
            // TODO: Shared tiles halved, Corruption, Gaia's Blessing
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var terrainTypes = gameMetadata.TerrainTypes;

            float foodOutput = 0.0f;
            foreach (var cell in catchmentCells)
            {
                var terrainType = terrainTypes[cell.TerrainTypeId];
                foodOutput += terrainType.FoodOutput;
            }

            return foodOutput;
        }
    }
}