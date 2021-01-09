using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public class SettlementBuildingRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public int SettlementId { get; } // Foreign key -> GameData.Settlement
        public int BuildingTypeId { get; } // Foreign key -> GameMetadata.BuildingType

        public SettlementBuildingRecord(int settlementId, int buildingTypeId)
        {
            Id = GameDataRepository.GetNextSequence("SettlementBuilding");
            SettlementId = settlementId;
            BuildingTypeId = buildingTypeId;
        }
    }

    internal class SettlementBuildingsCollection : DataList<SettlementBuildingRecord>
    {
    }
}