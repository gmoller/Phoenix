using System;
using System.Collections.Generic;
using PhoenixGameData;
using Zen.Utilities;

namespace PhoenixGameLibrary.Helpers
{
    public static class SettlementProduction
    {
        public static int DetermineProduction(Settlement settlement, List<Cell> catchmentCells)
        {
            // TODO: Trade Goods, Shared Terrain, Corruption, Gaia's Blessing, Inspirations, Cursed Lands, Sawmill, Forester's Guild, Miner's Guild, Mechanicians' Guild
            var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");

            var farmerProduction = settlement.Race.FarmerProductionRate * settlement.Citizens.Farmers;
            var workerProduction = settlement.Race.WorkerProductionRate * settlement.Citizens.Workers;

            var total = farmerProduction + workerProduction;

            var totalProduction = (int)Math.Ceiling(total);

            var productionBonus = 0.0f;
            foreach (var cell in catchmentCells)
            {
                var terrain = gameConfigCache.GetTerrainConfigById(cell.TerrainId);
                productionBonus += terrain.ProductionPercentage;
            }
            total = totalProduction * (1.0f + productionBonus * 0.01f);

            return (int)total;
        }
    }
}