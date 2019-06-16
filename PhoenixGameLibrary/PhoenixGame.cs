using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        private HexGrid _hexGrid;

        public PhoenixGame()
        {
            //_hex1 = new Hex(new Vector2(16.0f, 24.0f), 0);
            //_hex2 = new Hex(new Vector2(16.0f + 25.0f, 24.0f - 15.0f), 0); // 24.0f, 8.0f
            //_hex3 = new Hex(new Vector2(16.0f + 50.0f, 24.0f), 0);
            //_hex4 = new Hex(new Vector2(16.0f + 25.0f, 24.0f + 15.0f), 0); // 24.0f, 8.0f

            _hexGrid = new HexGrid(30, 15);
        }

        public void LoadContent()
        {
            _hexGrid.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _hexGrid.Draw(spriteBatch);
        }
    }
}