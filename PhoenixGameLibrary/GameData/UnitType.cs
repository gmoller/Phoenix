using System.Collections.Generic;
using System.Diagnostics;
using GameLogic;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct UnitType : IIdentifiedByIdAndName
    {
        public int Id { get; }
        public string Name { get; }
        public string ShortName { get; }
        public float ConstructionCost { get; }

        private List<string> _whichRacesCanBuild;
        private List<string> _dependsOnBuildings;

        private UnitType(int id, string name, string shortName, float constructionCost, List<string> whichRacesCanBuild, List<string> dependsOnBuildings)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            ConstructionCost = constructionCost;
            _whichRacesCanBuild = whichRacesCanBuild;
            _dependsOnBuildings = dependsOnBuildings;
        }

        public static UnitType Create(int id, string name, string shortName, float constructionCost, List<string> whichRacesCanBuild, List<string> dependsOnBuildings)
        {
            return new UnitType(id, name, shortName, constructionCost, whichRacesCanBuild, dependsOnBuildings);
        }

        public bool CanBeBuiltBy(string name)
        {
            return _whichRacesCanBuild.Contains(name);
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

    public static class UnitTypesLoader
    {
        public static NamedDataList<UnitType> Load()
        {
            var unitTypes = new List<UnitType>
            {
                UnitType.Create(0, "Barbarian Settlers", "Settlers", 60, new List<string> { "Barbarians" }, new List<string>()),
                UnitType.Create(1, "Barbarian Spearmen", "Spearmen", 15, new List<string> { "Barbarians" }, new List<string>()),
                UnitType.Create(2, "Barbarian Swordsmen", "Swordsmen", 30, new List<string> { "Barbarians" }, new List<string> { "Barracks", "Smithy" }),
            };

            return NamedDataList<UnitType>.Create(unitTypes);
        }
    }
}