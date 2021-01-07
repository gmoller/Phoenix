using System;
using PhoenixGameLibrary.GameData2;
using Zen.Utilities;

namespace PhoenixGameLibrary
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
            Units.Add(unit);
        }

        public void Add(StackRecord stack)
        {
            Stacks.Add(stack);
        }

        public FactionRecord GetFactionById(int id)
        {
            var faction = Factions.GetById(id);

            return faction;
        }

        public DataList<StackRecord> GetStacksByFactionId(int factionId)
        {
            var stacks = Stacks.GetByFactionId(factionId);

            return stacks;
        }

        public DataList<UnitRecord> GetUnitsByStackId(int stackId)
        {
            var units = Units.GetByStackId(stackId);

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