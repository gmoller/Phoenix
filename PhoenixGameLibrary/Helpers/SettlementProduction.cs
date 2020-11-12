using System;
using System.Collections.Generic;
using Zen.Utilities;

namespace PhoenixGameLibrary.Helpers
{
    public static class SettlementProduction
    {
        public static int DetermineProduction(Settlement settlement, List<Cell> catchmentCells)
        {
            // TODO: Trade Goods, Shared Terrain, Corruption, Gaia's Blessing, Inspirations, Cursed Lands, Sawmill, Forester's Guild, Miner's Guild, Mechanicians' Guild
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var terrainTypes = gameMetadata.TerrainTypes;

            float farmerProduction = settlement.RaceType.FarmerProductionRate * settlement.Citizens.Farmers;
            float workerProduction = settlement.RaceType.WorkerProductionRate * settlement.Citizens.Workers;

            float total = farmerProduction + workerProduction;

            int totalProduction = (int)Math.Ceiling(total);

            float productionBonus = 0.0f;
            foreach (var cell in catchmentCells)
            {
                var terrainType = terrainTypes[cell.TerrainTypeId];
                productionBonus += terrainType.ProductionPercentage;
            }
            total = totalProduction * (1.0f + productionBonus * 0.01f);

            return (int)total;
        }
    }
}