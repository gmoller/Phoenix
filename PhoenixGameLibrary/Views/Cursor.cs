using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;

namespace PhoenixGameLibrary.Views
{
    public class Cursor
    {
        private Texture2D _texture;
        public Vector2 Position { get; private set; }
        private Vector2 _origin;

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("Cursor");
            _origin = new Vector2(3.0f, 1.0f);
        }

        public void Update(GameTime gameTime)
        {
            Position = new Vector2(MouseHandler.MousePosition.X, MouseHandler.MousePosition.Y);
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            spriteBatch.Begin();
            spriteBatch.Draw(_texture, Position, null, Color.White, 0.0f, _origin, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }
    }
}