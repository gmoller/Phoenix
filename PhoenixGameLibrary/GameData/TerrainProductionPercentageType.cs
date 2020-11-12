﻿using System.Collections.Generic;
using System.Diagnostics;
using Zen.Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct TerrainProductionPercentageType : IIdentifiedById
    {
        #region State
        public int Id { get; }
        public float ProductionPercentage { get; }
        #endregion

        private TerrainProductionPercentageType(int terrainId, float productionPercentage)
        {
            Id = terrainId;
            ProductionPercentage = productionPercentage;
        }

        public static TerrainProductionPercentageType Create(int terrainId, float productionPercentage)
        {
            return new TerrainProductionPercentageType(terrainId, productionPercentage);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},ProductionPercentage={ProductionPercentage}}}";
    }

    public static class TerrainProductionPercentageTypesLoader
    {
        public static DataDictionary<TerrainProductionPercentageType> Load()
        {
            var terrainProductionPercentageTypes = new List<TerrainProductionPercentageType>
            {
                TerrainProductionPercentageType.Create(1, 3.0f),
                TerrainProductionPercentageType.Create(2, 3.0f),
                TerrainProductionPercentageType.Create(6, 3.0f),
                TerrainProductionPercentageType.Create(7, 5.0f)
            };

            return DataDictionary<TerrainProductionPercentageType>.Create(terrainProductionPercentageTypes);
        }
    }
}