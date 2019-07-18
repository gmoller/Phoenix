using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using PhoenixGameLibrary.Views;

namespace PhoenixGamePresentationLibrary
{
    internal class CursorView
    {
        private Cursor _cursor;
        private Texture2D _texture;
        private Vector2 _origin;

        internal CursorView(Cursor cursor)
        {
            _cursor = cursor;
        }

        internal void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("Cursor");
            _origin = new Vector2(3.0f, 1.0f);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _cursor.Position, null, Color.White, 0.0f, _origin, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }
    }
}