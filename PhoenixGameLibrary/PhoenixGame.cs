using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        private OverlandMap _overlandMap;

        public PhoenixGame()
        {
            _overlandMap = new OverlandMap();
        }

        public void Update(GameTime gameTime)
        {
            _overlandMap.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _overlandMap.Draw(spriteBatch);
        }
    }
}