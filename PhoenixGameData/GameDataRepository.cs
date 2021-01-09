using System;
using System.Linq;
using PhoenixGameData.Tuples;
using Zen.Utilities;

namespace PhoenixGameData
{
    public class GameDataRepository
    {
        private static int _factionSequence;
        private static int _unitSequence;
        private static int _stackSequence;
        private static int _settlementSequence;
        private static int _settlementCitizenSequence;
        private static int _settlementBuildingSequence;
        private static int _settlementProducingSequence;

        private FactionsCollection Factions { get; }
        private UnitsCollection Units { get; }
        private StacksCollection Stacks { get; }
        private SettlementsCollection Settlements { get; }

        public GameDataRepository()
        {
            Factions = new FactionsCollection();
            Units = new UnitsCollection();
            Stacks = new StacksCollection();
            Settlements = new SettlementsCollection();
        }

        public void Add(FactionRecord faction)
        {
            Factions.Add(faction);
        }

        public void Add(UnitRecord unit)
        {
            // referential integrity:
            var stack = Stacks.FirstOrDefault(x => x.Id == unit.StackId);
            if (stack == null)
            {
                throw new Exception($"Inconsistent data: Stack [{unit.StackId}] not found in Stacks.");
            }

            Units.Add(unit);
        }

        public void Add(StackRecord stack)
        {
            // referential integrity:
            var faction = Factions.FirstOrDefault(x => x.Id == stack.FactionId);
            if (faction == null)
            {
                throw new Exception($"Inconsistent data: Faction [{stack.FactionId}] not found in Factions.");
            }

            Stacks.Add(stack);
        }

        public void Add(SettlementRecord settlement)
        {
            // referential integrity:
            var faction = Factions.FirstOrDefault(x => x.Id == settlement.FactionId);
            if (faction == null)
            {
                throw new Exception($"Inconsistent data: Faction [{settlement.FactionId}] not found in Factions.");
            }

            Settlements.Add(settlement);
        }

        public FactionRecord GetFactionById(int id)
        {
            var faction = Factions.GetById(id);

            return faction;
        }

        public UnitRecord GetUnitById(int id)
        {
            var unit = Units.GetById(id);

            return unit;
        }

        public StackRecord GetStackById(int id)
        {
            var stack = Stacks.GetById(id);

            return stack;
        }

        public SettlementRecord GetSettlementById(int id)
        {
            var settlement = Settlements.GetById(id);

            return settlement;
        }

        public DataList<StackRecord> GetStacksByFactionId(int factionId)
        {
            var stacks = Stacks.GetByFactionId(factionId);

            return stacks;
        }

        public DataList<StackRecord> GetStacksByLocationHex(PointI locationHex)
        {
            var stacks = Stacks.GetByLocationHex(locationHex);

            return stacks;
        }

        public DataList<StackRecord> GetStacksByOrdersNotBeenGivenThisTurnAndFactionId(int factionId)
        {
            var stacks = Stacks.GetByOrdersNotBeenGivenThisTurnAndFactionId(factionId);

            return stacks;
        }

        public DataList<UnitRecord> GetUnitsByStackId(int stackId)
        {
            var units = Units.GetByStackId(stackId);

            return units;
        }

        public DataList<UnitRecord> GetUnitsByFactionId(int factionId)
        {
            var stacks = Stacks.GetByFactionId(factionId);

            var units = DataList<UnitRecord>.Create();
            foreach (var stack in stacks)
            {
                var unitsForStack = GetUnitsByStackId(stack.Id);
                foreach (var unit in unitsForStack)
                {
                    units.Add(unit);
                }
            }

            return units;
        }

        public DataList<SettlementRecord> GetSettlementsByFactionId(int factionId)
        {
            var settlements = Settlements.GetByFactionId(factionId);

            return settlements;
        }

        public static int GetNextSequence(string sequenceName)
        {
            switch (sequenceName)
            {
                case "Faction":
                    _factionSequence++;
                    return _factionSequence;
                case "Unit":
                    _unitSequence++;
                    return _unitSequence;
                case "Stack":
                    _stackSequence++;
                    return _stackSequence;
                case "Settlement":
                    _settlementSequence++;
                    return _settlementSequence;
                case "SettlementCitizen":
                    _settlementCitizenSequence++;
                    return _settlementCitizenSequence;
                case "SettlementBuilding":
                    _settlementBuildingSequence++;
                    return _settlementBuildingSequence;
                case "SettlementProducing":
                    _settlementProducingSequence++;
                    return _settlementProducingSequence;
            }

            throw new Exception($"Unknown sequence requested: [{sequenceName}].");
        }
    }
}