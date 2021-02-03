using System;
using PhoenixGameData.Tuples;
using Zen.Utilities;

namespace PhoenixGameData
{
    public class GameDataRepository
    {
        private readonly GameConfigCache _gameConfigCache;

        private FactionsCollection Factions { get; }
        private UnitRecords Units { get; }
        private StacksCollection Stacks { get; }
        private SettlementsCollection Settlements { get; }

        public event EventHandler<FactionRecord> FactionUpdated;
        public event EventHandler<UnitRecord> UnitUpdated;
        public event EventHandler<StackRecord> StackUpdated;

        public GameDataRepository()
        {
            _gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");

            Factions = new FactionsCollection();
            Units = new UnitRecords();
            Stacks = new StacksCollection();
            Settlements = new SettlementsCollection();
        }

        public void Add(FactionRecord factionRecord)
        {
            // referential integrity:
            _gameConfigCache.GetRaceConfigById(factionRecord.RaceTypeId.Value);

            Factions.Add(factionRecord);
        }

        public void Update(FactionRecord factionRecord)
        {
            // referential integrity:
            _gameConfigCache.GetRaceConfigById(factionRecord.RaceTypeId.Value);

            Factions.Update(factionRecord);
            FactionUpdated?.Invoke(this, factionRecord);
        }

        public FactionRecord GetFactionById(int id)
        {
            var factionRecord = Factions.GetById(id);

            return factionRecord;
        }

        public void Add(UnitRecord unitRecord)
        {
            // referential integrity:
            //GetStackById(unitRecord.StackId.Value);
            _gameConfigCache.GetUnitConfigById(unitRecord.UnitTypeId.Value);

            Units.Add(unitRecord);
        }

        public void Update(UnitRecord unitRecord)
        {
            // referential integrity:
            //GetStackById(unitRecord.StackId.Value);
            _gameConfigCache.GetUnitConfigById(unitRecord.UnitTypeId.Value);

            Units.Update(unitRecord);
            UnitUpdated?.Invoke(this, unitRecord);
        }

        public UnitRecord GetUnitById(int id)
        {
            var unit = Units.GetById(id);

            return unit;
        }

        public UnitRecords GetUnitsByStackId(int stackId)
        {
            var unitRecords = Units.GetByStackId(stackId);

            return unitRecords;
        }

        public UnitRecords GetUnitsByFactionId(int factionId)
        {
            var stacks = Stacks.GetByFactionId(factionId);

            var unitRecords = new UnitRecords();
            foreach (var stack in stacks)
            {
                var unitsForStack = GetUnitsByStackId(stack.Id);
                foreach (var unit in unitsForStack)
                {
                    unitRecords.Add(unit);
                }
            }

            return unitRecords;
        }

        public void Add(StackRecord stackRecord)
        {
            // referential integrity:
            //GetFactionById(stackRecord.FactionId.Value);

            Stacks.Add(stackRecord);
        }

        public void Update(StackRecord stackRecord)
        {
            // referential integrity:
            //GetFactionById(stackRecord.FactionId.Value);

            Stacks.Update(stackRecord);
            StackUpdated?.Invoke(this, stackRecord);
        }

        public StackRecord GetStackById(int id)
        {
            var stack = Stacks.GetById(id);

            return stack;
        }

        public StacksCollection GetStacksByFactionId(int factionId)
        {
            var stacks = Stacks.GetByFactionId(factionId);

            return stacks;
        }

        public StacksCollection GetStacksByLocationHex(PointI locationHex)
        {
            var stacks = Stacks.GetByLocationHex(locationHex);

            return stacks;
        }

        public StacksCollection GetStacksByOrdersNotBeenGivenThisTurnAndFactionId(int factionId)
        {
            var stacks = Stacks.GetByOrdersNotBeenGivenThisTurnAndFactionId(factionId);

            return stacks;
        }

        public void Add(SettlementRecord settlement)
        {
            // referential integrity:
            //GetFactionById(settlement.FactionId.Value);
            _gameConfigCache.GetRaceConfigById(settlement.RaceTypeId.Value);

            Settlements.Add(settlement);
        }

        public void Update(SettlementRecord settlement)
        {
            // referential integrity:
            //GetFactionById(settlement.FactionId.Value);
            _gameConfigCache.GetRaceConfigById(settlement.RaceTypeId.Value);

            Settlements.Update(settlement);
        }

        public SettlementRecord GetSettlementById(int id)
        {
            var settlement = Settlements.GetById(id);

            return settlement;
        }

        public SettlementsCollection GetSettlementsByFactionId(int factionId)
        {
            var settlements = Settlements.GetByFactionId(factionId);

            return settlements;
        }
    }
}