using System;
using NUnit.Framework;
using PhoenixGameConfig;
using PhoenixGameData;
using PhoenixGameData.Enumerations;
using PhoenixGameData.StrongTypes;
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

            CallContext<GameConfigRepository>.SetData("GameConfigRepository", new GameConfigRepository());

            _repo = new GameDataRepository();
            _repo.Add(new FactionRecord(0, 0, 0));
            _repo.Add(new StackRecord(1, PointI.Zero));
            _repo.Add(new StackRecord(1, PointI.Zero));
        }

        [Test]
        public void Faction_tests()
        {
            _repo.FactionUpdated += FactionUpdated;

            var factionRecord = new FactionRecord(0, 0, 0);
            _repo.Add(factionRecord);
            var id = factionRecord.Id;
            factionRecord = _repo.GetFactionById(id);

            Assert.AreEqual(id, factionRecord.Id);
            Assert.AreEqual(0, factionRecord.RaceTypeId.Value);
            Assert.AreEqual(0, factionRecord.GoldInTreasury.Value);
            Assert.AreEqual(0, factionRecord.ManaInTreasury.Value);

            var updatedFaction = new FactionRecord(factionRecord, new GoldInTreasury(10), new ManaInTreasury(20));
            _repo.Update(updatedFaction);
            factionRecord = _repo.GetFactionById(id);
            Assert.AreEqual(0, factionRecord.RaceTypeId.Value);
            Assert.AreEqual(10, factionRecord.GoldInTreasury.Value);
            Assert.AreEqual(20, factionRecord.ManaInTreasury.Value);

            updatedFaction = new FactionRecord(factionRecord, new GoldInTreasury(25));
            _repo.Update(updatedFaction);
            factionRecord = _repo.GetFactionById(id);
            Assert.AreEqual(0, factionRecord.RaceTypeId.Value);
            Assert.AreEqual(25, factionRecord.GoldInTreasury.Value);
            Assert.AreEqual(20, factionRecord.ManaInTreasury.Value);

            updatedFaction = new FactionRecord(factionRecord, new ManaInTreasury(50));
            _repo.Update(updatedFaction);
            factionRecord = _repo.GetFactionById(id);
            Assert.AreEqual(0, factionRecord.RaceTypeId.Value);
            Assert.AreEqual(25, factionRecord.GoldInTreasury.Value);
            Assert.AreEqual(50, factionRecord.ManaInTreasury.Value);
        }

        private void FactionUpdated(object sender, FactionRecord factionRecord)
        {
            Console.WriteLine("Faction updated!");
        }

        [Test]
        public void Stack_tests()
        {
            var stackRecord = new StackRecord(1, new PointI(12, 9));
            _repo.Add(stackRecord);
            var id = stackRecord.Id;
            stackRecord = _repo.GetStackById(id);
            var stacks = _repo.GetStacksByFactionId(1);

            Assert.AreEqual(id, stackRecord.Id);
            Assert.AreEqual(1, stackRecord.FactionId.Value);
            Assert.AreEqual(UnitStatus.None, stackRecord.Status.Value);
            Assert.AreEqual(false, stackRecord.HaveOrdersBeenGivenThisTurn.Value);
            Assert.AreEqual(3, stacks.Count);
            Assert.AreEqual(1, stacks[0].Id);
            Assert.AreEqual(2, stacks[1].Id);

            stacks = _repo.GetStacksByLocationHex(new PointI(12, 9));
            Assert.AreEqual(1, stacks.Count);
            Assert.AreEqual(3, stacks[0].Id);

            stacks = _repo.GetStacksByOrdersNotBeenGivenThisTurnAndFactionId(1);
            Assert.AreEqual(3, stacks.Count);
            Assert.AreEqual(1, stacks[0].Id);
            Assert.AreEqual(2, stacks[1].Id);
            Assert.AreEqual(3, stacks[2].Id);

            var updatedStack = new StackRecord(stackRecord, stackRecord.LocationHex, new Status(UnitStatus.Patrol), new HaveOrdersBeenGivenThisTurn(true));
            _repo.Update(updatedStack);
            stackRecord = _repo.GetStackById(id);
            Assert.AreEqual(1, stackRecord.FactionId.Value);
            Assert.AreEqual(new PointI(12, 9), stackRecord.LocationHex.Value);
            Assert.AreEqual(UnitStatus.Patrol, stackRecord.Status.Value);
            Assert.AreEqual(true, stackRecord.HaveOrdersBeenGivenThisTurn.Value);

            updatedStack = new StackRecord(stackRecord, new LocationHex(new PointI(10, 10)));
            _repo.Update(updatedStack);
            stackRecord = _repo.GetStackById(id);
            Assert.AreEqual(1, stackRecord.FactionId.Value);
            Assert.AreEqual(new PointI(10, 10), stackRecord.LocationHex.Value);
            Assert.AreEqual(UnitStatus.Patrol, stackRecord.Status.Value);
            Assert.AreEqual(true, stackRecord.HaveOrdersBeenGivenThisTurn.Value);

            updatedStack = new StackRecord(stackRecord, new Status(UnitStatus.Explore));
            _repo.Update(updatedStack);
            stackRecord = _repo.GetStackById(id);
            Assert.AreEqual(1, stackRecord.FactionId.Value);
            Assert.AreEqual(new PointI(10, 10), stackRecord.LocationHex.Value);
            Assert.AreEqual(UnitStatus.Explore, stackRecord.Status.Value);
            Assert.AreEqual(true, stackRecord.HaveOrdersBeenGivenThisTurn.Value);

            updatedStack = new StackRecord(stackRecord, new HaveOrdersBeenGivenThisTurn(false));
            _repo.Update(updatedStack);
            stackRecord = _repo.GetStackById(id);
            Assert.AreEqual(1, stackRecord.FactionId.Value);
            Assert.AreEqual(new PointI(10, 10), stackRecord.LocationHex.Value);
            Assert.AreEqual(UnitStatus.Explore, stackRecord.Status.Value);
            Assert.AreEqual(false, stackRecord.HaveOrdersBeenGivenThisTurn.Value);
        }

        [Test]
        public void Unit_tests()
        {
            var unitRecord = new UnitRecord(0, 1);
            _repo.Add(unitRecord);
            var id = unitRecord.Id;
            unitRecord = _repo.GetUnitById(id);
            var units = _repo.GetUnitsByStackId(1);

            Assert.AreEqual(id, unitRecord.Id);
            Assert.AreEqual(1, unitRecord.StackId.Value);
            Assert.AreEqual(0, unitRecord.UnitTypeId.Value);
            Assert.AreEqual(1.0f, unitRecord.MovementPoints.Value);
            Assert.AreEqual(1, units.Count);
            Assert.AreEqual(1, units[0].Id);

            units = _repo.GetUnitsByFactionId(1);
            Assert.AreEqual(1, units.Count);
            Assert.AreEqual(1, units[0].Id);

            var updatedUnit = new UnitRecord(unitRecord, new StackId(2), new MovementPoints(1.0f));
            _repo.Update(updatedUnit);
            unitRecord = _repo.GetUnitById(id);
            Assert.AreEqual(2, unitRecord.StackId.Value);
            Assert.AreEqual(0, unitRecord.UnitTypeId.Value);
            Assert.AreEqual(1.0f, unitRecord.MovementPoints.Value);

            updatedUnit = new UnitRecord(unitRecord, new StackId(1));
            _repo.Update(updatedUnit);
            unitRecord = _repo.GetUnitById(id);
            Assert.AreEqual(1, unitRecord.StackId.Value);
            Assert.AreEqual(0, unitRecord.UnitTypeId.Value);
            Assert.AreEqual(1.0f, unitRecord.MovementPoints.Value);

            updatedUnit = new UnitRecord(unitRecord, new MovementPoints(3.0f));
            _repo.Update(updatedUnit);
            unitRecord = _repo.GetUnitById(id);
            Assert.AreEqual(1, unitRecord.StackId.Value);
            Assert.AreEqual(0, unitRecord.UnitTypeId.Value);
            Assert.AreEqual(3.0f, unitRecord.MovementPoints.Value);
        }

        [Test]
        public void Settlement_tests()
        {
            var settlementRecord = new SettlementRecord(0, 1, new PointI(12, 9), "Gregville");
            _repo.Add(settlementRecord);
            var id = settlementRecord.Id;
            settlementRecord = _repo.GetSettlementById(id);
            var settlements = _repo.GetSettlementsByFactionId(1);

            Assert.AreEqual(id, settlementRecord.Id);
            Assert.AreEqual(1, settlementRecord.FactionId.Value);
            Assert.AreEqual(0, settlementRecord.RaceTypeId.Value);
            Assert.AreEqual(new PointI(12, 9), settlementRecord.LocationHex.Value);
            Assert.AreEqual("Gregville", settlementRecord.Name.Value);
            Assert.AreEqual(1, settlements.Count);
            Assert.AreEqual(1, settlements[0].Id);
        }
    }
}