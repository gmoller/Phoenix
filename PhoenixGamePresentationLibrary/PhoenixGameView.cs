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
            _world = new WorldView();
            _cursor = new CursorView(phoenixGame.Cursor);
        }

        public void LoadContent(ContentManager content)
        {
            _world.LoadContent(content);
            _cursor.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _world.Draw();
            _cursor.Draw(spriteBatch);
        }
    }
}