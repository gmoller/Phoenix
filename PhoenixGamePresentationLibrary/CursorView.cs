using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;

namespace PhoenixGamePresentationLibrary
{
    internal class CursorView
    {
        private ContentManager _content;
        private readonly Cursor _cursor;

        internal CursorView(Cursor cursor)
        {
            _cursor = cursor;
        }

        internal void LoadContent(ContentManager content)
        {
            _content = content;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var imgCursor = new Image(_cursor.Position, Alignment.TopLeft,new Vector2(28.0f, 32.0f), "Cursor", 0.0f, null, "imgCursor");
            imgCursor.LoadContent(_content);
            spriteBatch.Begin();
            imgCursor.Draw();
            spriteBatch.End();
        }
    }
}