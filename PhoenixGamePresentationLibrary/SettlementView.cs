﻿using System;
using System.Diagnostics;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGamePresentationLibrary.SettlementViewComposite;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class SettlementView
    {
        #region State
        private readonly WorldView _worldView;

        private readonly MainFrame _mainFrame;
        private readonly SecondaryFrame _secondaryFrame;

        private BuildingsFrame _buildingsFrame;
        private UnitsFrame _unitsFrame;
        private OtherFrame _otherFrame;

        private readonly Vector2 _topLeftPositionSecondary;

        internal Settlement Settlement { get; set; }
        #endregion

        internal SettlementView(WorldView worldView, Settlement settlement)
        {
            _worldView = worldView;
            Settlement = settlement;

            var topLeftPositionMain = new Vector2(1920.0f * 0.05f, 200.0f);
            _topLeftPositionSecondary = new Vector2(1920.0f * 0.65f, 200.0f);
            _mainFrame = new MainFrame(this, topLeftPositionMain, "GUI_Textures_1");
            _secondaryFrame = new SecondaryFrame(this, _topLeftPositionSecondary, "GUI_Textures_1");
        }

        internal void LoadContent(ContentManager content)
        {
            _mainFrame.LoadContent(content);
            _secondaryFrame.LoadContent(content);

            _buildingsFrame = new BuildingsFrame(this, new Vector2(_topLeftPositionSecondary.X + 20.0f, _topLeftPositionSecondary.Y + 40.0f));
            _buildingsFrame.LoadContent(content);
            _unitsFrame = new UnitsFrame(this, new Vector2(_topLeftPositionSecondary.X + 20.0f, _topLeftPositionSecondary.Y + 40.0f + 495.0f));
            _unitsFrame.LoadContent(content);
            _otherFrame = new OtherFrame(this, new Vector2(_topLeftPositionSecondary.X + 20.0f, _topLeftPositionSecondary.Y + 40.0f + 600.0f));
            _otherFrame.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _mainFrame.Update(input, deltaTime);

            _secondaryFrame.Update(input, deltaTime);
            _buildingsFrame.Update(input, deltaTime);
            _unitsFrame.Update(input, deltaTime);
            _otherFrame.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _mainFrame.Draw(spriteBatch);

            _secondaryFrame.Draw(spriteBatch);
            _buildingsFrame.Draw(spriteBatch);
            _unitsFrame.Draw(spriteBatch);
            _otherFrame.Draw(spriteBatch);
        }

        internal void CloseButtonClick(object sender, EventArgs e)
        {
            Command closeSettlementCommand = new CloseSettlementCommand { Payload = (Settlement, _worldView.World.Settlements) };
            closeSettlementCommand.Execute();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Name={Settlement.Name}}}";
    }
}