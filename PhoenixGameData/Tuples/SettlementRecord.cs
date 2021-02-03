using System.Linq;
using PhoenixGameData.StrongTypes;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public readonly struct SettlementRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public RaceTypeId RaceTypeId { get; } // Foreign key -> GameMetadata.RaceType
        public FactionId FactionId { get; } // Foreign key -> GameData.Faction
        public LocationHex LocationHex { get; }
        public Name Name { get; }

        public SettlementRecord(int raceTypeId, int factionId, PointI locationHex, string name)
        {
            Id = GameDataSequences.GetNextSequence("Settlement");
            RaceTypeId = new RaceTypeId(raceTypeId);
            FactionId = new FactionId(factionId);
            LocationHex = new LocationHex(locationHex);
            Name = new Name(name);
        }

        public SettlementRecord(SettlementRecord settlementRecord, FactionId factionId, Name name)
        {
            Id = settlementRecord.Id;
            RaceTypeId = settlementRecord.RaceTypeId;
            FactionId = factionId;
            LocationHex = settlementRecord.LocationHex;
            Name = name;
        }

        public SettlementRecord(SettlementRecord settlementRecord, FactionId factionId)
        {
            Id = settlementRecord.Id;
            RaceTypeId = settlementRecord.RaceTypeId;
            FactionId = factionId;
            LocationHex = settlementRecord.LocationHex;
            Name = settlementRecord.Name;
        }

        public SettlementRecord(SettlementRecord settlementRecord, Name name)
        {
            Id = settlementRecord.Id;
            RaceTypeId = settlementRecord.RaceTypeId;
            FactionId = settlementRecord.FactionId;
            LocationHex = settlementRecord.LocationHex;
            Name = name;
        }
    }

    public class SettlementsCollection : DataList<SettlementRecord>
    {
        internal SettlementsCollection GetByFactionId(int factionId)
        {
            var list = new SettlementsCollection();
            foreach (var item in Items.Where(item => item.FactionId.Value == factionId))
            {
                list.Add(item);
            }

            return list;
        }
    }
}