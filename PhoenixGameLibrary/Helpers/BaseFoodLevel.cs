using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary.Helpers
{
    public static class BaseFoodLevel
    {
        public static float DetermineBaseFoodLevel(Point location, List<Cell> catchmentCells) // used by Surveyor
        {
            // https://masterofmagic.fandom.com/wiki/Food#Base_Food_Level
            // TODO: Shared tiles halved, Corruption, Gaia's Blessing
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var terrainTypes = ((GameMetadata)context.GameMetadata).TerrainTypes;

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