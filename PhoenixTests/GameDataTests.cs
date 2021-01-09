using NUnit.Framework;
using PhoenixGameData;
using PhoenixGameData.Enumerations;
using PhoenixGameData.Tuples;
using Zen.Utilities;

namespace PhoenixTests
{
    public class GameDataTests
    {
        private GameDataRepository _repo;

        [SetUp]
        public void Setup()
        {
            if (_repo != null) return;

            _repo = new GameDataRepository();
            _repo.Add(new FactionRecord(0));
            _repo.Add(new StackRecord(1, PointI.Zero));
        }

        [Test]
        public void Faction_can_be_added()
        {
            var factionRecord = new FactionRecord(0)
            {
                GoldInTreasury = 10,
                ManaInTreasury = 20
            };
            _repo.Add(factionRecord);
            var id = factionRecord.Id;
            factionRecord = _repo.GetFactionById(id);

            Assert.AreEqual(id, factionRecord.Id);
            Assert.AreEqual(0, factionRecord.RaceTypeId);
            Assert.AreEqual(10, factionRecord.GoldInTreasury);
            Assert.AreEqual(20, factionRecord.ManaInTreasury);
        }

        [Test]
        public void Stack_can_be_added()
        {
            var stackRecord = new StackRecord(1, new PointI(12, 9))
            {
                Status = UnitStatus.Explore,
                HaveOrdersBeenGivenThisTurn = true
            };
            _repo.Add(stackRecord);
            var id = stackRecord.Id;
            stackRecord = _repo.GetStackById(id);
            var stacks = _repo.GetStacksByFactionId(1);

            Assert.AreEqual(id, stackRecord.Id);
            Assert.AreEqual(1, stackRecord.FactionId);
            Assert.AreEqual(UnitStatus.Explore, stackRecord.Status);
            Assert.AreEqual(true, stackRecord.HaveOrdersBeenGivenThisTurn);
            Assert.AreEqual(2, stacks.Count);
            Assert.AreEqual(1, stacks[0].Id);
            Assert.AreEqual(2, stacks[1].Id);

            stacks = _repo.GetStacksByLocationHex(new PointI(12, 9));
            Assert.AreEqual(1, stacks.Count);
            Assert.AreEqual(2, stacks[0].Id);

            stacks = _repo.GetStacksByOrdersNotBeenGivenThisTurnAndFactionId(1);
            Assert.AreEqual(1, stacks.Count);
            Assert.AreEqual(1, stacks[0].Id);
        }

        [Test]
        public void Unit_can_be_added()
        {
            var unitRecord = new UnitRecord(0, 1)
            {
                MovementPoints = 2.0f
            };
            _repo.Add(unitRecord);
            var id = unitRecord.Id;
            unitRecord = _repo.GetUnitById(id);
            var units = _repo.GetUnitsByStackId(1);

            Assert.AreEqual(id, unitRecord.Id);
            Assert.AreEqual(1, unitRecord.StackId);
            Assert.AreEqual(0, unitRecord.UnitTypeId);
            Assert.AreEqual(2.0f, unitRecord.MovementPoints);
            Assert.AreEqual(1, units.Count);
            Assert.AreEqual(1, units[0].Id);

            units = _repo.GetUnitsByFactionId(1);
            Assert.AreEqual(1, units.Count);
            Assert.AreEqual(1, units[0].Id);
        }

        [Test]
        public void Settlement_can_be_added()
        {
            var settlementRecord = new SettlementRecord(0, 1, new PointI(12, 9), "Gregville");
            _repo.Add(settlementRecord);
            var id = settlementRecord.Id;
            settlementRecord = _repo.GetSettlementById(id);
            var settlements = _repo.GetSettlementsByFactionId(1);

            Assert.AreEqual(id, settlementRecord.Id);
            Assert.AreEqual(1, settlementRecord.FactionId);
            Assert.AreEqual(0, settlementRecord.RaceTypeId);
            Assert.AreEqual(new PointI(12, 9), settlementRecord.LocationHex);
            Assert.AreEqual("Gregville", settlementRecord.Name);
            Assert.AreEqual(1, settlements.Count);
            Assert.AreEqual(1, settlements[0].Id);
        }
    }
}