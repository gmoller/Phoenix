using System;
using System.Collections.Generic;
using System.Diagnostics;
using Utilities;
using Utilities.ExtensionMethods;
using Zen.Hexagons;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// This class is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class CellGrid
    {
        #region State
        private readonly Cell[,] _cellGrid;

        private World World { get; }
        public int NumberOfColumns { get; }
        public int NumberOfRows { get; }

        public event EventHandler NewCellSeen;
        #endregion

        public CellGrid(World world, int numberOfColumns, int numberOfRows)
        {
            World = world;
            var map = MapGenerator.Generate(numberOfColumns, numberOfRows);

            NumberOfColumns = numberOfColumns;
            NumberOfRows = numberOfRows;
            _cellGrid = new Cell[numberOfColumns, numberOfRows];
            for (var r = 0; r < numberOfRows; ++r)
            {
                for (var q = 0; q < numberOfColumns; ++q)
                {
                    var terrainType = map[q, r];
                    var cell = new Cell(World, q, r, terrainType.Id);
                    _cellGrid[q, r] = cell;
                }
            }
        }

        public Cell GetCell(HexOffsetCoordinates location)
        {
            return GetCell(location.Col, location.Row);
        }

        public Cell GetCell(PointI location)
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
            SetCell(cell, seenState, cell.ControlledByFaction);
        }

        public void SetCell(Cell cell, SeenState seenState, int controlledByFaction)
        {
            if (cell.Equals(Cell.Empty)) return;

            var newCell = new Cell(cell, seenState, controlledByFaction, cell.Borders);
            if (cell.SeenState != newCell.SeenState)
            {
                // raise event
                OnNewCellSeen(new EventArgs());
            }

            _cellGrid[cell.Column, cell.Row] = newCell;
        }

        private void OnNewCellSeen(EventArgs e)
        {
            NewCellSeen?.Invoke(this, e);
        }

        public void CellFactionChange(List<Cell> cells)
        {
            foreach (var cell in cells)
            {
                var borders = 0;
                for (var i = 0; i < 8; i++)
                {
                    var neighborCell = cell.GetNeighbor((Direction)i, this);
                    borders = cell.ControlledByFaction == neighborCell.ControlledByFaction || cell == neighborCell ? borders.ResetBit(i) : borders.SetBit(i);
                }

                var newCell = new Cell(cell, cell.SeenState, cell.ControlledByFaction, (byte)borders);
                _cellGrid[cell.Column, cell.Row] = newCell;
            }
        }

        public List<Cell> GetCatchment(int column, int row, int radius)
        {
            var catchmentCells = new List<Cell>();
            var catchment = World.HexLibrary.GetSpiralRing(new HexOffsetCoordinates(column, row), radius);
            foreach (var tile in catchment)
            {
                var cell = GetCell(tile.Col, tile.Row);

                if (cell != Cell.Empty)
                {
                    catchmentCells.Add(cell);
                }
            }

            return catchmentCells;
        }

        public Cell GetClosestUnexploredCell(PointI location)
        {
            var closestCells = new List<Cell>();
            var max = Math.Max(NumberOfColumns, NumberOfRows);
            for (int i = 1; i < max; i++)
            {
                var ring = World.HexLibrary.GetSingleRing(new HexOffsetCoordinates(location.X, location.Y), i);
                foreach (var coordinates in ring)
                {
                    var cell = GetCell(coordinates.Col, coordinates.Row);
                    if (!cell.Equals(Cell.Empty) && cell.SeenState == SeenState.NeverSeen)
                    {
                        closestCells.Add(cell);
                    }
                }

                if (closestCells.Count > 0)
                {
                    break;
                }
            }

            if (closestCells.Count == 0)
            {
                return Cell.Empty;
            }

            var random = RandomNumberGenerator.Instance.GetRandomInt(0, closestCells.Count - 1);

            return closestCells[random];
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{NumberOfColumns={NumberOfColumns},NumberOfRows={NumberOfRows}}}";
    }
}