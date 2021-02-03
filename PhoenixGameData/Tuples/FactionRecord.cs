using PhoenixGameData.StrongTypes;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public readonly struct FactionRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public RaceTypeId RaceTypeId { get; } // Foreign key -> Config.RaceType
        public GoldInTreasury GoldInTreasury { get; }
        public ManaInTreasury ManaInTreasury { get; }

        public FactionRecord(int raceTypeId, int goldInTreasury, int manaInTreasury) // for creation
        {
            Id = GameDataSequences.GetNextSequence("Faction");
            RaceTypeId = new RaceTypeId(raceTypeId);
            GoldInTreasury = new GoldInTreasury(goldInTreasury);
            ManaInTreasury = new ManaInTreasury(manaInTreasury);
        }

        public FactionRecord(FactionRecord faction, GoldInTreasury goldInTreasury, ManaInTreasury manaInTreasury) // for update
        {
            Id = faction.Id;
            RaceTypeId = faction.RaceTypeId;
            GoldInTreasury = goldInTreasury;
            ManaInTreasury = manaInTreasury;
        }

        public FactionRecord(FactionRecord faction, GoldInTreasury goldInTreasury) // for update
        {
            Id = faction.Id;
            RaceTypeId = faction.RaceTypeId;
            GoldInTreasury = goldInTreasury;
            ManaInTreasury = faction.ManaInTreasury;
        }

        public FactionRecord(FactionRecord faction, ManaInTreasury manaInTreasury) // for update
        {
            Id = faction.Id;
            RaceTypeId = faction.RaceTypeId;
            GoldInTreasury = faction.GoldInTreasury;
            ManaInTreasury = manaInTreasury;
        }
    }

    internal class FactionsCollection : DataList<FactionRecord>
    {
    }
}