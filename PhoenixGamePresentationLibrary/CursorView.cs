using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;
using Utilities.ViewportAdapters;

namespace PhoenixGamePresentationLibrary
{
    internal class CursorView
    {
        private readonly Cursor _cursor;
        private Image _imgCursor;

        internal CursorView(Cursor cursor)
        {
            _cursor = cursor;
        }

        internal void LoadContent(ContentManager content)
        {
            _imgCursor = new Image(_cursor.Position, Alignment.TopLeft, new Vector2(28.0f, 32.0f), "Cursor");
            _imgCursor.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _imgCursor.SetTopLeftPosition((int)_cursor.Position.X, (int)_cursor.Position.Y);
        }

        internal void Draw(SpriteBatch spriteBatch, ViewportAdapter viewportAdapter)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: viewportAdapter.GetScaleMatrix());
            _imgCursor.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}