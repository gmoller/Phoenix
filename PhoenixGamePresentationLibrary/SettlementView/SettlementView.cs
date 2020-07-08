using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using GuiControls;
using HexLibrary;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using Utilities;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class SettlementView
    {
        private readonly WorldView _worldView;

        private Texture2D _guiTextures;
        private AtlasSpec2 _guiAtlas;

        private MainFrame _mainFrame;
        private SecondaryFrame _secondaryFrame;
        private Label _lblSettlementName1;
        private Label _lblSettlementName2;
        private PopulationFrame _populationFrame;
        private ResourceFrame _resourceFrame;
        private BuildingsFrame _buildingsFrame;
        private UnitsFrame _unitsFrame;
        private OtherFrame _otherFrame;
        private ProducingFrame _producingFrame;

        private Vector2 _topLeftPositionMain;
        private Vector2 _topLeftPositionSecondary;

        internal Settlement Settlement { get; set; }

        internal SettlementView(WorldView worldView, Settlement settlement)
        {
            _worldView = worldView;
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

            _lblSettlementName1 = new Label("lblSettlementName1", "Carolingia-Regular-24", new Vector2(_topLeftPositionMain.X + 278.0f, _topLeftPositionMain.Y - 49.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);
            _lblSettlementName2 = new Label("lblSettlementName2", "Carolingia-Regular-24", new Vector2(_topLeftPositionMain.X + 278.0f, _topLeftPositionMain.Y - 24.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);

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
            if (input.IsRightMouseButtonReleased && CursorIsOnThisSettlement())
            {
                Command openSettlementCommand = new OpenSettlementCommand();
                openSettlementCommand.Payload = Settlement;
                Globals.Instance.MessageQueue.Enqueue(openSettlementCommand);

                var worldPixelLocation = HexOffsetCoordinates.OffsetCoordinatesToPixel(Settlement.Location.X, Settlement.Location.Y);
                _worldView.Camera.LookAt(worldPixelLocation);
            }

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
            Globals.Instance.MessageQueue.Enqueue(closeSettlementCommand);
        }

        private bool CursorIsOnThisSettlement()
        {
            var hexPoint = GetHexPoint();

            return Settlement.Location == hexPoint;
        }

        private Utilities.Point GetHexPoint()
        {
            var hex = DeviceManager.Instance.WorldHex;
            var hexPoint = new Utilities.Point(hex.X, hex.Y);

            return hexPoint;
        }
    }
}