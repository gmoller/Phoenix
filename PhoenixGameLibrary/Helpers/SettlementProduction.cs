using System;
using GameLogic;
using HexLibrary;

namespace PhoenixGameLibrary.Helpers
{
    public static class SettlementProduction
    {
        public static int DetermineProduction(Settlement settlement, CellGrid cellGrid)
        {
            // TODO: Trade Goods, Shared Terrain, Corruption, Gaia's Blessing, Inspirations, Cursed Lands, Sawmill, Forester's Guild, Miner's Guild, Mechanicians' Guild

            float farmerProduction = settlement.RaceType.FarmerProductionRate * settlement.Citizens.Farmers;
            float workerProduction = settlement.RaceType.WorkerProductionRate * settlement.Citizens.Workers;

            float total = farmerProduction + workerProduction;

            int totalProduction = (int)Math.Ceiling(total);

            var catchment = HexOffsetCoordinates.GetAllNeighbors(settlement.Location.X, settlement.Location.Y);
            float productionBonus = 0.0f;
            foreach (var tile in catchment)
            {
                var cell = cellGrid.GetCell(tile.Col, tile.Row);
                var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
                productionBonus += terrainType.ProductionPercentage;
            }
            total = totalProduction * (1.0f + productionBonus * 0.01f);

            return (int)total;
        }
    }
}