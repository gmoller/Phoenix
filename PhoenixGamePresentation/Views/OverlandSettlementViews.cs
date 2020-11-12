using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using Zen.Input;

namespace PhoenixGamePresentation.Views
{
    internal class OverlandSettlementViews : ViewBase, IDisposable
    {
        #region State
        private Settlements Settlements { get; }

        private OverlandSettlementView OverlandSettlementView { get; }
        #endregion End State

        public OverlandSettlementViews(WorldView worldView, Settlements settlements, InputHandler input)
        {
            WorldView = worldView;
            Settlements = settlements;

            OverlandSettlementView = new OverlandSettlementView(WorldView, input);

            SetupViewport(0, 0, WorldView.Camera.GetViewport.Width, WorldView.Camera.GetViewport.Height);

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), "OverlandSettlementViews");
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), "OverlandSettlementViews");

            WorldView.SubscribeToStatusChanges("OverlandSettlementViews", worldView.HandleStatusChange);
        }

        public void LoadContent(ContentManager content)
        {
            OverlandSettlementView.LoadContent(content);
        }

        public void Update(float deltaTime)
        {
            if (WorldView.GameStatus != GameStatus.OverlandMap) return;

            foreach (var settlement in Settlements)
            {
                OverlandSettlementView.Settlement = settlement;
                OverlandSettlementView.Update(deltaTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = Viewport;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform * ViewportAdapter.GetScaleMatrix()); // FrontToBack

            foreach (var settlement in Settlements)
            {
                OverlandSettlementView.Settlement = settlement;
                OverlandSettlementView.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)
                Input.UnsubscribeAllFromEventHandler("OverlandSettlementViews");
                OverlandSettlementView.Dispose();

                // set large fields to null
                ViewportAdapter = null;

                IsDisposed = true;
            }
        }
    }
}