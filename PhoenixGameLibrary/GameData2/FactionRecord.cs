using Zen.Utilities;

namespace PhoenixGameLibrary.GameData2
{
    public class FactionRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public int RaceTypeId { get; } // Foreign key -> Config.RaceType
        public int GoldInTreasury { get; set; }
        public int GoldPerTurn { get; }
        public int ManaInTreasury { get; set; }
        public int ManaPerTurn { get; }

        public FactionRecord(int raceType)
        {
            Id = GameDataRepository.GetNextSequence("Faction");
            RaceTypeId = raceType;
            GoldInTreasury = 0;
            GoldPerTurn = 0;
            ManaInTreasury = 0;
            ManaPerTurn = 0;
        }
    }

    public class FactionsCollection : DataList<FactionRecord>
    {
    }
}