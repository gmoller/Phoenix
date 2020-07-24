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
        public Texture TextureFogOfWar { get; }
        public int BelongsToSettlement { get; set; }
        public SeenState SeenState { get; set; }

        public bool IsSeenByPlayer(World world)
        {
            foreach (var settlement in world.Settlements)
            {
                if (settlement.CanSeeCell(this))
                {
                    return true;
                }
            }

            foreach (var unit in world.Units)
            {
                if (unit.CanSeeCell(this))
                {
                    return true;
                }
            }

            return false;
        }

        public Cell(int col, int row, TerrainType terrainType)
        {
            Index = (row * Constants.WORLD_MAP_COLUMNS) + col;
            TerrainTypeId = terrainType.Id;
            Texture = terrainType.PossibleTextures[RandomNumberGenerator.Instance.GetRandomInt(0, 3)];
            TextureFogOfWar = new Texture("terrain_hextiles_basic_1", (byte)RandomNumberGenerator.Instance.GetRandomInt(36, 39)); //28-31,36-39
            BelongsToSettlement = -1;
            SeenState = SeenState.Never;
        }

        public Point ToPoint => new Point(Column, Row);

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Col={Column},Row={Row},TerrainTypeId={TerrainTypeId}}}";
    }

    public enum SeenState
    {
        Never,
        Current,
        HasBeen
    }
}