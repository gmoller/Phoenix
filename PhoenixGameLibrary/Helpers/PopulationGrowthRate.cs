using System;
using System.Collections.Generic;
using GameLogic;
using PhoenixGameLibrary.GameData;

namespace PhoenixGameLibrary.Helpers
{
    public static class PopulationGrowthRate
    {
        public static int DetermineGrowthRate(int maxSettlementSize, int numberOfCitizens, RaceType raceType, List<int> buildingsBuilt)
        {
            // TODO: Housing Bonus, Dark Rituals, Stream of Life

            if (numberOfCitizens >= maxSettlementSize) return 0;

            float baseGrowthRate = (maxSettlementSize - numberOfCitizens + 1) / 2.0f;
            int baseGrowthRateRoundedUp = (int)Math.Ceiling(baseGrowthRate);

            int adjustedGrowthRate = baseGrowthRateRoundedUp * 10;

            // buildings
            foreach (var item in Globals.Instance.BuildingPopulationGrowthTypes)
            {
                if (buildingsBuilt.Contains(item.Id))
                {
                    adjustedGrowthRate += item.PopulationGrowthRateIncrease;
                }
            }

            adjustedGrowthRate += raceType.GrowthRateModifier;

            return adjustedGrowthRate;
        }
    }
}