using System.Diagnostics;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Cell
    {
        private readonly int _index;

        public int Column => _index % Constants.WORLD_MAP_COLUMNS;
        public int Row => _index / Constants.WORLD_MAP_COLUMNS;
        public int TerrainTypeId { get; }
        public Texture Texture { get; }

        public Cell(int col, int row, TerrainType terrainType)
        {
            _index = (row * Constants.WORLD_MAP_COLUMNS) + col;
            TerrainTypeId = terrainType.Id;
            Texture = terrainType.PossibleTextures[RandomNumberGenerator.Instance.GetRandomInt(0, 3)];
        }

        private string DebuggerDisplay => $"{{Col={Column},Row={Row},TerrainTypeId={TerrainTypeId}}}";
    }
}