using System.Collections.Generic;
using System.Diagnostics;
using Utilities;

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
        public float MovementPoints { get; }
        public string TextureName { get; }

        public List<string> MovementTypes { get; }

        private readonly List<string> _whichRacesCanBuild;
        private readonly List<string> _dependsOnBuildings;
        private readonly List<string> _actions;

        private UnitType(int id, string name, string shortName, float constructionCost, float movementPoints, List<string> movementTypes, string textureName, List<string> whichRacesCanBuild, List<string> dependsOnBuildings, List<string> actions)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            ConstructionCost = constructionCost;
            MovementPoints = movementPoints;
            MovementTypes = movementTypes;
            TextureName = textureName;
            _whichRacesCanBuild = whichRacesCanBuild;
            _dependsOnBuildings = dependsOnBuildings;
            _actions = actions;
        }

        public static UnitType Create(int id, string name, string shortName, float constructionCost, float movementPoints, List<string> movementTypes, string textureName, List<string> whichRacesCanBuild, List<string> dependsOnBuildings, List<string> actions)
        {
            return new UnitType(id, name, shortName, constructionCost, movementPoints, movementTypes, textureName, whichRacesCanBuild, dependsOnBuildings, actions);
        }

        public bool CanBeBuiltBy(string name)
        {
            return _whichRacesCanBuild.Contains(name);
        }

        public bool IsReadyToBeBuilt(List<int> buildingsAlreadyBuilt)
        {
            foreach (var building in _dependsOnBuildings)
            {
                var buildingId = Globals.Instance.BuildingTypes[building].Id;
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

    public static class UnitTypesLoader
    {
        public static NamedDataDictionary<UnitType> Load()
        {
            var unitTypes = new List<UnitType>
            {
                UnitType.Create(0, "Barbarian Settlers", "Settlers", 60.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_Settlers_Transparent", new List<string> { "Barbarians" }, new List<string>(), new List<string> { "Done", "Patrol", "Wait", "BuildOutpost" }),
                UnitType.Create(1, "Barbarian Spearmen", "Spearmen", 15.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianSpearmen_Transparent", new List<string> { "Barbarians" }, new List<string>(), new List<string> { "Done", "Patrol", "Wait" }),
                UnitType.Create(2, "Barbarian Swordsmen", "Swordsmen", 30.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianSwordsmen_Transparent", new List<string> { "Barbarians" }, new List<string> { "Barracks", "Smithy" }, new List<string> { "Done", "Patrol", "Wait" }),
                UnitType.Create(3, "Barbarian Bowmen", "Bowmen", 30.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianBowmen_Transparent", new List<string> { "Barbarians" }, new List<string> { "Barracks", "Sawmill" }, new List<string> { "Done", "Patrol", "Wait" }),
                UnitType.Create(4, "Barbarian Cavalry", "Cavalry", 60.0f, 2.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianCavalry_Transparent", new List<string> { "Barbarians" }, new List<string> { "Barracks", "Stables" }, new List<string> { "Done", "Patrol", "Wait" }),
                UnitType.Create(5, "Barbarian Shamans", "Shamans", 50.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianShamans_Transparent", new List<string> { "Barbarians" }, new List<string> { "Shrine" }, new List<string> { "Done", "Patrol", "Wait" }),
                UnitType.Create(6, "Barbarian Trireme", "Trireme", 60.0f, 2.0f, new List<string> { "Sailing" }, "Unit_Icon_Trireme_Transparent", new List<string> { "Barbarians" }, new List<string> { "Shipwrights Guild" }, new List<string> { "Done", "Patrol", "Wait" }),
                UnitType.Create(7, "Barbarian Galley", "Galley", 100.0f, 3.0f, new List<string> { "Sailing" }, "Unit_Icon_Galley_Transparent", new List<string> { "Barbarians" }, new List<string> { "Shipyard" }, new List<string> { "Done", "Patrol", "Wait" }),
                UnitType.Create(8, "Barbarian Warship", "Warship", 160.0f, 4.0f, new List<string> { "Sailing" }, "Unit_Icon_Warship_Transparent", new List<string> { "Barbarians" }, new List<string> { "Maritime Guild" }, new List<string> { "Done", "Patrol", "Wait" }),
                UnitType.Create(9, "Barbarian Beserkers", "Beserkers", 120.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_Berserkers_Transparent", new List<string> { "Barbarians" }, new List<string> { "Armorers Guild" }, new List<string> { "Done", "Patrol", "Wait" }),

                UnitType.Create(10, "Beastmen Settlers", "Settlers", 120.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_Settlers_Transparent", new List<string> { "Beastmen" }, new List<string>(), new List<string> { "Done", "Patrol", "Wait", "BuildOutpost" }),
                UnitType.Create(11, "Beastmen Spearmen", "Spearmen", 20.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BeastmenSpearmen_Transparent", new List<string> { "Beastmen" }, new List<string>(), new List<string> { "Done", "Patrol", "Wait" }),

                UnitType.Create(12, "Lizardmen Spearmen", "Spearmen", 10.0f, 1.0f, new List<string> { "Walking", "Swimming" }, "Unit_Icon_BarbarianBowmen_Transparent" /* this is wrong */, new List<string> { "Lizardmen" }, new List<string>(), new List<string> { "Done", "Patrol", "Wait" }),

                UnitType.Create(13, "Nomad Griffins", "Griffins", 200.0f, 2.0f, new List<string> { "Flying" }, "Unit_Icon_Griffins_Transparent", new List<string> { "Nomads" }, new List<string> { "Fantastic Stable" }, new List<string> { "Done", "Patrol", "Wait" }),

                UnitType.Create(100, "Test Dude", "Test", 1.0f, 4.0f, new List<string> { "Walking", "Mountaineer", "Pathfinding", "PlaneShift" }, "Unit_Icon_Griffins_Transparent", new List<string> { "Barbarians" }, new List<string>(), new List<string> { "Done", "Patrol", "Wait" }),
            };

            return NamedDataDictionary<UnitType>.Create(unitTypes);
        }
    }
}