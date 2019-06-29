using System.Collections.Generic;

namespace PhoenixGameLibrary.GameData
{
    public static class RaceTypesLoader
    {
        public static List<RaceType> GetRaceTypes()
        {
            var raceTypes = new List<RaceType>
            {
                RaceType.Create(0, "Barbarians", 2.0f, 20, 2.0f, 0.5f),
                RaceType.Create(1, "Beastmen", 2.0f, 0, 2.0f, 0.5f),
                RaceType.Create(2, "Dark Elves", 2.0f, -20, 2.0f, 0.5f),
                RaceType.Create(3, "Draconians", 2.0f, -10, 2.0f, 0.5f),
                RaceType.Create(4, "Dwarves", 2.0f, -20, 3.0f, 0.5f),
                RaceType.Create(5, "Gnolls", 2.0f, -10, 2.0f, 0.5f),
                RaceType.Create(6, "Halflings", 3.0f, 0, 2.0f, 0.5f),
                RaceType.Create(7, "High Elves", 2.0f, -20, 2.0f, 0.5f),
                RaceType.Create(8, "High Men", 2.0f, 0, 2.0f, 0.5f),
                RaceType.Create(9, "Klackons", 2.0f, -10, 3.0f, 0.5f),
                RaceType.Create(10, "Lizardmen", 2.0f, 10, 2.0f, 0.5f),
                RaceType.Create(11, "Nomads", 2.0f, -10, 2.0f, 0.5f),
                RaceType.Create(12, "Orcs", 2.0f, 0, 2.0f, 0.5f),
                RaceType.Create(13, "Trolls", 2.0f, -20, 2.0f, 0.5f)
            };

            return raceTypes;
        }
    }
}