using GuiControls;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUtilities.ExtensionMethods;
using MonoGameUtilities.ViewportAdapters;

namespace PhoenixGamePresentation.Views
{
    internal class CursorView
    {
        private readonly Cursor _cursor;
        private readonly Image _imgCursor;

        internal CursorView(Cursor cursor)
        {
            _cursor = cursor;
            _imgCursor = new Image(_cursor.Position.ToVector2(), Alignment.TopLeft, new Vector2(28.0f, 32.0f), "Cursor");
        }

        internal void LoadContent(ContentManager content)
        {
            _imgCursor.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _imgCursor.SetTopLeftPosition(_cursor.Position);
        }

        internal void Draw(SpriteBatch spriteBatch, ViewportAdapter viewportAdapter)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: viewportAdapter.GetScaleMatrix());
            _imgCursor.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}