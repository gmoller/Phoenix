using GuiControls;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUtilities.ExtensionMethods;
using MonoGameUtilities.ViewportAdapters;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    internal class CursorView
    {
        #region State
        private readonly Cursor _cursor;
        private readonly Image _imgCursor;

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;
        #endregion State

        internal CursorView(Cursor cursor)
        {
            _cursor = cursor;
            _imgCursor = new Image(_cursor.Position.ToVector2(), Alignment.TopLeft, new Vector2(28.0f, 32.0f), "Cursor");

            SetupViewport(0, 0, 1920, 1080);
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            _viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            _viewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
        }

        internal void LoadContent(ContentManager content)
        {
            _imgCursor.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _imgCursor.SetTopLeftPosition(_cursor.Position);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = _viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: _viewportAdapter.GetScaleMatrix());
            _imgCursor.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }
    }
}