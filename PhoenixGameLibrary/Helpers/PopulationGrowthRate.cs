using System;
using System.Collections.Generic;
using PhoenixGameData;
using PhoenixGameData.GameData;
using Zen.Utilities;

namespace PhoenixGameLibrary.Helpers
{
    public static class PopulationGrowthRate
    {
        public static int DetermineGrowthRate(int maxSettlementSize, int numberOfCitizens, RaceConfig race, List<int> buildingsBuilt)
        {
            // base
            if (numberOfCitizens >= maxSettlementSize) return 0;

            var baseGrowthRate = (maxSettlementSize - numberOfCitizens + 1) * Zen.Utilities.Constants.OneHalf;
            var baseGrowthRateFloored = (int)Math.Floor(baseGrowthRate);

            var adjustedGrowthRate = baseGrowthRateFloored * 10;

            // racial modifiers
            adjustedGrowthRate += (int)race.GrowthRateModifier;

            // settlement buildings (granary +20, farmers market +30)
            var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");
            foreach (var buildingBuilt in buildingsBuilt)
            {
                var building = gameConfigCache.GetBuildingConfigById(buildingBuilt);
                adjustedGrowthRate += (int)building.PopulationGrowthRateIncrease;
            }

            // TODO: spells (stream of life, dark rituals)

            // TODO: random events (population boom)

            // TODO: housing

            // TODO: starvation

            return adjustedGrowthRate;
        }
    }
}