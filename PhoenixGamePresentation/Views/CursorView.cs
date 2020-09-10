using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Image CursorImage { get; } // readonly
        private Viewport Viewport { get; set; }
        private ViewportAdapter ViewportAdapter { get; set; }

        private InputHandler Input { get; } // readonly
        private bool IsDisposed { get; set; }
        #endregion End State

        internal CursorView(WorldView worldView, InputHandler input)
        {
            CursorImage = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(28.0f, 32.0f), "Cursor");
            SetPosition(Mouse.GetState().Position.ToPointI());

            SetupViewport(0, 0, 1920, 1080);

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), "CursorView");
            Input.Register(0, this, MouseInputActionType.Moved, UpdateCursorPositionEvent.HandleEvent);
            input.Register(0, this, Keys.F1, KeyboardInputActionType.Released, (sender, e) => { Input.SetMousePosition(new Point(840, 540)); }); // for testing
            Input.EndRegistration();
            Input.BeginRegistration(GameStatus.CityView.ToString(), "CursorView");
            Input.Register(0, this, MouseInputActionType.Moved, UpdateCursorPositionEvent.HandleEvent);
            input.Register(0, this, Keys.F1, KeyboardInputActionType.Released, (sender, e) => { Input.SetMousePosition(new Point(840, 540)); }); // for testing
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), "CursorView");

            worldView.SubscribeToStatusChanges("CursorView", worldView.HandleStatusChange);
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            Viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            ViewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
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
            CursorImage.SetTopLeftPosition(pointI);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)
                Input.UnsubscribeAllFromEventHandler("CursorView");

                // set large fields to null
                ViewportAdapter = null;

                IsDisposed = true;
            }
        }
    }
}