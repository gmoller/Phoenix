using PhoenixGameData.StrongTypes;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public readonly struct SettlementProducingRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public SettlementId SettlementId { get; } // Foreign key -> GameData.Settlement
        public ProductionTypeId ProductionTypeId { get; } // Foreign key -> GameMetadata.ProductionType *undefined
        public ProductionId ProductionId { get; } // Foreign key -> GameMetadata.BuildingType/GameMetadata.UnitType/GameMetadata.OtherType *undefined
        public ProductionAccrued ProductionAccrued { get; }

        public SettlementProducingRecord(int settlementId, int productionTypeId, int productionId, int productionAccrued)
        {
            Id = GameDataSequences.GetNextSequence("SettlementProducing");
            SettlementId = new SettlementId(settlementId);
            ProductionTypeId = new ProductionTypeId(productionTypeId);
            ProductionId = new ProductionId(productionId);
            ProductionAccrued = new ProductionAccrued(productionAccrued);
        }

        public SettlementProducingRecord(SettlementProducingRecord settlementProducingRecord, ProductionAccrued productionAccrued)
        {
            Id = settlementProducingRecord.Id;
            SettlementId = settlementProducingRecord.SettlementId;
            ProductionTypeId = settlementProducingRecord.ProductionTypeId;
            ProductionId = settlementProducingRecord.ProductionId;
            ProductionAccrued = productionAccrued;
        }
    }

    internal class SettlementProducingCollection : DataList<SettlementProducingRecord>
    {
    }
}