using System.Collections.Generic;
using Utilities;

namespace PhoenixGameLibrary.Helpers
{
    internal static class SettlementFoodProduction
    {
        internal static FoodBreakdown DetermineFoodProduction(Settlement settlement, List<int> buildingsBuilt)
        {
            // https://masterofmagic.fandom.com/wiki/Food
            // TODO: Animists' Guild, Famine, Wild Game

            var foodBreakdown = new FoodBreakdown();

            var foodFromFarmers = settlement.RaceType.FarmingRate * settlement.Citizens.Farmers;
            var excess = foodFromFarmers - settlement.BaseFoodLevel;
            if (excess > 0.0f)
            {
                foodFromFarmers = settlement.BaseFoodLevel + excess / 2;
            }
            foodBreakdown.Add("Farmers", foodFromFarmers);

            // buildings
            var context = CallContext<GlobalContext>.GetData("AmbientGlobalContext");
            var buildingFoodOutputIncreaseTypes = context.GameMetadata.BuildingFoodOutputIncreaseTypes;
            var buildingTypes = context.GameMetadata.BuildingTypes;
            foreach (var item in buildingFoodOutputIncreaseTypes)
            {
                if (buildingsBuilt.Contains(item.Id))
                {
                    var buildingName = buildingTypes[item.Id].Name;
                    foodBreakdown.Add(buildingName, item.FoodOutputIncrease);
                }
            }

            return foodBreakdown;
        }
    }

    public class FoodBreakdown
    {
        private readonly List<FoodBreakdownItem> _foodBreakdown;

        internal FoodBreakdown()
        {
            _foodBreakdown = new List<FoodBreakdownItem>();
        }

        internal int TotalFood
        {
            get
            {
                float sum = 0;
                foreach (var item in _foodBreakdown)
                {
                    sum += item.Food;
                }

                return (int)sum;
            }
        }

        internal void Add(string foodSource, float food)
        {
            _foodBreakdown.Add(new FoodBreakdownItem(foodSource, food));
        }
    }

    internal struct FoodBreakdownItem
    {
        private string Name { get; }
        internal float Food { get; }

        internal FoodBreakdownItem(string name, float food)
        {
            Name = name;
            Food = food;
        }
    }
}