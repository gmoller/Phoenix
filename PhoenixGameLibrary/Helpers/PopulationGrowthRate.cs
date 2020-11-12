using System;
using System.Collections.Generic;
using PhoenixGameLibrary.GameData;
using Zen.Utilities;

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
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var buildingPopulationGrowthTypes = gameMetadata.BuildingPopulationGrowthTypes;

            foreach (var item in buildingPopulationGrowthTypes)
            {
                if (buildingsBuilt.Contains(item.BuildingId))
                {
                    adjustedGrowthRate += item.PopulationGrowthRateIncrease;
                }
            }

            adjustedGrowthRate += raceType.GrowthRateModifier;

            return adjustedGrowthRate;
        }
    }
}