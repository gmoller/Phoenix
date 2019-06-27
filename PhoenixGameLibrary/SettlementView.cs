using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Utilities;
using System;

namespace PhoenixGameLibrary
{
    public class SettlementView
    {
        private Texture2D _guiTextures;
        private AtlasSpec2 _guiAtlas;

        private Button _btnClose;
        private Label _lblSettlementName1;
        private Label _lblSettlementName2;
        private Label _lblPopulationGrowth;
        private Label _lblCitizens;

        private readonly Settlement _settlement;
        private readonly Vector2 _topLeftPosition;

        public bool IsEnabled { get; set; }

        public SettlementView(Settlement settlement)
        {
            _settlement = settlement;
            IsEnabled = false;
            _topLeftPosition = new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.65f, 200.0f);
        }

        public void LoadContent(ContentManager content)
        {
            _guiTextures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            _guiAtlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");

            _btnClose = new Button(new Vector2(_topLeftPosition.X + 508.0f, _topLeftPosition.Y + 8.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(43.0f, 44.0f), "GUI_Textures_1", "close_button_n", "close_button_a", "close_button_h");
            _btnClose.Click += closeButtonClick;

            var font = AssetsManager.Instance.GetSpriteFont("Carolingia-Regular-24");
            _lblSettlementName1 = new Label(font, new Vector2(_topLeftPosition.X + 278.0f, _topLeftPosition.Y + 51.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);
            _lblSettlementName2 = new Label(font, new Vector2(_topLeftPosition.X + 278.0f, _topLeftPosition.Y + 76.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Center, Color.Purple, Color.DarkBlue);
            font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");
            _lblPopulationGrowth = new Label(font, new Vector2(_topLeftPosition.X + 536.0f, _topLeftPosition.Y + 140.0f), HorizontalAlignment.Right, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblCitizens = new Label(font, new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 140.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblSettlementName1.Text = $"{_settlement.SettlementType} of";
            _lblSettlementName2.Text = $"{_settlement.Name}";
            _lblPopulationGrowth.Text = $"Population: {_settlement.Population} (+{_settlement.PopulationGrowth})";
            _lblCitizens.Text = $"SF: {_settlement.Citizens.SubsistenceFarmers} F: {_settlement.Citizens.AdditionalFarmers} W: {_settlement.Citizens.Workers}";
            _btnClose.Update(gameTime);
        }

        public void Draw()
        {
            if (IsEnabled)
            {
                //DeviceManager.Instance.SetViewport(new Viewport(1035, 0, 556, 800, 0, 1));

                var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

                spriteBatch.Begin();

                var frame = _guiAtlas.Frames["player_bg_outer"];
                spriteBatch.Draw(_guiTextures, _topLeftPosition, new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                frame = _guiAtlas.Frames["texture_big_heading"];
                spriteBatch.Draw(_guiTextures, _topLeftPosition, new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                frame = _guiAtlas.Frames["bottom_box_ornament"];
                spriteBatch.Draw(_guiTextures, new Vector2(_topLeftPosition.X + 0.0f, _topLeftPosition.Y + 680.0f), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);

                frame = _guiAtlas.Frames["top_h_border_repeat_x"];
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(106, 200), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                frame = _guiAtlas.Frames["left_v_border_repeat_y"];
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(100, 205), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                frame = _guiAtlas.Frames["right_v_border_repeat_y"];
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(298, 205), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                frame = _guiAtlas.Frames["bottom_h_border_repeat_x"];
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(106, 318), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);

                frame = _guiAtlas.Frames["frame_corner"];
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(100, 200), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(296, 200), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(100, 318), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(296, 318), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);

                frame = _guiAtlas.Frames["slot"];
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(110, 210), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(162, 210), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(214, 210), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(110, 262), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(162, 262), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);
                spriteBatch.Draw(_guiTextures, _topLeftPosition + new Vector2(214, 262), new Rectangle(frame.X, frame.Y, frame.Width, frame.Height), Color.White);

                spriteBatch.End();

                _lblSettlementName1.Draw();
                _lblSettlementName2.Draw();
                _lblPopulationGrowth.Draw();
                _lblCitizens.Draw();
                _btnClose.Draw();

                //DeviceManager.Instance.ResetViewport();
            }
        }

        private void closeButtonClick(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                IsEnabled = false;
            }
        }
    }
}