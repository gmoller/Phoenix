using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Zen.Utilities;
using Zen.Utilities.ExtensionMethods;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct UnitType : IIdentifiedByIdAndName
    {
        #region State
        public int Id { get; }
        public string Name { get; }
        public string ShortName { get; }
        public float ConstructionCost { get; }
        public float MovementPoints { get; }
        public string TextureName { get; }

        public List<string> MovementTypes { get; }
        public List<string> Actions { get; }

        private readonly List<string> _whichRacesCanBuild;
        private readonly List<string> _dependsOnBuildings;
        #endregion

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
            Actions = actions;
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
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var buildingTypes = gameMetadata.BuildingTypes;

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

        #region Overrides and Overloads

        public override bool Equals(object obj)
        {
            return obj is UnitType unitType && this == unitType;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode() ^ ShortName.GetHashCode() ^ ConstructionCost.GetHashCode() ^ MovementPoints.GetHashCode() ^ TextureName.GetHashCode() ^ MovementTypes.GetHashCode() ^ Actions.GetHashCode() ^ _whichRacesCanBuild.GetHashCode() ^ _dependsOnBuildings.GetHashCode();
        }

        public static bool operator == (UnitType a, UnitType b)
        {
            return a.Id == b.Id && a.Name == b.Name && a.ShortName == b.ShortName && a.ConstructionCost.AboutEquals(b.ConstructionCost) && a.MovementPoints.AboutEquals(b.MovementPoints) && a.TextureName == b.TextureName && a.MovementTypes == b.MovementTypes && a.Actions == b.Actions && a._whichRacesCanBuild == b._whichRacesCanBuild && a._dependsOnBuildings == b._dependsOnBuildings;
        }

        public static bool operator != (UnitType a, UnitType b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";

        #endregion
    }

    public struct UnitTypeForDeserialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public float ConstructionCost { get; set; }
        public float MovementPoints { get; set; }
        public string TextureName { get; set; }
        public List<string> MovementTypes { get; }
        public List<string> Actions { get; }
        public List<string> WhichRacesCanBuild;
        public List<string> DependsOnBuildings;
    }

    public static class UnitTypesLoader
    {
        public static NamedDataDictionary<UnitType> Load()
        {
            var unitTypes = new List<UnitType>
            {
                UnitType.Create(0, "Barbarian Settlers", "Settlers", 60.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_Settlers_Transparent", new List<string> { "Barbarians" }, new List<string>(), new List<string> { "BuildOutpost" }),
                UnitType.Create(1, "Barbarian Spearmen", "Spearmen", 15.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianSpearmen_Transparent", new List<string> { "Barbarians" }, new List<string>(), new List<string>()),
                UnitType.Create(2, "Barbarian Swordsmen", "Swordsmen", 30.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianSwordsmen_Transparent", new List<string> { "Barbarians" }, new List<string> { "Barracks", "Smithy" }, new List<string>()),
                UnitType.Create(3, "Barbarian Bowmen", "Bowmen", 30.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianBowmen_Transparent", new List<string> { "Barbarians" }, new List<string> { "Barracks", "Sawmill" }, new List<string>()),
                UnitType.Create(4, "Barbarian Cavalry", "Cavalry", 60.0f, 2.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianCavalry_Transparent", new List<string> { "Barbarians" }, new List<string> { "Barracks", "Stables" }, new List<string>()),
                UnitType.Create(5, "Barbarian Shamans", "Shamans", 50.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BarbarianShamans_Transparent", new List<string> { "Barbarians" }, new List<string> { "Shrine" }, new List<string>()),
                UnitType.Create(6, "Barbarian Trireme", "Trireme", 60.0f, 2.0f, new List<string> { "Sailing" }, "Unit_Icon_Trireme_Transparent", new List<string> { "Barbarians" }, new List<string> { "Shipwrights Guild" }, new List<string>()),
                UnitType.Create(7, "Barbarian Galley", "Galley", 100.0f, 3.0f, new List<string> { "Sailing" }, "Unit_Icon_Galley_Transparent", new List<string> { "Barbarians" }, new List<string> { "Shipyard" }, new List<string>()),
                UnitType.Create(8, "Barbarian Warship", "Warship", 160.0f, 4.0f, new List<string> { "Sailing" }, "Unit_Icon_Warship_Transparent", new List<string> { "Barbarians" }, new List<string> { "Maritime Guild" }, new List<string>()),
                UnitType.Create(9, "Barbarian Beserkers", "Beserkers", 120.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_Berserkers_Transparent", new List<string> { "Barbarians" }, new List<string> { "Armorers Guild" }, new List<string>()),

                UnitType.Create(10, "Beastmen Settlers", "Settlers", 120.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_Settlers_Transparent", new List<string> { "Beastmen" }, new List<string>(), new List<string> { "BuildOutpost" }),
                UnitType.Create(11, "Beastmen Spearmen", "Spearmen", 20.0f, 1.0f, new List<string> { "Walking" }, "Unit_Icon_BeastmenSpearmen_Transparent", new List<string> { "Beastmen" }, new List<string>(), new List<string>()),

                UnitType.Create(12, "Lizardmen Spearmen", "Spearmen", 10.0f, 1.0f, new List<string> { "Walking", "Swimming" }, "Unit_Icon_BarbarianBowmen_Transparent" /* this is wrong */, new List<string> { "Lizardmen" }, new List<string>(), new List<string>()),

                UnitType.Create(13, "Nomad Griffins", "Griffins", 200.0f, 2.0f, new List<string> { "Flying" }, "Unit_Icon_Griffins_Transparent", new List<string> { "Nomads" }, new List<string> { "Fantastic Stable" }, new List<string>()),

                UnitType.Create(100, "Test Dude", "Test", 1.0f, 4000.0f, new List<string> { "Walking", "Flying", "Mountaineer", "Pathfinding", "PlaneShift" }, "Unit_Icon_Griffins_Transparent", new List<string> { "Barbarians" }, new List<string>(), new List<string>()),
            };

            //var foo = JsonSerializer.Serialize(unitTypes);

            return NamedDataDictionary<UnitType>.Create(unitTypes);
        }

        public static List<UnitType> LoadFromJsonFile(string fileName)
        {
            var jsonString = File.ReadAllText($@".\Content\GameMetadata\{fileName}.json");
            var deserialized = JsonSerializer.Deserialize<List<UnitTypeForDeserialization>>(jsonString);
            var list = new List<UnitType>();
            foreach (var item in deserialized)
            {
                list.Add(UnitType.Create(item.Id, item.Name, item.ShortName, item.ConstructionCost, item.MovementPoints, item.MovementTypes, item.TextureName, item.WhichRacesCanBuild, item.DependsOnBuildings, item.Actions));
            }

            return list;
        }
    }
}