using System.Diagnostics;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct Cell
    {
        public int Index { get; }
        public int Column => Index % Constants.WORLD_MAP_COLUMNS;
        public int Row => Index / Constants.WORLD_MAP_COLUMNS;
        public int TerrainTypeId { get; }
        public Texture Texture { get; }
        public int BelongsToSettlement { get; set; }
        public bool FogOfWar { get; set; }

        public Cell(int col, int row, TerrainType terrainType)
        {
            Index = (row * Constants.WORLD_MAP_COLUMNS) + col;
            TerrainTypeId = terrainType.Id;
            Texture = terrainType.PossibleTextures[RandomNumberGenerator.Instance.GetRandomInt(0, 3)];
            BelongsToSettlement = -1;
            FogOfWar = true;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Col={Column},Row={Row},TerrainTypeId={TerrainTypeId}}}";
    }
}