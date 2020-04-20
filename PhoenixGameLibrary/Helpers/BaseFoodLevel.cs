using System.Collections.Generic;
using GameLogic;
using Utilities;

namespace PhoenixGameLibrary.Helpers
{
    public static class BaseFoodLevel
    {
        public static float DetermineBaseFoodLevel(Point location, List<Cell> catchmentCells) // used by Surveyor
        {
            // https://masterofmagic.fandom.com/wiki/Food#Base_Food_Level
            // TODO: Shared tiles halved, Corruption, Gaia's Blessing
            
            float foodOutput = 0.0f;
            foreach (var cell in catchmentCells)
            {
                var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
                foodOutput += terrainType.FoodOutput;
            }

            return foodOutput;
        }
    }
}