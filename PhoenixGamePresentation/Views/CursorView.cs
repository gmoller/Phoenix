using System;
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
    internal class CursorView : IDisposable
    {
        #region State
        private PointI _position;

        private readonly Image _imgCursor;

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;

        private readonly InputHandler _input;
        private bool _disposedValue;
        #endregion End State

        internal CursorView(InputHandler input)
        {
            _imgCursor = new Image(_position.ToVector2(), Alignment.TopLeft, new Vector2(28.0f, 32.0f), "Cursor");

            SetupViewport(0, 0, 1920, 1080);

            input.AddCommandHandler("CursorView", 0, new MouseInputAction(MouseInputActionType.Moved, UpdatePosition));
            _input = input;
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

        internal void Update(float deltaTime)
        {
            _position = new PointI(_input.MousePosition.X, _input.MousePosition.Y);
            _imgCursor.SetTopLeftPosition(_position);
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

        #region Event Handlers

        private void UpdatePosition(object sender, EventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs is null) return;

            _position = new PointI(mouseEventArgs.X, mouseEventArgs.Y);
            _imgCursor.SetTopLeftPosition(_position);
        }

        #endregion

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // TODO: dispose managed state (managed objects)
                _input.RemoveCommandHandler("CursorView", 0, new MouseInputAction(MouseInputActionType.Moved, UpdatePosition));

                // TODO: set large fields to null
                _viewportAdapter = null;

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}