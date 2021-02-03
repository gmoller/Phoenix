using PhoenixGameData.StrongTypes;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public readonly struct SettlementCitizenRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public SettlementId SettlementId { get; } // Foreign key -> GameData.Settlement
        public CitizenTypeId CitizenTypeId { get; } // Foreign key -> GameMetadata.CitizenType *undefined
        public Amount Amount { get; }

        public SettlementCitizenRecord(int settlementId, int citizenTypeId, int amount)
        {
            Id = GameDataSequences.GetNextSequence("SettlementCitizen");
            SettlementId = new SettlementId(settlementId);
            CitizenTypeId = new CitizenTypeId(citizenTypeId);
            Amount = new Amount(amount);
        }

        public SettlementCitizenRecord(SettlementCitizenRecord settlementCitizenRecord, Amount amount)
        {
            Id = settlementCitizenRecord.Id;
            SettlementId = settlementCitizenRecord.SettlementId;
            CitizenTypeId = settlementCitizenRecord.CitizenTypeId;
            Amount = amount;
        }
    }

    internal class SettlementCitizensCollection : DataList<SettlementCitizenRecord>
    {
    }
}