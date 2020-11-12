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
    public readonly struct BuildingMaximumPopulationIncreaseType : IIdentifiedById
    {
        #region State
        public int Id { get; }
        public int BuildingId { get; }
        public int MaximumPopulationIncrease { get; }
        #endregion End State

        private BuildingMaximumPopulationIncreaseType(int id, int buildingId, int maximumPopulationIncrease)
        {
            Id = id;
            BuildingId = buildingId;
            MaximumPopulationIncrease = maximumPopulationIncrease;
        }

        public static BuildingMaximumPopulationIncreaseType Create(int id, int buildingId, int populationGrowthRateIncrease)
        {
            return new BuildingMaximumPopulationIncreaseType(id, buildingId, populationGrowthRateIncrease);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},BuildingId={BuildingId},MaximumPopulationIncrease={MaximumPopulationIncrease}}}";
    }

    public struct BuildingMaximumPopulationIncreaseTypeForDeserialization
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int MaximumPopulationIncrease { get; set; }
    }

    public static class BuildingMaximumPopulationIncreaseTypesLoader
    {
        public static DataDictionary<BuildingMaximumPopulationIncreaseType> Load()
        {
            var buildingMaximumPopulationIncreaseTypes = new List<BuildingMaximumPopulationIncreaseType>
            {
                BuildingMaximumPopulationIncreaseType.Create(1, 26, 2),
                BuildingMaximumPopulationIncreaseType.Create(2, 27, 3)
            };

            return DataDictionary<BuildingMaximumPopulationIncreaseType>.Create(buildingMaximumPopulationIncreaseTypes);
        }

        public static List<BuildingMaximumPopulationIncreaseType> LoadFromJsonFile(string fileName)
        {
            var jsonString = File.ReadAllText($@".\Content\GameMetadata\{fileName}.json");
            var deserialized = JsonSerializer.Deserialize<List<BuildingMaximumPopulationIncreaseTypeForDeserialization>>(jsonString);
            var list = new List<BuildingMaximumPopulationIncreaseType>();
            foreach (var item in deserialized)
            {
                list.Add(BuildingMaximumPopulationIncreaseType.Create(item.Id, item.BuildingId, item.MaximumPopulationIncrease));
            }

            return list;
        }
    }
}