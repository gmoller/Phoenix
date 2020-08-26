using System.Collections.Generic;
using System.Diagnostics;
using Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct TerrainCanSettleOnType : IIdentifiedById
    {
        #region State
        public int Id { get; }
        #endregion

        private TerrainCanSettleOnType(int terrainId)
        {
            Id = terrainId;
        }

        public static TerrainCanSettleOnType Create(int terrainId)
        {
            return new TerrainCanSettleOnType(terrainId);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id}}}";
    }

    public static class TerrainCanSettleOnTypesLoader
    {
        public static DataDictionary<TerrainCanSettleOnType> Load()
        {
            var terrainCanSettleOnTypes = new List<TerrainCanSettleOnType>
            {
                TerrainCanSettleOnType.Create(0),
                TerrainCanSettleOnType.Create(1),
                TerrainCanSettleOnType.Create(2),
                TerrainCanSettleOnType.Create(3),
                TerrainCanSettleOnType.Create(6),
                TerrainCanSettleOnType.Create(7),
                TerrainCanSettleOnType.Create(9)
            };

            return DataDictionary<TerrainCanSettleOnType>.Create(terrainCanSettleOnTypes);
        }
    }
}