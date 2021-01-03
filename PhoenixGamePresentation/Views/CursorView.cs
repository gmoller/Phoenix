using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zen.GuiControls;
using Zen.GuiControls.TheControls;
using Zen.Input;
using Zen.MonoGameUtilities.ExtensionMethods;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views
{
    internal class CursorView : ViewBase, IDisposable
    {
        #region State
        private Image CursorImage { get; }
        #endregion

        internal CursorView(WorldView worldView, InputHandler input)
        {
            WorldView = worldView;

            CursorImage = new Image("Cursor")
            {
                TextureNormal = "Cursor",
                PositionAlignment = Alignment.TopLeft,
                Size = new PointI(28, 32)
            };
            SetPosition(Mouse.GetState().Position.ToPointI());

            SetupViewport(0, 0, 1920, 1080);

            Input = input;
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
            spriteBatch.GraphicsDevice.Viewport = Viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: ViewportAdapter.GetScaleMatrix());
            CursorImage.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        internal void SetPosition(PointI pointI)
        {
            CursorImage.Position = pointI;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)

                // set large fields to null
                ViewportAdapter = null;

                IsDisposed = true;
            }
        }
    }
}