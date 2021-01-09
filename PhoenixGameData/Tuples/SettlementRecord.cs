using System.Linq;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public class SettlementRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public int RaceTypeId { get; } // Foreign key -> GameMetadata.RaceType
        public int FactionId { get; set; } // Foreign key -> GameData.Faction
        public PointI LocationHex { get; }
        public string Name { get; set; }

        public SettlementRecord(int raceTypeId, int factionId, PointI locationHex, string name)
        {
            Id = GameDataRepository.GetNextSequence("Settlement");
            RaceTypeId = raceTypeId;
            FactionId = factionId;
            LocationHex = locationHex;
            Name = name;
        }
    }

    internal class SettlementsCollection : DataList<SettlementRecord>
    {
        internal DataList<SettlementRecord> GetByFactionId(int factionId)
        {
            var list = Create();
            foreach (var item in Items.Where(item => item.FactionId == factionId))
            {
                list.Add(item);
            }

            return list;
        }
    }
}