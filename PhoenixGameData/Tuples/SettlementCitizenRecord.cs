namespace PhoenixGameData.Tuples
{
    public struct SettlementCitizenRecord
    {
        public int Id { get; } // Primary key
        public int SettlementId { get; } // Foreign key -> GameData.Settlement
        public int CitizenTypeId { get; } // Foreign key -> GameMetadata.CitizenType *undefined
        public int Amount { get; set; }

        public SettlementCitizenRecord(int id, int settlementId, int citizenTypeId)
        {
            Id = id;
            SettlementId = settlementId;
            CitizenTypeId = citizenTypeId;
            Amount = 0;
        }
    }
}