using PhoenixGameData.StrongTypes;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public readonly struct SettlementBuildingRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public SettlementId SettlementId { get; } // Foreign key -> GameData.Settlement
        public BuildingTypeId BuildingTypeId { get; } // Foreign key -> GameMetadata.BuildingType

        public SettlementBuildingRecord(int settlementId, int buildingTypeId)
        {
            Id = GameDataSequences.GetNextSequence("SettlementBuilding");
            SettlementId = new SettlementId(settlementId);
            BuildingTypeId = new BuildingTypeId(buildingTypeId);
        }

        public SettlementBuildingRecord(SettlementBuildingRecord settlementBuilding)
        {
            Id = settlementBuilding.Id;
            SettlementId = settlementBuilding.SettlementId;
            BuildingTypeId = settlementBuilding.BuildingTypeId;
        }
    }

    internal class SettlementBuildingsCollection : DataList<SettlementBuildingRecord>
    {
    }
}