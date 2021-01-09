using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public class SettlementCitizenRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public int SettlementId { get; } // Foreign key -> GameData.Settlement
        public int CitizenTypeId { get; } // Foreign key -> GameMetadata.CitizenType *undefined
        public int Amount { get; set; }

        public SettlementCitizenRecord(int settlementId, int citizenTypeId)
        {
            Id = GameDataRepository.GetNextSequence("SettlementCitizen");
            SettlementId = settlementId;
            CitizenTypeId = citizenTypeId;
            Amount = 0;
        }
    }

    internal class SettlementCitizensCollection : DataList<SettlementCitizenRecord>
    {
    }
}