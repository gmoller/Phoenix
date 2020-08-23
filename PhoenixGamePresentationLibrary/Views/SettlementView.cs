using System;
using System.Diagnostics;
using GuiControls;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGamePresentationLibrary.SettlementViewComposite;

namespace PhoenixGamePresentationLibrary.Views
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class SettlementView
    {
        #region State
        private readonly WorldView _worldView;

        internal Settlement Settlement { get; set; }

        private readonly IControl _mainFrame;
        private readonly IControl _secondaryFrame;
        #endregion

        internal SettlementView(WorldView worldView, Settlement settlement)
        {
            _worldView = worldView;
            Settlement = settlement;

            var topLeftPositionMain = new Vector2(1920.0f * 0.05f, 200.0f);
            var topLeftPositionSecondary = new Vector2(1920.0f * 0.65f, 200.0f);
            _mainFrame = new MainFrame(this, topLeftPositionMain, "GUI_Textures_1");
            _secondaryFrame = new SecondaryFrame(this, topLeftPositionSecondary, "GUI_Textures_1");
        }

        internal void LoadContent(ContentManager content)
        {
            _mainFrame.LoadContent(content);
            _secondaryFrame.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            if (_worldView.GameStatus != GameStatus.CityView) return;

            // Causes

            // Actions

            _mainFrame.Update(input, deltaTime);
            _secondaryFrame.Update(input, deltaTime);

            // Status change?
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _mainFrame.Draw(spriteBatch);
            _secondaryFrame.Draw(spriteBatch);
        }

        internal void CloseButtonClick(object sender, EventArgs e)
        {
            Command closeSettlementCommand = new CloseSettlementCommand { Payload = (Settlement, _worldView.World.Settlements) };
            closeSettlementCommand.Execute();
            _worldView.GameStatus = GameStatus.OverlandMap;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Name={Settlement.Name}}}";
    }
}