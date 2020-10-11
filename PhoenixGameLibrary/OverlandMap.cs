namespace PhoenixGameLibrary
{
    public class OverlandMap
    {
        private readonly World _world;

        public CellGrid CellGrid { get; }

        internal OverlandMap(World world, int numberOfColumns, int numberOfRows)
        {
            _world = world;
            CellGrid = new CellGrid(_world, numberOfColumns, numberOfRows);
        }
    }
}