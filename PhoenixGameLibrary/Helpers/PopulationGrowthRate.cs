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
            // base
            if (numberOfCitizens >= maxSettlementSize) return 0;

            var baseGrowthRate = (maxSettlementSize - numberOfCitizens + 1) * Zen.Utilities.Constants.OneHalf;
            var baseGrowthRateFloored = (int)Math.Floor(baseGrowthRate);

            var adjustedGrowthRate = baseGrowthRateFloored * 10;

            // racial modifiers
            adjustedGrowthRate += raceType.GrowthRateModifier;

            // settlement buildings (granary +20, farmers market +30)
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var buildingPopulationGrowthTypes = gameMetadata.BuildingPopulationGrowthTypes;
            foreach (var item in buildingPopulationGrowthTypes)
            {
                if (buildingsBuilt.Contains(item.BuildingId))
                {
                    adjustedGrowthRate += item.PopulationGrowthRateIncrease;
                }
            }

            // TODO: spells (stream of life, dark rituals)

            // TODO: random events (population boom)

            // TODO: housing

            // TODO: starvation

            return adjustedGrowthRate;
        }
    }
}