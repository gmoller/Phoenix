using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGamePresentation.Events;
using PhoenixGamePresentation.Views.SettlementViewComposite;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class SettlementView : IDisposable
    {
        #region State
        private WorldView WorldView { get; } // readonly 

        internal Settlement Settlement { get; set; }

        private IControl MainFrame { get; } // readonly 
        private IControl SecondaryFrame { get; }// readonly 

        private Viewport Viewport { get; set; }
        private ViewportAdapter ViewportAdapter { get; set; }

        private InputHandler Input { get; } // readonly
        private bool IsDisposed { get; set; }
        #endregion End State

        internal SettlementView(WorldView worldView, Settlement settlement, InputHandler input)
        {
            WorldView = worldView;
            Settlement = settlement;

            var topLeftPositionMain = new Vector2(1920.0f * 0.01f, 200.0f);
            var topLeftPositionSecondary = new Vector2(1920.0f * 0.58f, 200.0f);
            MainFrame = new MainFrame(this, topLeftPositionMain, "GUI_Textures_1");
            SecondaryFrame = new SecondaryFrame(this, topLeftPositionSecondary, "GUI_Textures_1");

            SetupViewport(0, 0, 1920, 1080);

            Settlement.SettlementOpened += SettlementOpened;
            Input = input;
        }

        private void SettlementOpened(object sender, EventArgs e)
        {
            OpenSettlementEvent.HandleEvent(sender, new MouseEventArgs(null, WorldView, 0.0f));
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            Viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            ViewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
        }

        internal void LoadContent(ContentManager content)
        {
            MainFrame.LoadContent(content);
            SecondaryFrame.LoadContent(content);
        }

        internal void Update(float deltaTime, Viewport? viewport)
        {
            if (WorldView.GameStatus != GameStatus.CityView) return;

            MainFrame.Update(Input, deltaTime, viewport);
            SecondaryFrame.Update(Input, deltaTime, viewport);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = Viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: ViewportAdapter.GetScaleMatrix());

            if (Settlement != null)
            {
                MainFrame.Draw(spriteBatch);
                SecondaryFrame.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        #region Event Handlers

        internal void CloseButtonClick(object sender, EventArgs e)
        {
            Command closeSettlementCommand = new CloseSettlementCommand { Payload = (Settlement, WorldView.Settlements) };
            closeSettlementCommand.Execute();
            WorldView.ChangeState(GameStatus.CityView, GameStatus.OverlandMap);
        }

        #endregion

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Name={Settlement.Name}}}";

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)
                Input.UnsubscribeAllFromEventHandler("SettlementView");

                // set large fields to null
                ViewportAdapter = null;

                IsDisposed = true;
            }
        }
    }
}