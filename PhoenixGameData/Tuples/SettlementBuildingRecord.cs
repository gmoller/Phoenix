namespace PhoenixGameData.Tuples
{
    public struct SettlementBuildingRecord
    {
        public int Id { get; } // Primary key
        public int SettlementId { get; } // Foreign key -> GameData.Settlement
        public int BuildingTypeId { get; } // Foreign key -> GameMetadata.BuildingType

        public SettlementBuildingRecord(int id, int settlementId, int buildingTypeId)
        {
            Id = id;
            SettlementId = settlementId;
            BuildingTypeId = buildingTypeId;
        }
    }
}