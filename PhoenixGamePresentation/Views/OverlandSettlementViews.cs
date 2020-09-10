using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    public class OverlandSettlementViews : IDisposable
    {
        #region State
        private WorldView WorldView { get; }// readonly
        private Settlements Settlements { get; } // readonly

        private OverlandSettlementView OverlandSettlementView { get; } // readonly

        private Viewport Viewport { get; set; }
        private ViewportAdapter ViewportAdapter { get; set; }

        private InputHandler Input { get; } // readonly
        private bool IsDisposed { get; set; }
        #endregion End State

        public OverlandSettlementViews(WorldView worldView, Settlements settlements, InputHandler input)
        {
            WorldView = worldView;
            Settlements = settlements;

            OverlandSettlementView = new OverlandSettlementView(WorldView, input);

            SetupViewport(0, 0, WorldView.Camera.GetViewport.Width, WorldView.Camera.GetViewport.Height);

            Input = input;
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            Viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            ViewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
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