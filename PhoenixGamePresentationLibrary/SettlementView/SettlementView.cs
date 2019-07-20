using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using GuiControls;
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
        private Label _lblSettlementName1;
        private Label _lblSettlementName2;
        private PopulationFrame _populationFrame;
        private ResourceFrame _resourceFrame;
        private BuildingsFrame _buildingsFrame;
        private ProducingFrame _producingFrame;

        private readonly Vector2 _topLeftPositionMain;
        private readonly Vector2 _topLeftPositionSecondary;

        internal Settlement Settlement { get; set; }

        internal SettlementView()
        {
            _topLeftPositionMain = new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.05f, 200.0f);
            _topLeftPositionSecondary = new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.65f, 200.0f);
        }

        internal void LoadContent(ContentManager content)
        {
            _guiTextures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            _guiAtlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");

            _mainFrame = new MainFrame(this, _topLeftPositionMain, _guiTextures, _guiAtlas);
            _secondaryFrame = new SecondaryFrame(this, _topLeftPositionSecondary, _guiTextures, _guiAtlas);

            _lblSettlementName1 = new Label("lblSettlementName1", "Carolingia-Regular-24", new Vector2(_topLeftPositionMain.X + 278.0f, _topLeftPositionMain.Y - 49.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);
            _lblSettlementName2 = new Label("lblSettlementName2", "Carolingia-Regular-24", new Vector2(_topLeftPositionMain.X + 278.0f, _topLeftPositionMain.Y - 24.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);

            _populationFrame = new PopulationFrame(this, new Vector2(_topLeftPositionMain.X + 20.0f, _topLeftPositionMain.Y + 40.0f));
            _resourceFrame = new ResourceFrame(this, new Vector2(_topLeftPositionMain.X + 20.0f, _topLeftPositionMain.Y + 190.0f));
            _producingFrame = new ProducingFrame(this, new Vector2(_topLeftPositionMain.X + 20.0f, _topLeftPositionMain.Y + 390.0f));

            _buildingsFrame = new BuildingsFrame(this, new Vector2(_topLeftPositionSecondary.X + 20.0f, _topLeftPositionSecondary.Y + 40.0f));
        }

        internal void Update(GameTime gameTime, InputHandler input)
        {
            _mainFrame.Update(gameTime, input);
            _secondaryFrame.Update(gameTime, input);

            _lblSettlementName1.Text = $"{Settlement.SettlementType} of";
            _lblSettlementName2.Text = $"{Settlement.Name}";

            _populationFrame.Update(gameTime, input);
            _resourceFrame.Update(gameTime, input);
            _producingFrame.Update(gameTime, input);
            _buildingsFrame.Update(gameTime, input);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _mainFrame.Draw();
            _secondaryFrame.Draw();

            _lblSettlementName1.Draw();
            _lblSettlementName2.Draw();

            _populationFrame.Draw(spriteBatch);
            _resourceFrame.Draw();
            _producingFrame.Draw();
            _buildingsFrame.Draw();
        }

        internal void CloseButtonClick(object sender, EventArgs e)
        {
            Globals.Instance.MessageQueue.Enqueue(new CloseSettlementCommand());
        }
    }
}