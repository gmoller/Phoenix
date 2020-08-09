﻿using System.Collections.Generic;
using System.Diagnostics;
using HexLibrary;
using Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// This class is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class CellGrid
    {
        private readonly Cell[,] _cellGrid;

        public int NumberOfColumns { get; }
        public int NumberOfRows { get; }

        public CellGrid(int numberOfColumns, int numberOfRows)
        {
            var map = MapGenerator.Generate(numberOfColumns, numberOfRows);

            NumberOfColumns = numberOfColumns;
            NumberOfRows = numberOfRows;
            _cellGrid = new Cell[numberOfColumns, numberOfRows];
            for (var r = 0; r < numberOfRows; ++r)
            {
                for (var q = 0; q < numberOfColumns; ++q)
                {
                    var terrainType = map[q, r];
                    var cell = new Cell(q, r, terrainType.Id);
                    _cellGrid[q, r] = cell;
                }
            }
        }

        public Cell GetCell(Point location)
        {
            return GetCell(location.X, location.Y);
        }

        public Cell GetCell(int column, int row)
        {
            return IsWithinBounds(column, row) ? _cellGrid[column, row] : Cell.Empty;
        }

        private bool IsWithinBounds(int column, int row)
        {
            return column >= 0 && column < NumberOfColumns &&
                   row >= 0 && row < NumberOfRows;
        }

        public void SetCell(Cell cell, SeenState seenState)
        {
            if (cell.Equals(Cell.Empty)) return;

            var newCell = new Cell(cell, seenState);
            _cellGrid[cell.Column, cell.Row] = newCell;
        }

        public List<Cell> GetCatchment(int column, int row, int radius)
        {
            var catchmentCells = new List<Cell>();
            var catchment = HexOffsetCoordinates.GetSpiralRing(column, row, radius);
            foreach (var tile in catchment)
            {
                if (tile.Col >= 0 && tile.Row >= 0 && tile.Col < Constants.WORLD_MAP_COLUMNS && tile.Row < Constants.WORLD_MAP_ROWS)
                {
                    var cell = GetCell(tile.Col, tile.Row);
                    catchmentCells.Add(cell);
                }
            }

            return catchmentCells;
        }

        public Cell GetClosestUnexploredCell(Point location)
        {
            for (int i = 1; i < 11; i++)
            {
                var ring = HexOffsetCoordinates.GetSingleRing(location.X, location.Y, i);
                foreach (var coordinates in ring)
                {
                    var cell = GetCell(coordinates.Col, coordinates.Row);
                    if (!cell.Equals(Cell.Empty) && cell.SeenState == SeenState.NeverSeen)
                    {
                        return cell;
                    }
                }
            }

            return Cell.Empty;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{NumberOfColumns={NumberOfColumns},NumberOfRows={NumberOfRows}}}";
    }
}