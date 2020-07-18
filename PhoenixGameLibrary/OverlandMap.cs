namespace PhoenixGameLibrary
{
    public class OverlandMap
    {
        private readonly World _world;

        public CellGrid CellGrid { get; }

        internal OverlandMap(World world)
        {
            _world = world;
            CellGrid = new CellGrid(Constants.WORLD_MAP_COLUMNS, Constants.WORLD_MAP_ROWS);
        }
    }
}