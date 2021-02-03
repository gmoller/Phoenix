using PhoenixGameData.StrongTypes;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public readonly struct CellRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key: also used to determine x and y (Id = row * Constants.WORLD_MAP_COLUMNS + col, Column (X) = Id % Constants.WORLD_MAP_COLUMNS, Row (Y) = Id / Constants.WORLD_MAP_COLUMNS)
        public TerrainTypeId TerrainTypeId { get; } // Foreign key -> GameMetadata.TerrainType

        public CellRecord(int id, int terrainTypeId) // for creation
        {
            Id = id;
            TerrainTypeId = new TerrainTypeId(terrainTypeId);
        }
    }

    internal class CellCollection : DataList<CellRecord>
    {
    }
}