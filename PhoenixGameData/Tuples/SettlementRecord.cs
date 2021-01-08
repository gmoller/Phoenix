using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public struct SettlementRecord
    {
        public int Id { get; } // Primary key
        public int RaceTypeId { get; } // Foreign key -> GameMetadata.RaceType
        public int FactionId { get; set; } // Foreign key -> GameData.Faction
        public PointI LocationHex { get; set; }
        public string Name { get; set; }

        public SettlementRecord(int id, int raceTypeId, int factionId, PointI locationHex, string name)
        {
            Id = id;
            RaceTypeId = raceTypeId;
            FactionId = factionId;
            LocationHex = locationHex;
            Name = name;
        }
    }
}