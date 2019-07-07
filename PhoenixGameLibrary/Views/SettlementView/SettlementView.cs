using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using GuiControls;
using HexLibrary;
using Utilities;

namespace PhoenixGameLibrary.Views.SettlementView
{
    public class SettlementView
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

        private readonly Settlement _settlement;
        private readonly Vector2 _topLeftPositionMain;
        private readonly Vector2 _topLeftPositionSecondary;

        public bool IsEnabled { get; set; }

        public SettlementView(Settlement settlement)
        {
            _settlement = settlement;
            IsEnabled = false;
            _topLeftPositionMain = new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.05f, 200.0f);
            _topLeftPositionSecondary = new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.65f, 200.0f);
        }

        public void LoadContent(ContentManager content)
        {
            _guiTextures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            _guiAtlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");

            _mainFrame = new MainFrame(this, _topLeftPositionMain, _guiTextures, _guiAtlas);
            _secondaryFrame = new SecondaryFrame(this, _topLeftPositionSecondary, _guiTextures, _guiAtlas);

            _lblSettlementName1 = new Label("lblSettlementName1", "Carolingia-Regular-24", new Vector2(_topLeftPositionMain.X + 278.0f, _topLeftPositionMain.Y - 49.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);
            _lblSettlementName2 = new Label("lblSettlementName2", "Carolingia-Regular-24", new Vector2(_topLeftPositionMain.X + 278.0f, _topLeftPositionMain.Y - 24.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);

            _populationFrame = new PopulationFrame(new Vector2(_topLeftPositionMain.X + 20.0f, _topLeftPositionMain.Y + 40.0f), _settlement);
            _resourceFrame = new ResourceFrame(new Vector2(_topLeftPositionMain.X + 20.0f, _topLeftPositionMain.Y + 160.0f), _settlement);
            _producingFrame = new ProducingFrame(new Vector2(_topLeftPositionMain.X + 20.0f, _topLeftPositionMain.Y + 360.0f), _settlement);

            _buildingsFrame = new BuildingsFrame(new Vector2(_topLeftPositionSecondary.X + 20.0f, _topLeftPositionSecondary.Y + 40.0f), _settlement);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            Globals.Instance.World.CanScrollMap &= !IsEnabled;

            if (IsEnabled)
            {
                _mainFrame.Update(gameTime, input);
                _secondaryFrame.Update(gameTime, input);

                _lblSettlementName1.Text = $"{_settlement.SettlementType} of";
                _lblSettlementName2.Text = $"{_settlement.Name}";

                _populationFrame.Update(gameTime, input);
                _resourceFrame.Update(gameTime, input);
                _producingFrame.Update(gameTime, input);
                _buildingsFrame.Update(gameTime, input);
            }
        }

        public void Draw()
        {
            if (IsEnabled)
            {
                //DeviceManager.Instance.SetViewport(new Viewport(1035, 0, 556, 800, 0, 1));

                _mainFrame.Draw();
                _secondaryFrame.Draw();

                _lblSettlementName1.Draw();
                _lblSettlementName2.Draw();

                _populationFrame.Draw();
                _resourceFrame.Draw();
                _producingFrame.Draw();
                _buildingsFrame.Draw();

                var catchment = HexOffsetCoordinates.GetAllNeighbors(_settlement.Location.X, _settlement.Location.Y);
                foreach (var tile in catchment)
                {
                    var cell = Globals.Instance.World.OverlandMap.CellGrid.GetCell(tile.Col, tile.Row);
                    DrawCell(cell, 1.0f);
                }

                //DeviceManager.Instance.ResetViewport();
            }
        }

        // TODO: get rid of this duplication, cater for depth, zoom in, draw settlement
        private void DrawCell(Cell cell, float layerDepth)
        {
            var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
            var texture = AssetsManager.Instance.GetTexture(cell.Texture.TexturePalette);
            var spec = AssetsManager.Instance.GetAtlas(cell.Texture.TexturePalette);
            var frame = spec.Frames[cell.Texture.TextureId];
            var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 111, 192);
            var camera = Globals.Instance.World.Camera;
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, camera.Transform);
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.LightSlateGray, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
            spriteBatch.End();
        }

        public void CloseButtonClick(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                IsEnabled = false;
            }
        }
    }
}