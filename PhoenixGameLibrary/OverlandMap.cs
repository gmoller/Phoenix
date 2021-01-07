namespace PhoenixGameLibrary
{
    public class OverlandMap
    {
        public CellGrid CellGrid { get; }

        internal OverlandMap(int numberOfColumns, int numberOfRows)
        {
            CellGrid = new CellGrid(numberOfColumns, numberOfRows);
        }
    }
}