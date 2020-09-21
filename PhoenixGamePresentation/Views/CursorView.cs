﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GuiControls;
using Input;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGamePresentation.Events;
using Utilities;

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

            CursorImage = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(28.0f, 32.0f), "Cursor");
            SetPosition(Mouse.GetState().Position.ToPointI());

            SetupViewport(0, 0, 1920, 1080);

            Input = input;
            Input.BeginRegistration(new List<string> { GameStatus.OverlandMap.ToString(), GameStatus.CityView.ToString() }, "CursorView");
            Input.Register(0, this, MouseInputActionType.Moved, UpdateCursorPositionEvent.HandleEvent);
            input.Register(1, this, Keys.F1, KeyboardInputActionType.Released, (sender, e) => { Input.SetMousePosition(new Point(840, 540)); }); // for testing
            Input.Register(2, this, MouseInputActionType.Moved, UpdateCursorPositionEvent.HandleEvent);
            input.Register(3, this, Keys.F1, KeyboardInputActionType.Released, (sender, e) => { Input.SetMousePosition(new Point(840, 540)); }); // for testing
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), "CursorView");

            WorldView.SubscribeToStatusChanges("CursorView", worldView.HandleStatusChange);
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