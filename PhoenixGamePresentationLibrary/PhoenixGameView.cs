using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    public class PhoenixGameView
    {
        private readonly WorldView _worldView;
        private readonly CursorView _cursorView;
        private readonly Cursor _cursor;
        private readonly InputHandler _input;

        public PhoenixGameView(PhoenixGame phoenixGame)
        {
            _worldView = new WorldView(phoenixGame.World);
            _cursor = new Cursor();
            _cursorView = new CursorView(_cursor);

            _input = new InputHandler();
            _input.Initialize();
        }

        public void LoadContent(ContentManager content)
        {
            _worldView.LoadContent(content);
            _cursorView.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            _input.Update(gameTime);
            _worldView.Update(gameTime, _input);
            _cursor.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _worldView.Draw(spriteBatch);
            _cursorView.Draw(spriteBatch);
        }
    }
}