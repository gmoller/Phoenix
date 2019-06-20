using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Utilities;

namespace Phoenix
{
    public class MetricsPanel
    {
        private FramesPerSecondCounter _fps;

        private Label _lblFps1;
        private Label _lblFps2;
        private Label _lblMemory1;
        private Label _lblMemory2;
        private Label _lblGCCount1;
        private Label _lblGCCount2;

        private Label _lblTest;

        public MetricsPanel(Vector2 position)
        {
            _fps = new FramesPerSecondCounter();

            var font = AssetsManager.Instance.GetSpriteFont("TimesSanSerif");
            //var font = AssetsManager.Instance.GetSpriteFont("carolingia");
            Vector2 size = new Vector2(270.0f, 50.0f);

            _lblMemory1 = new Label(font, position, HorizontalAlignment.Left, VerticalAlignment.Bottom, size, "MEM:", Color.LawnGreen, Color.DarkRed, Color.White);
            _lblMemory2 = new Label(font, _lblMemory1, HorizontalAlignment.Right, VerticalAlignment.Middle, new Vector2(size.X, size.Y), string.Empty, Color.LawnGreen, Color.DarkRed, Color.White);

            _lblFps1 = new Label(font, _lblMemory1, HorizontalAlignment.Center, VerticalAlignment.Top, size, "FPS (U/D):", Color.LawnGreen, Color.DarkRed, Color.White);
            _lblFps2 = new Label(font, _lblFps1, HorizontalAlignment.Right, VerticalAlignment.Middle, size, string.Empty, Color.LawnGreen, Color.DarkRed, Color.White);

            _lblGCCount1 = new Label(font, _lblFps1, HorizontalAlignment.Center, VerticalAlignment.Top, size, "GC COUNT:", Color.LawnGreen, Color.DarkRed, Color.White);
            _lblGCCount2 = new Label(font, _lblGCCount1, HorizontalAlignment.Right, VerticalAlignment.Middle, size, string.Empty, Color.LawnGreen, Color.DarkRed, Color.White);

            _lblTest = new Label(font, new Vector2(1450.0f, 175.0f), HorizontalAlignment.Center, VerticalAlignment.Middle, new Vector2(245.0f ,56.0f), "", Color.Yellow, null, null, AssetsManager.Instance.GetTexture("reg_button_n"));
        }

        public void Update(GameTime gameTime)
        {
            _fps.Update(gameTime);

            _lblFps2.Text = $"{_fps.UpdateFramesPerSecond}/{_fps.DrawFramesPerSecond}";
            _lblMemory2.Text = $"{GC.GetTotalMemory(false) / 1024} KB";
            _lblGCCount2.Text = $"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var original = DeviceManager.Instance.GraphicsDevice.Viewport;
            DeviceManager.Instance.GraphicsDevice.Viewport = DeviceManager.Instance.MetricsViewport;

            spriteBatch.Begin();

            _lblFps1.Draw(spriteBatch);
            _lblFps2.Draw(spriteBatch);
            _lblMemory1.Draw(spriteBatch);
            _lblMemory2.Draw(spriteBatch);
            _lblGCCount1.Draw(spriteBatch);
            _lblGCCount2.Draw(spriteBatch);

            _lblTest.Draw(spriteBatch);

            spriteBatch.End();

            DeviceManager.Instance.GraphicsDevice.Viewport = original;

            _fps.Draw();
        }
    }
}