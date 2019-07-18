using PhoenixGameLibrary.Views;

namespace PhoenixGameLibrary
{
    public class OverlandMap
    {
        private readonly World _world;
        private readonly OverlandMapView _overlandMapView;

        public CellGrid CellGrid { get; }

        public OverlandMap(World world)
        {
            _world = world;
            CellGrid = new CellGrid(Constants.WORLD_MAP_COLUMNS, Constants.WORLD_MAP_ROWS);
            _overlandMapView = new OverlandMapView(this);
        }

        public void Draw()
        {
            _overlandMapView.Draw();
        }
    }
}