using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;

namespace PhoenixGamePresentationLibrary
{
    internal class CursorView
    {
        private readonly Cursor _cursor;

        internal CursorView(Cursor cursor)
        {
            _cursor = cursor;
        }

        internal void LoadContent(ContentManager content)
        {
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var imgCursor = new Image("imgCursor", _cursor.Position, new Vector2(28.0f, 32.0f), "Cursor");
            spriteBatch.Begin();
            imgCursor.Draw();
            spriteBatch.End();
        }
    }
}