using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace PhoenixGameLibrary.Helpers
{
    public static class SettlementProduction
    {
        public static int DetermineProduction(Settlement settlement, List<Cell> catchmentCells)
        {
            // TODO: Trade Goods, Shared Terrain, Corruption, Gaia's Blessing, Inspirations, Cursed Lands, Sawmill, Forester's Guild, Miner's Guild, Mechanicians' Guild
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var terrainTypes = context.GameMetadata.TerrainTypes;

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