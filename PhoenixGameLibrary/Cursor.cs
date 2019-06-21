using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;

namespace PhoenixGameLibrary
{
    public class Cursor
    {
        private Texture2D _texture;
        private Vector2 _cursorPos;

        public void LoadContent()
        {
            _texture = AssetsManager.Instance.GetTexture("cursor");
        }

        public void Update(GameTime gameTime)
        {
            _cursorPos = new Vector2(MouseHandler.MousePosition.X, MouseHandler.MousePosition.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _cursorPos, Color.White);
            spriteBatch.End();
        }
    }
}