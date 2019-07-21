﻿using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct TerrainFoodOutputType : IIdentifiedById
    {
        public int Id { get; }
        public float FoodOutput { get; }

        private TerrainFoodOutputType(int terrainId, float foodOutput)
        {
            Id = terrainId;
            FoodOutput = foodOutput;
        }

        public static TerrainFoodOutputType Create(int terrainId, float foodOutput)
        {
            return new TerrainFoodOutputType(terrainId, foodOutput);
        }

        private string DebuggerDisplay => $"{{Id={Id},FoodOutput={FoodOutput}}}";
    }

    public static class TerrainFoodOutputTypesLoader
    {
        public static DataList<TerrainFoodOutputType> Load()
        {
            var terrainFoodOutputTypes = new List<TerrainFoodOutputType>
            {
                TerrainFoodOutputType.Create(0, 1.5f),
                TerrainFoodOutputType.Create(1, 0.5f),
                TerrainFoodOutputType.Create(6, 0.5f)
            };

            return DataList<TerrainFoodOutputType>.Create(terrainFoodOutputTypes);
        }
    }
}