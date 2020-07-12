using System.Collections.Generic;
using HexLibrary;

namespace PhoenixGameLibrary
{
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
            for (int r = 0; r < numberOfRows; ++r)
            {
                for (int q = 0; q < numberOfColumns; ++q)
                {
                    _cellGrid[q, r] = new Cell(q, r, map[q, r]);
                }
            }
        }

        public Cell GetCell(int column, int row)
        {
            return _cellGrid[column, row];
        }

        public void SetCell(int column, int row, Cell cell)
        {
            _cellGrid[column, row] = cell;
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
    }
}