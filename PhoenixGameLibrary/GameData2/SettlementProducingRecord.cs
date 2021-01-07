namespace PhoenixGameLibrary.GameData2
{
    public struct SettlementProducingRecord
    {
        public int Id { get; } // Primary key
        public int SettlementId { get; } // Foreign key -> GameData.Settlement
        public int ProductionTypeId { get; } // Foreign key -> GameMetadata.ProductionType *undefined
        public int ProductionId { get; } // Foreign key -> GameMetadata.BuildingType/GameMetadata.UnitType/GameMetadata.OtherType *undefined
        public int ProductionAccrued { get; set; }

        public SettlementProducingRecord(int id, int settlementId, int productionTypeId, int productionId)
        {
            Id = id;
            SettlementId = settlementId;
            ProductionTypeId = productionTypeId;
            ProductionId = productionId;
            ProductionAccrued = 0;
        }
    }
}