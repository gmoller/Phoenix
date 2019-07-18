using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    public class PhoenixGameView
    {
        private readonly WorldView _world;
        private readonly CursorView _cursor;

        public PhoenixGameView(PhoenixGame phoenixGame)
        {
            _world = new WorldView(phoenixGame.World);
            _cursor = new CursorView(phoenixGame.Cursor);
        }

        public void LoadContent(ContentManager content)
        {
            _world.LoadContent(content);
            _cursor.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _world.Draw(spriteBatch);
            _cursor.Draw(spriteBatch);
        }
    }
}