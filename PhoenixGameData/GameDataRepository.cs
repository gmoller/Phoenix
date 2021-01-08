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

        private FactionsCollection Factions { get; }
        private UnitsCollection Units { get; }
        private StacksCollection Stacks { get; }

        public GameDataRepository()
        {
            Factions = new FactionsCollection();
            Units = new UnitsCollection();
            Stacks = new StacksCollection();
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

        public DataList<StackRecord> GetStacksByOrdersNotBeenGivenThisTurn()
        {
            var stacks = Stacks.GetByOrdersNotBeenGivenThisTurn();

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
            }

            throw new Exception($"Unknown sequence requested: [{sequenceName}].");
        }
    }
}