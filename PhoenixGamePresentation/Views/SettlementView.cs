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
using PhoenixGamePresentation.Views.SettlementViewComposite;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class SettlementView
    {
        #region State
        private readonly WorldView _worldView;

        internal Settlement Settlement { get; set; }

        private readonly IControl _mainFrame;
        private readonly IControl _secondaryFrame;

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;
        #endregion

        internal SettlementView(WorldView worldView, Settlement settlement)
        {
            _worldView = worldView;
            Settlement = settlement;

            var topLeftPositionMain = new Vector2(1920.0f * 0.01f, 200.0f);
            var topLeftPositionSecondary = new Vector2(1920.0f * 0.58f, 200.0f);
            _mainFrame = new MainFrame(this, topLeftPositionMain, "GUI_Textures_1");
            _secondaryFrame = new SecondaryFrame(this, topLeftPositionSecondary, "GUI_Textures_1");

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
            _mainFrame.LoadContent(content);
            _secondaryFrame.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
            if (_worldView.GameStatus != GameStatus.CityView) return;

            _mainFrame.Update(input, deltaTime, viewport);
            _secondaryFrame.Update(input, deltaTime, viewport);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = _viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: _viewportAdapter.GetScaleMatrix());

            if (Settlement != null)
            {
                _mainFrame.Draw(spriteBatch);
                _secondaryFrame.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        #region Event Handlers

        internal void CloseButtonClick(object sender, EventArgs e)
        {
            Command closeSettlementCommand = new CloseSettlementCommand { Payload = (Settlement, _worldView.World.Settlements) };
            closeSettlementCommand.Execute();
            _worldView.GameStatus = GameStatus.OverlandMap;
        }

        #endregion

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Name={Settlement.Name}}}";
    }
}