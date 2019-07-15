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

        public Cell GetCell(int col, int row)
        {
            return _cellGrid[col, row];
        }

        public void SetCell(int col, int row, Cell cell)
        {
            _cellGrid[col, row] = cell;
        }
    }
}