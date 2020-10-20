namespace PhoenixGameLibrary
{
    public class OverlandMap
    {
        public CellGrid CellGrid { get; }

        internal OverlandMap(World world, int numberOfColumns, int numberOfRows)
        {
            CellGrid = new CellGrid(world, numberOfColumns, numberOfRows);
        }
    }
}