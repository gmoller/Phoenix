using System.Collections.Generic;
using System.Diagnostics;
using Zen.Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct TerrainFoodOutputType : IIdentifiedById
    {
        #region State
        public int Id { get; }
        public float FoodOutput { get; }
        #endregion

        private TerrainFoodOutputType(int terrainId, float foodOutput)
        {
            Id = terrainId;
            FoodOutput = foodOutput;
        }

        public static TerrainFoodOutputType Create(int terrainId, float foodOutput)
        {
            return new TerrainFoodOutputType(terrainId, foodOutput);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},FoodOutput={FoodOutput}}}";
    }

    public static class TerrainFoodOutputTypesLoader
    {
        public static DataDictionary<TerrainFoodOutputType> Load()
        {
            var terrainFoodOutputTypes = new List<TerrainFoodOutputType>
            {
                TerrainFoodOutputType.Create(0, 1.5f),
                TerrainFoodOutputType.Create(1, 0.5f),
                TerrainFoodOutputType.Create(6, 0.5f)
            };

            return DataDictionary<TerrainFoodOutputType>.Create(terrainFoodOutputTypes);
        }
    }
}