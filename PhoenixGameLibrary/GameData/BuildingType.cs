using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using GameLogic;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct BuildingType
    {
        public int Id { get; }
        public string Name { get; }
        public Point Slot { get; }
        public float ConstructionCost { get; }
        public float UpkeepGold { get; }
        public float UpkeepMana { get; }
        public float FoodProduced { get; }
        public float GrowthRateIncrease { get; }

        private List<string> _whichRacesCanNotBuild;
        private List<string> _dependsOnBuildings;

        private BuildingType(int id, string name, float constructionCost, float upkeepGold, float upkeepMana, float foodProduced, float growthRateIncrease, List<string> whichRacesCanNotBuild, List<string> dependsOnBuildings, Point slot)
        {
            Id = id;
            Name = name;
            Slot = slot;
            ConstructionCost = constructionCost;
            UpkeepGold = upkeepGold;
            UpkeepMana = upkeepMana;
            FoodProduced = foodProduced;
            GrowthRateIncrease = growthRateIncrease;

            _whichRacesCanNotBuild = whichRacesCanNotBuild;
            _dependsOnBuildings = dependsOnBuildings;
        }

        public static BuildingType Create(int id, string name, float constructionCost, float upkeepGold, float upkeepMana, float foodProduced, float growthRateIncrease, List<string> whichRacesCanNotBuild, List<string> dependsOnBuildings, Point slot)
        {
            return new BuildingType(id, name, constructionCost, upkeepGold, upkeepMana, foodProduced, growthRateIncrease, whichRacesCanNotBuild, dependsOnBuildings, slot);
        }

        public bool CanBeBuiltBy(string raceTypeName)
        {
            return !_whichRacesCanNotBuild.Contains(raceTypeName);
        }

        public bool CanNotBeBuiltBy(string raceTypeName)
        {
            return _whichRacesCanNotBuild.Contains(raceTypeName);
        }

        public bool IsReadyToBeBuilt(List<int> buildingsAlreadyBuilt)
        {
            var isReadyToBeBuilt = true;
            foreach (var building in _dependsOnBuildings)
            {
                var buildingId = Globals.Instance.BuildingTypes[building].Id;
                if (!buildingsAlreadyBuilt.Contains(buildingId))
                {
                    return false;
                }
            }

            return isReadyToBeBuilt;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }
}