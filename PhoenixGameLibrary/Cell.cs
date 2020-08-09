using System.Diagnostics;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct Cell
    {
        public static readonly Cell Empty = new Cell(0, 0, TerrainType.Invalid.Id);

        public int Index { get; }
        public int TerrainTypeId { get; }
        public Texture Texture { get; }
        public Texture TextureFogOfWar { get; }
        public SeenState SeenState { get; }

        public int Column => Index % Constants.WORLD_MAP_COLUMNS;
        public int Row => Index / Constants.WORLD_MAP_COLUMNS;
        public Point ToPoint => new Point(Column, Row);

        public Cell(int col, int row, int terrainTypeId)
        {
            Index = row * Constants.WORLD_MAP_COLUMNS + col;

            TerrainTypeId = terrainTypeId;
            if (terrainTypeId == TerrainType.Invalid.Id)
            {
                Texture = new Texture("terrain_hextiles_basic_1", 36);
            }
            else
            {
                var terrainType = Globals.Instance.TerrainTypes[terrainTypeId];
                Texture = terrainType.PossibleTextures[RandomNumberGenerator.Instance.GetRandomInt(0, 3)];
            }
            
            TextureFogOfWar = new Texture("terrain_hextiles_basic_1", (byte)RandomNumberGenerator.Instance.GetRandomInt(36, 39)); //28-31,36-39
            SeenState = SeenState.NeverSeen;
        }

        public Cell(Cell cell, SeenState seenState)
        {
            Index = cell.Index;
            TerrainTypeId = cell.TerrainTypeId;
            Texture = cell.Texture;
            TextureFogOfWar = cell.TextureFogOfWar;
            SeenState = seenState;
        }

        public bool IsSeenByPlayer(World world)
        {
            foreach (var settlement in world.Settlements)
            {
                if (settlement.CanSeeCell(this))
                {
                    return true;
                }
            }

            foreach (var stacks in world.Stacks)
            {
                if (stacks.CanSeeCell(this))
                {
                    return true;
                }
            }

            return false;
        }

        #region Overrides and Overloads

        public override bool Equals(object obj)
        {
            return obj is Cell cell && this == cell;
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ TerrainTypeId.GetHashCode() ^ Texture.GetHashCode() ^ TextureFogOfWar.GetHashCode() ^ SeenState.GetHashCode();
        }

        public static bool operator == (Cell a, Cell b)
        {
            return a.Index == b.Index && a.TerrainTypeId == b.TerrainTypeId && a.Texture == b.Texture && a.TextureFogOfWar == b.TextureFogOfWar && a.SeenState == b.SeenState;
        }

        public static bool operator !=(Cell a, Cell b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Col={Column},Row={Row},TerrainTypeId={TerrainTypeId}}}";

        #endregion
    }
}