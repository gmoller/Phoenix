using System.Collections.Generic;
using PhoenixGameData;
using Zen.Utilities;

namespace PhoenixGameLibrary.Helpers
{
    public static class BaseFoodLevel
    {
        public static float DetermineBaseFoodLevel(List<Cell> catchmentCells) // used by Surveyor
        {
            // https://masterofmagic.fandom.com/wiki/Food#Base_Food_Level
            // TODO: Shared tiles halved, Corruption, Gaia's Blessing
            var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");

            float foodOutput = 0.0f;
            foreach (var cell in catchmentCells)
            {
                var terrain = gameConfigCache.GetTerrainConfigById(cell.TerrainId);
                foodOutput += terrain.FoodOutput;
            }

            return foodOutput;
        }
    }
}