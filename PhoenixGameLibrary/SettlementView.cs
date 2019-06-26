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
        private Texture2D _textureFrameMain;
        private Texture2D _textureFrameBigHeading;
        private Texture2D _textureFrameBottom;

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
            AssetsManager.Instance.AddTexture("frame_main", "Textures\\player_bg_outer");
            AssetsManager.Instance.AddTexture("frame_big_heading", "Textures\\texture_big_heading");
            AssetsManager.Instance.AddTexture("frame_bottom", "Textures\\bottom_box_ornament");
            AssetsManager.Instance.AddTexture("close_button_n", "Textures\\close_button_n");
            AssetsManager.Instance.AddTexture("close_button_a", "Textures\\close_button_a");
            AssetsManager.Instance.AddTexture("close_button_h", "Textures\\close_button_h");

            _textureFrameMain = AssetsManager.Instance.GetTexture("frame_main");
            _textureFrameBigHeading = AssetsManager.Instance.GetTexture("frame_big_heading");
            _textureFrameBottom = AssetsManager.Instance.GetTexture("frame_bottom");

            var textureCloseButtonNormal = AssetsManager.Instance.GetTexture("close_button_n");
            var textureCloseButtonActive = AssetsManager.Instance.GetTexture("close_button_a");
            var textureCloseButtonHover = AssetsManager.Instance.GetTexture("close_button_h");

            _btnClose = new Button(new Vector2(_topLeftPosition.X + 508.0f, _topLeftPosition.Y + 8.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(43.0f, 44.0f), textureCloseButtonNormal, textureCloseButtonActive, textureCloseButtonHover);
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
                spriteBatch.Draw(_textureFrameMain, _topLeftPosition, Color.White);
                spriteBatch.Draw(_textureFrameBigHeading, _topLeftPosition, Color.White);
                spriteBatch.Draw(_textureFrameBottom, new Vector2(_topLeftPosition.X + 0.0f, _topLeftPosition.Y + 680.0f), Color.White);
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