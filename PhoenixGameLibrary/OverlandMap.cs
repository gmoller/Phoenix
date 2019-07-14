using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

        public void LoadContent(ContentManager content)
        {
            _overlandMapView.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _overlandMapView.Update(gameTime, input);
        }

        public void Draw()
        {
            _overlandMapView.Draw();
        }

        public void EndTurn()
        {
            _world.EndTurn();
        }
    }
}