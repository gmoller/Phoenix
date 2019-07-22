﻿using System.Collections.Generic;
using System.Linq;
using GameLogic;

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
            foreach (var item in Globals.Instance.BuildingFoodOutputIncreaseTypes)
            {
                if (buildingsBuilt.Contains(item.Id))
                {
                    var buildingName = Globals.Instance.BuildingTypes[item.Id].Name;
                    foodBreakdown.Add(buildingName, item.FoodOutputIncrease);
                }
            }

            return foodBreakdown;
        }
    }

    public class FoodBreakdown
    {
        private List<FoodBreakdownItem> _foodBreakdown;

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
        internal string Name { get; }
        internal float Food { get; }

        internal FoodBreakdownItem(string name, float food)
        {
            Name = name;
            Food = food;
        }
    }
}