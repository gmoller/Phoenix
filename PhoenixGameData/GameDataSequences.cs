using System;
using System.Collections.Generic;

namespace PhoenixGameData
{
    public static class GameDataSequences
    {
        private static readonly Dictionary<string, int> Sequences = new Dictionary<string, int>
        {
            {"Faction", 0},
            {"Unit", 0},
            {"Stack", 0},
            {"Settlement", 0},
            {"SettlementCitizen", 0},
            {"SettlementBuilding", 0},
            {"SettlementProducing", 0}
        };

        public static int GetNextSequence(string sequenceName)
        {
            if (!Sequences.ContainsKey(sequenceName))
            {
                throw new Exception($"Unknown sequence requested: [{sequenceName}].");
            }

            var currentSequence = Sequences[sequenceName];
            var nextSequence = currentSequence + 1;
            Sequences[sequenceName] = nextSequence;

            return nextSequence;
        }
    }
}