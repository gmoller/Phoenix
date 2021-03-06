﻿using System.Collections.Generic;
using System.Diagnostics;
using PhoenixGameConfig;
using PhoenixGameData;
using Zen.Hexagons;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct Cell
    {
        public static readonly Cell Empty = new Cell(-1, -1, -1);

        #region State
        public int Index { get; }
        public int TerrainId { get; }
        public Texture Texture { get; }
        public SeenState SeenState { get; }
        public int ControlledByFaction { get; }
        public byte Borders { get; }
        #endregion

        public int Column => Index % Constants.WORLD_MAP_COLUMNS;
        public int Row => Index / Constants.WORLD_MAP_COLUMNS;
        public PointI ToPoint => new PointI(Column, Row);

        public Cell(int col, int row, int terrainId)
        {
            Index = row * Constants.WORLD_MAP_COLUMNS + col;

            TerrainId = terrainId;
            if (terrainId == -1)
            {
                Texture = new Texture("terrain_hextiles_basic_1", 36);
            }
            else
            {
                var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");
                var terrainConfig = gameConfigCache.GetTerrainConfigById(terrainId);
                var chosenIndex = RandomNumberGenerator.Instance.GetRandomInt(0, terrainConfig.PossibleTexturesForThisTerrain.Count - 1);
                Texture = terrainConfig.PossibleTexturesForThisTerrain[chosenIndex];
            }

            SeenState = SeenState.NeverSeen;
            ControlledByFaction = 0; // None
            Borders = 0; // no borders
        }

        public Cell(Cell cell, SeenState seenState, int controlledByFaction, byte borders)
        {
            Index = cell.Index;
            TerrainId = cell.TerrainId;
            Texture = cell.Texture;
            SeenState = seenState;
            ControlledByFaction = controlledByFaction;
            Borders = borders;
        }

        public List<Cell> GetNeighbors(CellGrid cellGrid)
        {
            var world = CallContext<World>.GetData("GameWorld");
            var neighbors = world.HexLibrary.GetSingleRing(new HexOffsetCoordinates(Column, Row), 1);

            var returnCells = new List<Cell>();
            foreach (var neighbor in neighbors)
            {
                var cell = cellGrid.GetCell(neighbor.Col, neighbor.Row);

                if (cell != Empty)
                {
                    returnCells.Add(cell);
                }
            }

            return returnCells;
        }

        public Cell GetNeighbor(Direction direction, CellGrid cellGrid)
        {
            var world = CallContext<World>.GetData("GameWorld");
            var neighbor = world.HexLibrary.GetNeighbor(new HexOffsetCoordinates(Column, Row), direction);

            var cell = cellGrid.GetCell(neighbor.Col, neighbor.Row);

            return cell;
        }

        public bool IsSeenByPlayer(Settlements settlements, Stacks stacks)
        {
            foreach (var settlement in settlements)
            {
                if (settlement.CanSeeCell(this))
                {
                    return true;
                }
            }

            foreach (var stack in stacks)
            {
                if (stack.CanSeeCell(this))
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

        public static bool operator == (Cell a, Cell b)
        {
            return a.Index == b.Index && a.TerrainId == b.TerrainId && a.Texture == b.Texture && a.SeenState == b.SeenState && a.ControlledByFaction == b.ControlledByFaction;
        }

        public static bool operator !=(Cell a, Cell b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ TerrainId.GetHashCode() ^ Texture.GetHashCode() ^ SeenState.GetHashCode() ^ ControlledByFaction.GetHashCode();
        }

        public override string ToString() => DebuggerDisplay;

        private string DebuggerDisplay => $"{{Col={Column},Row={Row},TerrainTypeId={TerrainId}}}";

        #endregion
    }
}