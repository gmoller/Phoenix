using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        private readonly OverlandMap _overlandMap;
        private readonly Cursor _cursor;

        public PhoenixGame()
        {
            _overlandMap = new OverlandMap();
            _cursor = new Cursor();
        }

        public void LoadContent()
        {
            _cursor.LoadContent();
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _overlandMap.Update(gameTime, input);
            _cursor.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _overlandMap.Draw(spriteBatch);
            _cursor.Draw(spriteBatch);
        }
    }
}