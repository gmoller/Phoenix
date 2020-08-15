using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using PhoenixGameLibrary.GameData;
using Utilities;

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
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var buildingPopulationGrowthTypes = ((GameMetadata)context.GameMetadata).BuildingPopulationGrowthTypes;

            foreach (var item in buildingPopulationGrowthTypes)
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