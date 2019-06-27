using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;

namespace PhoenixGameLibrary
{
    public class Cursor
    {
        private Texture2D _texture;
        private Vector2 _cursorPos;

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("Cursor");
        }

        public void Update(GameTime gameTime)
        {
            _cursorPos = new Vector2(MouseHandler.MousePosition.X, MouseHandler.MousePosition.Y);
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _cursorPos, Color.White);
            spriteBatch.End();
        }
    }
}