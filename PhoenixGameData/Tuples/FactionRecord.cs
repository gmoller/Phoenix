using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public class FactionRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public int RaceTypeId { get; } // Foreign key -> Config.RaceType
        public int GoldInTreasury { get; set; }
        public int ManaInTreasury { get; set; }

        public FactionRecord(int raceTypeId)
        {
            Id = GameDataRepository.GetNextSequence("Faction");
            RaceTypeId = raceTypeId;
            GoldInTreasury = 0;
            ManaInTreasury = 0;
        }
    }

    internal class FactionsCollection : DataList<FactionRecord>
    {
    }
}