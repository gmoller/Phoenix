using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Utilities;
using System;

namespace PhoenixGameLibrary
{
    public class CityView
    {
        private Texture2D _textureFrameMain;
        private Texture2D _textureFrameBigHeading;
        private Texture2D _textureFrameBottom;

        private Button _closeButton;

        private readonly Vector2 _topLeftPosition;

        public bool IsEnabled { get; set; }

        public CityView()
        {
            IsEnabled = false;
            _topLeftPosition = new Vector2(1920.0f * 0.65f, 10.0f);
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

            _closeButton = new Button(new Vector2(_topLeftPosition.X + 508.0f, _topLeftPosition.Y + 8.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(43.0f, 44.0f), textureCloseButtonNormal, textureCloseButtonActive, textureCloseButtonHover);
            _closeButton.Click += closeButtonClick;
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _closeButton.Update(gameTime);
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

                _closeButton.Draw();

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