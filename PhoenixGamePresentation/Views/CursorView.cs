using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;
using MonoGameUtilities.ExtensionMethods;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGamePresentation.Events;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    internal class CursorView : IDisposable
    {
        #region State
        internal PointI Position { get; set; }

        internal Image CursorImage { get; }

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;

        private readonly InputHandler _input;
        private bool _disposedValue;
        #endregion End State

        internal CursorView(InputHandler input)
        {
            CursorImage = new Image(Position.ToVector2(), Alignment.TopLeft, new Vector2(28.0f, 32.0f), "Cursor");

            SetupViewport(0, 0, 1920, 1080);

            input.SubscribeToEventHandler("CursorView", 0, this, MouseInputActionType.Moved, UpdatePositionEvent.HandleEvent);
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
            CursorImage.LoadContent(content);
        }

        internal void Update(float deltaTime)
        {
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = _viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: _viewportAdapter.GetScaleMatrix());
            CursorImage.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // dispose managed state (managed objects)
                _input.UnsubscribeAllFromEventHandler("CursorView");

                // set large fields to null
                _viewportAdapter = null;

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}