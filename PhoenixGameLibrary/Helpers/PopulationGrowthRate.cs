using System;
using PhoenixGameLibrary.GameData;

namespace PhoenixGameLibrary.Helpers
{
    public static class PopulationGrowthRate
    {
        public static int DetermineGrowthRate(int maxSettlementSize, int numberOfCitizens, RaceType raceType)
        {
            // TODO: Granary, Farmer's Market, Housing Bonus, Dark Rituals, Stream of Life

            if (numberOfCitizens >= maxSettlementSize) return 0;

            float baseGrowthRate = (maxSettlementSize - numberOfCitizens + 1) / 2.0f;
            int baseGrowthRateRoundedUp = (int)Math.Ceiling(baseGrowthRate);

            int adjustedGrowthRate = baseGrowthRateRoundedUp * 10;
            adjustedGrowthRate += raceType.GrowthRateModifier;

            return adjustedGrowthRate;
        }
    }
}