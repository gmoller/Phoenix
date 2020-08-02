using System.Collections.Generic;
using System.Diagnostics;
using Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct BuildingMaximumPopulationIncreaseType : IIdentifiedById
    {
        public int Id { get; }
        public int MaximumPopulationIncrease { get; }

        private BuildingMaximumPopulationIncreaseType(int buildingId, int maximumPopulationIncrease)
        {
            Id = buildingId;
            MaximumPopulationIncrease = maximumPopulationIncrease;
        }

        public static BuildingMaximumPopulationIncreaseType Create(int buildingId, int populationGrowthRateIncrease)
        {
            return new BuildingMaximumPopulationIncreaseType(buildingId, populationGrowthRateIncrease);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},MaximumPopulationIncrease={MaximumPopulationIncrease}}}";
    }

    public static class BuildingMaximumPopulationIncreaseTypesLoader
    {
        public static DataDictionary<BuildingMaximumPopulationIncreaseType> Load()
        {
            var buildingMaximumPopulationIncreaseTypes = new List<BuildingMaximumPopulationIncreaseType>
            {
                BuildingMaximumPopulationIncreaseType.Create(26, 2),
                BuildingMaximumPopulationIncreaseType.Create(27, 3)
            };

            return DataDictionary<BuildingMaximumPopulationIncreaseType>.Create(buildingMaximumPopulationIncreaseTypes);
        }
    }
}