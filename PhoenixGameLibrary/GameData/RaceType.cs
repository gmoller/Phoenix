using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Zen.Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct RaceType : IIdentifiedByIdAndName
    {
        public static readonly RaceType Invalid = new RaceType(-1, "None", 0.0f, 0, 0.0f, 0.0f, null);

        #region State
        public int Id { get; }
        public string Name { get; }
        public float FarmingRate { get; }
        public int GrowthRateModifier { get; }
        public float WorkerProductionRate { get; }
        public float FarmerProductionRate { get; }
        public string[] TownNames { get; }
        #endregion

        private RaceType(int id, string name, float farmingRate, int growthRateModifier, float workerProductionRate, float farmerProductionRate, string[] townNames)
        {
            Id = id;
            Name = name;
            FarmingRate = farmingRate;
            GrowthRateModifier = growthRateModifier;
            WorkerProductionRate = workerProductionRate;
            FarmerProductionRate = farmerProductionRate;
            TownNames = townNames;
        }

        public static RaceType Create(int id, string name, float farmingRate, int growthRateModifier, float workerProductionRate, float farmerProductionRate, string[] townNames)
        {
            return new RaceType(id, name, farmingRate, growthRateModifier, workerProductionRate, farmerProductionRate, townNames);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public struct RaceTypeForDeserialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float FarmingRate { get; set; }
        public int GrowthRateModifier { get; set; }
        public float WorkerProductionRate { get; set; }
        public float FarmerProductionRate { get; set; }
        public string[] TownNames { get; set; }
    }

    public static class RaceTypesLoader
    {
        public static NamedDataDictionary<RaceType> Load()
        {
            var raceTypes = new List<RaceType>
            {
                RaceType.Create(0, "Barbarians", 2.0f, 20, 2.0f, 0.5f, new [] { "Norport", "Bromburg", "Burglitz", "Danzig", "Flensburg", "Hannover", "Kufstein", "Strassburg", "Schleswig", "Zwolle", "Bradenburg", "Bamburg", "Deventor", "Freiburg", "Hamburg", "Konstanz", "Linz", "Rostock", "Stettin", "Soest" }),
                RaceType.Create(1, "Beastmen", 2.0f, 0, 2.0f, 0.5f,new [] { "Kempen" }),
                RaceType.Create(2, "Dark Elves", 2.0f, -20, 2.0f, 0.5f,new [] { "Leer" }),
                RaceType.Create(3, "Draconians", 2.0f, -10, 2.0f, 0.5f,new [] { "Vallis" }),
                RaceType.Create(4, "Dwarves", 2.0f, -20, 3.0f, 0.5f,new [] { "Ebonsway" }),
                RaceType.Create(5, "Gnolls", 2.0f, -10, 2.0f, 0.5f,new [] { "Basel" }),
                RaceType.Create(6, "Halflings", 3.0f, 0, 2.0f, 0.5f,new [] { "Miroban" }),
                RaceType.Create(7, "High Elves", 2.0f, -20, 2.0f, 0.5f,new [] { "Silverdale" }),
                RaceType.Create(8, "High Men", 2.0f, 0, 2.0f, 0.5f,new [] { "Coventry" }),
                RaceType.Create(9, "Klackons", 2.0f, -10, 3.0f, 0.5f,new [] { "Fa-rul" }),
                RaceType.Create(10, "Lizardmen", 2.0f, 10, 2.0f, 0.5f,new [] { "South Wash" }),
                RaceType.Create(11, "Nomads", 2.0f, -10, 2.0f, 0.5f,new [] { "Mecca" }),
                RaceType.Create(12, "Orcs", 2.0f, 0, 2.0f, 0.5f,new [] { "Robenaar" }),
                RaceType.Create(13, "Trolls", 2.0f, -20, 2.0f, 0.5f,new [] { "Erfurt" })
            };

            return NamedDataDictionary<RaceType>.Create(raceTypes);
        }

        public static List<RaceType> LoadFromJsonFile(string fileName)
        {
            var jsonString = File.ReadAllText($@".\Content\GameMetadata\{fileName}.json");
            var deserialized = JsonSerializer.Deserialize<List<RaceTypeForDeserialization>>(jsonString);
            var list = new List<RaceType>();
            foreach (var item in deserialized)
            {
                list.Add(RaceType.Create(item.Id, item.Name, item.FarmingRate, item.GrowthRateModifier, item.WorkerProductionRate, item.FarmerProductionRate, item.TownNames));
            }

            return list;
        }
    }
}