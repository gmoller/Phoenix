using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGameLibrary
{
    public class OverlandMap
    {
        private HexGrid _hexGrid;

        public OverlandMap()
        {
            _hexGrid = new HexGrid(Constants.WORLD_MAP_WIDTH_IN_HEXES, Constants.WORLD_MAP_HEIGHT_IN_HEXES);
        }

        public void Update(GameTime gameTime)
        {
            _hexGrid.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _hexGrid.Draw(spriteBatch);
        }
    }
}