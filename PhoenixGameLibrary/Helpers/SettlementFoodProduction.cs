using System.Collections.Generic;
using GameLogic;

namespace PhoenixGameLibrary.Helpers
{
    public static class SettlementFoodProduction
    {
        public static int DetermineFoodProduction(Settlement settlement, List<int> buildingsBuilt)
        {
            // https://masterofmagic.fandom.com/wiki/Food
            // TODO: Animists' Guild, Famine, Wild Game

            float foodProduction = settlement.RaceType.FarmingRate * settlement.Citizens.Farmers;
            float excess = foodProduction - settlement.BaseFoodLevel;
            if (excess > 0.0f)
            {
                foodProduction = settlement.BaseFoodLevel + excess / 2;
            }

            // buildings
            foreach (var item in Globals.Instance.BuildingFoodOutputIncreaseTypes)
            {
                if (buildingsBuilt.Contains(item.BuildingId))
                {
                    foodProduction += item.FoodOutputIncrease;
                }
            }

            return (int)foodProduction;
        }
    }
}