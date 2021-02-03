using System.Linq;
using PhoenixGameData.Enumerations;
using PhoenixGameData.StrongTypes;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public readonly struct StackRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public FactionId FactionId { get; } // Foreign key -> GameData.Faction
        public LocationHex LocationHex { get; }
        public Status Status { get; }
        public HaveOrdersBeenGivenThisTurn HaveOrdersBeenGivenThisTurn { get; }

        public StackRecord(int factionId, PointI locationHex)
        {
            Id = GameDataSequences.GetNextSequence("Stack");
            FactionId = new FactionId(factionId);
            LocationHex = new LocationHex(locationHex);
            Status = new Status(UnitStatus.None);
            HaveOrdersBeenGivenThisTurn = new HaveOrdersBeenGivenThisTurn(false);
        }

        public StackRecord(StackRecord stackRecord, LocationHex locationHex, Status unitStatus, HaveOrdersBeenGivenThisTurn haveOrdersBeenGivenThisTurn)
        {
            Id = stackRecord.Id;
            FactionId = stackRecord.FactionId;
            LocationHex = locationHex;
            Status = unitStatus;
            HaveOrdersBeenGivenThisTurn = haveOrdersBeenGivenThisTurn;
        }

        public StackRecord(StackRecord stackRecord, LocationHex locationHex)
        {
            Id = stackRecord.Id;
            FactionId = stackRecord.FactionId;
            LocationHex = locationHex;
            Status = stackRecord.Status;
            HaveOrdersBeenGivenThisTurn = stackRecord.HaveOrdersBeenGivenThisTurn;
        }

        public StackRecord(StackRecord stackRecord, Status unitStatus)
        {
            Id = stackRecord.Id;
            FactionId = stackRecord.FactionId;
            LocationHex = stackRecord.LocationHex;
            Status = unitStatus;
            HaveOrdersBeenGivenThisTurn = stackRecord.HaveOrdersBeenGivenThisTurn;
        }

        public StackRecord(StackRecord stackRecord, HaveOrdersBeenGivenThisTurn haveOrdersBeenGivenThisTurn)
        {
            Id = stackRecord.Id;
            FactionId = stackRecord.FactionId;
            LocationHex = stackRecord.LocationHex;
            Status = stackRecord.Status;
            HaveOrdersBeenGivenThisTurn = haveOrdersBeenGivenThisTurn;
        }
    }

    public class StacksCollection : DataList<StackRecord>
    {
        internal StacksCollection GetByFactionId(int factionId)
        {
            var list = new StacksCollection();
            foreach (var item in Items.Where(item => item.FactionId.Value == factionId))
            {
                list.Add(item);
            }

            return list;
        }

        internal StacksCollection GetByOrdersNotBeenGivenThisTurnAndFactionId(int factionId)
        {
            var list = new StacksCollection();
            foreach (var item in Items.Where(item => item.HaveOrdersBeenGivenThisTurn.Value == false && item.FactionId.Value == factionId))
            {
                list.Add(item);
            }

            return list;
        }

        internal StacksCollection GetByLocationHex(PointI locationHex)
        {
            var list = new StacksCollection();
            foreach (var item in Items.Where(item => item.LocationHex.Value == locationHex))
            {
                list.Add(item);
            }

            return list;
        }
    }
}