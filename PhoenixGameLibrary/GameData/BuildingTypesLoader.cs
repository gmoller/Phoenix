using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PhoenixGameLibrary.GameData
{
    public static class BuildingTypesLoader
    {
        public static BuildingTypes GetBuildingTypes(RaceTypes raceTypes)
        {
            var buildingTypes = new List<BuildingType>
            {
                BuildingType.Create(0, "Barracks", 30.0f, 0.0f, 0.0f, 0.0f, 0.0f, new List<string>(), new List<string>(), new Point(2, 0), raceTypes),
                ///BuildingType.Create(1, "Armory", 80.0f, 2.0f, 0.0f, 0.0f, 0.0f, new List<string>(), new Point(2, 2), raceTypes),
                ///BuildingType.Create(2, "FightersGuild", 200.0f, 3.0f, 0.0f, 0.0f, 0.0f, new List<string>(), new Point(0, 0), raceTypes), // all new normal units start with 20 experience points
                ///BuildingType.Create(3, "ArmorersGuild", 350.0f, 4.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Halflings", "Gnolls" }, new Point(0, 0), raceTypes),
                ///BuildingType.Create(4, "WarCollege", 500.0f, 5.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Barbarians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen", "Trolls" }, new Point(0, 0), raceTypes), // all new normal units start with 61 experience points
                BuildingType.Create(5, "Smithy", 40.0f, 1.0f, 0.0f, 0.0f, 0.0f, new List<string>(), new List<string>(), new Point(3, 0), raceTypes),
                ///BuildingType.Create(6, "Stables", 80.0f, 2.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Dwarves", "Halflings" }, new Point(0, 0), raceTypes),
                ///BuildingType.Create(7, "AnimistsGuild", 300.0f, 5.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Barbarians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen" }, new Point(0, 0), raceTypes), // farmers x3, -1 unrest, stationed troops heal 66.7% faster
                ///BuildingType.Create(8, "FantasticStable", 600.0f, 6.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Barbarians", "Beastmen", "Draconians", "Dwarves", "Gnolls", "Halflings", "Klackons", "Lizardmen", "Trolls" }, new Point(0, 0), raceTypes),
                BuildingType.Create(9, "ShipwrightsGuild", 100.0f, 1.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Lizardmen" }, new List<string>(), new Point(0, 0), raceTypes),
                ///BuildingType.Create(10, "Shipyard", 200.0f, 2.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Beastmen", "Dwarves", "Halflings", "Klackons", "Lizardmen", "Trolls" }, new Point(0, 2), raceTypes),
                ///BuildingType.Create(11, "MaritimeGuild", 400.0f, 4.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Beastmen", "DarkElves", "Draconians", "Dwarves", "Gnolls", "Halflings", "HighElves", "Klackons", "Lizardmen", "Nomads", "Trolls" }, new Point(0, 0), raceTypes),
                BuildingType.Create(12, "Sawmill", 100.0f, 2.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Lizardmen" }, new List<string>(), new Point(1, 0), raceTypes), // +25% production
                ///BuildingType.Create(13, "Library", 60.0f, 1.0f, 0.0f, 0.0f, 0.0f, new List<string>(), new Point(0, 0), raceTypes), // +2 research points
                ///BuildingType.Create(14, "SagesGuild", 120.0f, 2.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Gnolls", "Klackons", "Lizardmen", "Trolls" }, new Point(0, 0), raceTypes), // +3 research points
                ///BuildingType.Create(15, "Oracle", 500.0f, 4.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Barbarians", "Dwarves", "Gnolls", "Halflings", "HighElves", "Klackons", "Lizardmen", "Trolls" }, new Point(0, 0), raceTypes), // sight range 4, -2 unrest
                ///BuildingType.Create(16, "AlchemistsGuild", 250.0f, 3.0f, 0.0f, 0.0f, 0.0f, new List<string> { "Gnolls", "Klackons", "Lizardmen", "Trolls" }, new Point(0, 0), raceTypes), // +3 mana, newly built troops have magic weapons
                //BuildingType.Create(17, "University", 300.0f, 3.0f, 0.0f, 0.0f, 0.0f), // +5 research points
                //BuildingType.Create(18, "Wizards Guild", 1000.0f, 5.0f, 3.0f, 0.0f, 0.0f), // +8 research points
                ///BuildingType.Create(19, "Shrine", 100.0f, 1.0f, 0.0f, 0.0f, 0.0f, new List<string>(), new Point(0, 0), raceTypes), // +1 mana, -1 unrest
                //BuildingType.Create(20, "Temple", 200.0f, 2.0f, 0.0f, 0.0f, 0.0f), // +2 mana, -1 unrest
                //BuildingType.Create(21, "Parthenon", 400.0f, 3.0f, 0.0f, 0.0f, 0.0f), // +3 mana, -1 unrest
                //BuildingType.Create(22, "Cathedral", 800.0f, 4.0f, 0.0f, 0.0f, 0.0f), // +4 mana, -1 unrest
                ///BuildingType.Create(23, "Marketplace", 100.0f, 1.0f, 0.0f, 0.0f, 0.0f, new List<string>(), new Point(0, 0), raceTypes), // +50% gold
                //BuildingType.Create(24, "Bank", 250.0f, 3.0f, 0.0f, 0.0f, 0.0f), // +50% gold
                //BuildingType.Create(25, "Merchants Guild", 600.0f, 5.0f, 0.0f, 0.0f, 0.0f), // +100% gold
                BuildingType.Create(26, "Granary", 40.0f, 1.0f, 0.0f, 2.0f, 20.0f, new List<string>(), new List<string> { "BuildersHall" }, new Point(8, 4), raceTypes), // +2 max population
                //BuildingType.Create(27, "Farmers Market", 100.0f, 2.0f, 0.0f, 3.0f, 30.0f), // +3 max population
                //BuildingType.Create(28, "Foresters Guild", 200.0f, 2.0f, 0.0f, 2.0f, 0.0f), // +25% prod
                BuildingType.Create(29, "BuildersHall", 60.0f, 1.0f, 0.0f, 0.0f, 0.0f, new List<string>(), new List<string>(), new Point(6, 2), raceTypes),
                //BuildingType.Create(30, "Mechanicians Guild", 600.0f, 5.0f, 0.0f, 0.0f, 0.0f), // +50% prod
                //BuildingType.Create(31, "Miners Guild", 300.0f, 3.0f, 0.0f, 0.0f, 0.0f), // minerals +50%, +50% prod
                ///BuildingType.Create(32, "CityWalls", 150.0f, 2.0f, 0.0f, 0.0f, 0.0f, new List<string>(), new Point(0, 0), raceTypes), // sight range 3
            };

            return BuildingTypes.Create(buildingTypes);
        }
    }
}