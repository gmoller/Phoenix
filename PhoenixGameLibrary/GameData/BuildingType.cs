using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

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

        private List<int> _whichRacesCanBuild;
        private List<string> _dependsOnBuildings;

        private BuildingType(int id, string name, float constructionCost, float upkeepGold, float upkeepMana, float foodProduced, float growthRateIncrease, List<string> whichRacesCanNotBuild, List<string> dependsOnBuildings, Point slot, RaceTypes raceTypes)
        {
            Id = id;
            Name = name;
            Slot = slot;
            ConstructionCost = constructionCost;
            UpkeepGold = upkeepGold;
            UpkeepMana = upkeepMana;
            FoodProduced = foodProduced;
            GrowthRateIncrease = growthRateIncrease;

            _whichRacesCanBuild = new List<int>();
            foreach (var raceType in raceTypes)
            {
                if (!whichRacesCanNotBuild.Contains(raceType.Name))
                {
                    _whichRacesCanBuild.Add(raceType.Id);
                }
            }

            _dependsOnBuildings = dependsOnBuildings;
        }

        public static BuildingType Create(int id, string name, float constructionCost, float upkeepGold, float upkeepMana, float foodProduced, float growthRateIncrease, List<string> whichRacesCanNotBuild, List<string> dependsOnBuildings, Point slot, RaceTypes raceTypes)
        {
            return new BuildingType(id, name, constructionCost, upkeepGold, upkeepMana, foodProduced, growthRateIncrease, whichRacesCanNotBuild, dependsOnBuildings, slot, raceTypes);
        }

        public bool CanBeBuiltBy(int raceTypeId)
        {
            return _whichRacesCanBuild.Contains(raceTypeId);
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }
}