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
    public readonly struct BuildingPopulationGrowthType : IIdentifiedById
    {
        #region State
        public int Id { get; }
        public int BuildingId { get; }
        public int PopulationGrowthRateIncrease { get; }
        #endregion

        private BuildingPopulationGrowthType(int id, int buildingId, int populationGrowthRateIncrease)
        {
            Id = id;
            BuildingId = buildingId;
            PopulationGrowthRateIncrease = populationGrowthRateIncrease;
        }

        public static BuildingPopulationGrowthType Create(int id, int buildingId, int populationGrowthRateIncrease)
        {
            return new BuildingPopulationGrowthType(id, buildingId, populationGrowthRateIncrease);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},BuildingId={BuildingId},PopulationGrowthRateIncrease={PopulationGrowthRateIncrease}}}";
    }

    public struct BuildingPopulationGrowthTypeForDeserialization
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int PopulationGrowthRateIncrease { get; set; }
    }

    public static class BuildingPopulationGrowthTypesLoader
    {
        public static DataDictionary<BuildingPopulationGrowthType> Load()
        {
            var buildingPopulationGrowthTypes = new List<BuildingPopulationGrowthType>
            {
                BuildingPopulationGrowthType.Create(1, 26, 20),
                BuildingPopulationGrowthType.Create(2, 27, 30)
            };

            return DataDictionary<BuildingPopulationGrowthType>.Create(buildingPopulationGrowthTypes);
        }

        public static List<BuildingPopulationGrowthType> LoadFromJsonFile(string fileName)
        {
            var jsonString = File.ReadAllText($@".\Content\GameMetadata\{fileName}.json");
            var deserialized = JsonSerializer.Deserialize<List<BuildingPopulationGrowthTypeForDeserialization>>(jsonString);
            var list = new List<BuildingPopulationGrowthType>();
            foreach (var item in deserialized)
            {
                list.Add(BuildingPopulationGrowthType.Create(item.Id, item.BuildingId, item.PopulationGrowthRateIncrease));
            }

            return list;
        }
    }
}