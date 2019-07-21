using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct BuildingFoodOutputIncreaseType : IIdentifiedById
    {
        public int Id { get; }
        public int FoodOutputIncrease { get; }

        private BuildingFoodOutputIncreaseType(int buildingId, int foodOutputIncrease)
        {
            Id = buildingId;
            FoodOutputIncrease = foodOutputIncrease;
        }

        public static BuildingFoodOutputIncreaseType Create(int buildingId, int foodOutputIncrease)
        {
            return new BuildingFoodOutputIncreaseType(buildingId, foodOutputIncrease);
        }

        private string DebuggerDisplay => $"{{Id={Id},FoodOutputIncrease={FoodOutputIncrease}}}";
    }

    public static class BuildingFoodOutputIncreaseTypesLoader
    {
        public static DataList<BuildingFoodOutputIncreaseType> Load()
        {
            var buildingFoodOutputIncreaseTypes = new List<BuildingFoodOutputIncreaseType>
            {
                BuildingFoodOutputIncreaseType.Create(26, 2),
                BuildingFoodOutputIncreaseType.Create(27, 3),
                BuildingFoodOutputIncreaseType.Create(28, 2)
            };

            return DataList<BuildingFoodOutputIncreaseType>.Create(buildingFoodOutputIncreaseTypes);
        }
    }
}