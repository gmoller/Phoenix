using System.Linq;
using Zen.Utilities;

namespace PhoenixGameLibrary.GameData2
{
    public class StackRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public int FactionId { get; } // Foreign key -> GameData.Faction
        public PointI LocationHex { get; set; }
        public UnitStatus Status { get; set; }
        public bool HaveOrdersBeenGivenThisTurn { get; set; }

        public StackRecord(int factionId, PointI locationHex)
        {
            Id = GameDataRepository.GetNextSequence("Stack");
            FactionId = factionId;
            LocationHex = locationHex;
            Status = UnitStatus.None;
            HaveOrdersBeenGivenThisTurn = false;
        }
    }

    public class StacksCollection : DataList<StackRecord>
    {
        public DataList<StackRecord> GetByFactionId(int factionId)
        {
            var list = Create();
            foreach (var item in Items.Where(item => item.FactionId == factionId))
            {
                list.Add(item);
            }

            return list;
        }

        public DataList<StackRecord> GetByOrdersNotBeenGivenThisTurn()
        {
            var list = Create();
            foreach (var item in Items.Where(item => item.HaveOrdersBeenGivenThisTurn == false))
            {
                list.Add(item);
            }

            return list;
        }

        public DataList<StackRecord> GetByLocationHex(PointI locationHex)
        {
            var list = Create();
            foreach (var item in Items.Where(item => item.LocationHex == locationHex))
            {
                list.Add(item);
            }

            return list;
        }
    }
}