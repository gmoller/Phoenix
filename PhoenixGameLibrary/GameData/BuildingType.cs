using System.Collections.Generic;
using System.Diagnostics;
using Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct BuildingType : IIdentifiedByIdAndName
    {
        #region State
        public int Id { get; }
        public string Name { get; }
        public Point Slot { get; }
        public float ConstructionCost { get; }
        public float UpkeepGold { get; }
        public float UpkeepMana { get; }

        private readonly List<string> _whichRacesCanNotBuild;
        private readonly List<string> _dependsOnBuildings;
        #endregion

        private BuildingType(int id, string name, float constructionCost, float upkeepGold, float upkeepMana, List<string> whichRacesCanNotBuild, List<string> dependsOnBuildings, Point slot)
        {
            Id = id;
            Name = name;
            Slot = slot;
            ConstructionCost = constructionCost;
            UpkeepGold = upkeepGold;
            UpkeepMana = upkeepMana;

            _whichRacesCanNotBuild = whichRacesCanNotBuild;
            _dependsOnBuildings = dependsOnBuildings;
        }

        public static BuildingType Create(int id, string name, float constructionCost, float upkeepGold, float upkeepMana, List<string> whichRacesCanNotBuild, List<string> dependsOnBuildings, Point slot)
        {
            return new BuildingType(id, name, constructionCost, upkeepGold, upkeepMana, whichRacesCanNotBuild, dependsOnBuildings, slot);
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
            if (buildingsAlreadyBuilt.Contains(Id)) return false;

            var context = CallContext<GlobalContext>.GetData("AmbientGlobalContext");
            var buildingTypes = context.GameMetadata.BuildingTypes;

            foreach (var building in _dependsOnBuildings)
            {
                var buildingId = buildingTypes[building].Id;
                if (!buildingsAlreadyBuilt.Contains(buildingId))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class BuildingTypesLoader
    {
        public static NamedDataDictionary<BuildingType> Load()
        {
            var buildingTypes = new List<BuildingType>
            {
                BuildingType.Create(0, "Barracks", 30.0f, 0.0f, 0.0f, new List<string>(), new List<string>(), new Point(2, 0)),
                BuildingType.Create(1, "Armory", 80.0f, 2.0f, 0.0f, new List<string>(), new List<string> { "Barracks", "Smithy" }, new Point(2, 2)),
                BuildingType.Create(2, "Fighters Guild", 200.0f, 3.0f, 0.0f, new List<string>(), new List<string> { "Armory" }, new Point(2, 4)), // all new normal units start with 20 experience points
                BuildingType.Create(3, "Armorers Guild", 350.0f, 4.0f, 0.0f, new List<string> { "Halflings", "Gnolls" }, new List<string> { "Fighters Guild" }, new Point(2, 6)),
                BuildingType.Create(4, "War College", 500.0f, 5.0f, 0.0f, new List<string> { "Barbarians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen", "Trolls" }, new List<string> { "University", "Armorers Guild" }, new Point(2, 14)), // all new normal units start with 61 experience points
                BuildingType.Create(5, "Smithy", 40.0f, 1.0f, 0.0f, new List<string>(), new List<string>(), new Point(3, 0)),
                BuildingType.Create(6, "Stables", 80.0f, 2.0f, 0.0f, new List<string> { "Dwarves", "Halflings" }, new List<string> { "Smithy" }, new Point(3, 2)),
                BuildingType.Create(7, "Animists Guild", 300.0f, 5.0f, 0.0f, new List<string> { "Barbarians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen" }, new List<string> { "Stables", "Temple" }, new Point(3, 8)), // farmers x3, -1 unrest, stationed troops heal 66.7% faster
                BuildingType.Create(8, "Fantastic Stable", 600.0f, 6.0f, 0.0f, new List<string> { "Barbarians", "Beastmen", "Draconians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen", "Trolls" }, new List<string> { "Animists Guild", "Armorers Guild" }, new Point(3, 10)),
                BuildingType.Create(9, "Shipwrights Guild", 100.0f, 1.0f, 0.0f, new List<string> { "Lizardmen" }, new List<string>(), new Point(0, 0)),
                BuildingType.Create(10, "Shipyard", 200.0f, 2.0f, 0.0f, new List<string> { "Beastmen", "Dwarves", "Halflings", "Klackons", "Lizardmen", "Trolls" }, new List<string> { "Shipwrights Guild", "Sawmill" }, new Point(0, 2)),
                BuildingType.Create(11, "Maritime Guild", 400.0f, 4.0f, 0.0f, new List<string> { "Beastmen", "DarkElves", "Draconians", "Dwarves", "Gnolls", "Halflings", "HighElves", "Klackons", "Lizardmen", "Nomads", "Trolls" }, new List<string> { "Armory", "Shipyard" }, new Point(1, 4)),
                BuildingType.Create(12, "Sawmill", 100.0f, 2.0f, 0.0f, new List<string> { "Lizardmen" }, new List<string>(), new Point(1, 0)), // +25% production
                BuildingType.Create(13, "Library", 60.0f, 1.0f, 0.0f, new List<string>(), new List<string> { "Builders Hall" }, new Point(7, 4)), // +2 research points
                BuildingType.Create(14, "Sages Guild", 120.0f, 2.0f, 0.0f, new List<string> { "Gnolls", "Klackons", "Lizardmen", "Trolls" }, new List<string> { "Library" }, new Point(7, 6)), // +3 research points
                BuildingType.Create(15, "Oracle", 500.0f, 4.0f, 0.0f, new List<string> { "Barbarians", "Dwarves", "Gnolls", "Halflings", "HighElves", "Klackons", "Lizardmen", "Trolls" }, new List<string> { "University", "Parthenon" }, new Point(6, 12)), // sight range 4, -2 unrest
                BuildingType.Create(16, "Alchemists Guild", 250.0f, 3.0f, 0.0f, new List<string> { "Gnolls", "Klackons", "Lizardmen", "Trolls" }, new List<string> { "Sages Guild" }, new Point(8, 8)), // +3 mana, newly built troops have magic weapons
                BuildingType.Create(17, "University", 300.0f, 3.0f, 0.0f, new List<string> { "Barbarians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen", "Trolls" }, new List<string> { "Sages Guild" }, new Point(7, 8)), // +5 research points
                BuildingType.Create(18, "Wizards Guild", 1000.0f, 5.0f, 3.0f, new List<string> { "Barbarians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen", "Trolls", "Nomads" }, new List<string> { "University", "Alchemists Guild" }, new Point(8, 10)), // +8 research points
                BuildingType.Create(19, "Shrine", 100.0f, 1.0f, 0.0f, new List<string>(), new List<string> { "Builders Hall" }, new Point(5, 4)), // +1 mana, -1 unrest
                BuildingType.Create(20, "Temple", 200.0f, 2.0f, 0.0f, new List<string> { "Klackons" }, new List<string> { "Shrine" }, new Point(5, 6)), // +2 mana, -1 unrest
                BuildingType.Create(21, "Parthenon", 400.0f, 3.0f, 0.0f, new List<string> { "Dwarves", "Gnolls", "HighElves", "Klackons", "Lizardmen" }, new List<string> { "Temple" }, new Point(5, 8)), // +3 mana, -1 unrest
                BuildingType.Create(22, "Cathedral", 800.0f, 4.0f, 0.0f, new List<string> { "Barbarians", "DarkElves", "Dwarves", "Gnolls", "HighElves", "Klackons", "Lizardmen" }, new List<string> { "Parthenon" }, new Point(5, 12)), // +4 mana, -1 unrest
                BuildingType.Create(23, "Marketplace", 100.0f, 1.0f, 0.0f, new List<string>(), new List<string> { "Smithy" }, new Point(9, 2)), // +50% gold
                BuildingType.Create(24, "Bank", 250.0f, 3.0f, 0.0f,  new List<string> { "Barbarians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen", "Trolls" }, new List<string> { "Marketplace", "University" }, new Point(9, 14)), // +50% gold
                BuildingType.Create(25, "Merchants Guild", 600.0f, 5.0f, 0.0f, new List<string> { "Barbarians", "Beastmen", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen", "Trolls" }, new List<string> { "Bank", "Shipyard" }, new Point(0, 16)), // +100% gold
                BuildingType.Create(26, "Granary", 40.0f, 1.0f, 0.0f, new List<string>(), new List<string> { "Builders Hall" }, new Point(8, 4)),
                BuildingType.Create(27, "Farmers Market", 100.0f, 2.0f, 0.0f, new List<string>(), new List<string> { "Granary", "Marketplace" }, new Point(8, 6)),
                BuildingType.Create(28, "Foresters Guild", 200.0f, 2.0f, 0.0f, new List<string> { "Lizardmen" }, new List<string> { "Sawmill" }, new Point(1, 2)), // +25% prod
                BuildingType.Create(29, "Builders Hall", 60.0f, 1.0f, 0.0f, new List<string>(), new List<string>(), new Point(6, 2)),
                BuildingType.Create(30, "Mechanicians Guild", 600.0f, 5.0f, 0.0f, new List<string> { "Barbarians", "Beastmen", "Draconians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen" }, new List<string> { "University", "Miners Guild" }, new Point(6, 10)), // +50% prod
                BuildingType.Create(31, "Miners Guild", 300.0f, 3.0f, 0.0f, new List<string> { "Lizardmen", "Trolls" }, new List<string> { "Builders Hall" }, new Point(6, 4)), // minerals +50%, +50% prod
                BuildingType.Create(32, "City Walls", 150.0f, 2.0f, 0.0f, new List<string>(), new List<string> { "Builders Hall" }, new Point(4, 4)), // sight range 3
            };

            return NamedDataDictionary<BuildingType>.Create(buildingTypes);
        }
    }
}