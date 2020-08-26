using System.Collections.Generic;
using System.Diagnostics;
using Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct BuildingPopulationGrowthType : IIdentifiedById
    {
        #region State
        public int Id { get; }
        public int PopulationGrowthRateIncrease { get; }
        #endregion

        private BuildingPopulationGrowthType(int buildingId, int populationGrowthRateIncrease)
        {
            Id = buildingId;
            PopulationGrowthRateIncrease = populationGrowthRateIncrease;
        }

        public static BuildingPopulationGrowthType Create(int buildingId, int populationGrowthRateIncrease)
        {
            return new BuildingPopulationGrowthType(buildingId, populationGrowthRateIncrease);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},PopulationGrowthRateIncrease={PopulationGrowthRateIncrease}}}";
    }

    public static class BuildingPopulationGrowthTypesLoader
    {
        public static DataDictionary<BuildingPopulationGrowthType> Load()
        {
            var buildingPopulationGrowthTypes = new List<BuildingPopulationGrowthType>
            {
                BuildingPopulationGrowthType.Create(26, 20),
                BuildingPopulationGrowthType.Create(27, 30)
            };

            return DataDictionary<BuildingPopulationGrowthType>.Create(buildingPopulationGrowthTypes);
        }
    }
}