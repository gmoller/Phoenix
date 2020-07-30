using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using Utilities;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class SettlementView
    {
        private Texture2D _guiTextures;
        private AtlasSpec2 _guiAtlas;

        private MainFrame _mainFrame;
        private SecondaryFrame _secondaryFrame;
        private LabelAutoSized _lblSettlementName1;
        private LabelAutoSized _lblSettlementName2;
        private PopulationFrame _populationFrame;
        private ResourceFrame _resourceFrame;
        private BuildingsFrame _buildingsFrame;
        private UnitsFrame _unitsFrame;
        private OtherFrame _otherFrame;
        private ProducingFrame _producingFrame;

        private Vector2 _topLeftPositionMain;
        private Vector2 _topLeftPositionSecondary;

        internal Settlement Settlement { get; set; }

        internal SettlementView(Settlement settlement)
        {
            Settlement = settlement;
        }

        internal void LoadContent(ContentManager content)
        {
            _topLeftPositionMain = new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.05f, 200.0f);
            _topLeftPositionSecondary = new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.65f, 200.0f);

            _guiTextures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            _guiAtlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");

            _mainFrame = new MainFrame(this, _topLeftPositionMain, _guiTextures, _guiAtlas);
            _mainFrame.LoadContent(content);
            _secondaryFrame = new SecondaryFrame(this, _topLeftPositionSecondary, _guiTextures, _guiAtlas);
            _secondaryFrame.LoadContent(content);

            _lblSettlementName1 = new LabelAutoSized("_lblSettlementName1", new Vector2(_topLeftPositionMain.X + 278.0f, _topLeftPositionMain.Y - 49.0f), Alignment.MiddleCenter, string.Empty, "Carolingia-Regular-24", Color.Purple, Color.DarkBlue);
            _lblSettlementName1.LoadContent(content);
            _lblSettlementName2 = new LabelAutoSized("_lblSettlementName2", new Vector2(_topLeftPositionMain.X + 278.0f, _topLeftPositionMain.Y - 24.0f), Alignment.MiddleCenter, string.Empty, "Carolingia-Regular-24", Color.Purple, Color.DarkBlue);
            _lblSettlementName2.LoadContent(content);

            _populationFrame = new PopulationFrame(this, new Vector2(_topLeftPositionMain.X + 20.0f, _topLeftPositionMain.Y + 40.0f));
            _populationFrame.LoadContent(content);
            _resourceFrame = new ResourceFrame(this, new Vector2(_topLeftPositionMain.X + 20.0f, _topLeftPositionMain.Y + 190.0f));
            _resourceFrame.LoadContent(content);
            _producingFrame = new ProducingFrame(this, new Vector2(_topLeftPositionMain.X + 20.0f, _topLeftPositionMain.Y + 390.0f));
            _producingFrame.LoadContent(content);

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

            _lblSettlementName1.Text = $"{Settlement.SettlementType} of";
            _lblSettlementName2.Text = $"{Settlement.Name}";

            _populationFrame.Update(input, deltaTime);
            _resourceFrame.Update(input, deltaTime);
            _producingFrame.Update(input, deltaTime);
            _buildingsFrame.Update(input, deltaTime);
            _unitsFrame.Update(input, deltaTime);
            _otherFrame.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _mainFrame.Draw();
            _secondaryFrame.Draw();

            _lblSettlementName1.Draw();
            _lblSettlementName2.Draw();

            _populationFrame.Draw(spriteBatch);
            _resourceFrame.Draw(spriteBatch);
            _producingFrame.Draw(spriteBatch);
            _buildingsFrame.Draw(spriteBatch);
            _unitsFrame.Draw(spriteBatch);
            _otherFrame.Draw(spriteBatch);
        }

        internal void CloseButtonClick(object sender, EventArgs e)
        {
            Command closeSettlementCommand = new CloseSettlementCommand();
            closeSettlementCommand.Payload = Settlement;
            closeSettlementCommand.Execute();
        }
    }
}