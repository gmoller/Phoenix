using Zen.Utilities;

namespace PhoenixGameLibrary.GameData2
{
    public readonly struct FactionRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public int RaceTypeId { get; } // Foreign key -> Config.RaceType
        public int GoldInTreasury { get; }
        public int GoldPerTurn { get; }
        public int ManaInTreasury { get; }
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