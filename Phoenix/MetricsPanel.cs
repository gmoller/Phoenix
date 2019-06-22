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

        public MetricsPanel(Vector2 position)
        {
            _fps = new FramesPerSecondCounter();

            var font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");
            var size1 = font.MeasureString("GC COUNT:") + new Vector2(10.0f, 0.0f);
            var size2 = size1 - new Vector2(10.0f, 0.0f);

            _lblMemory1 = new Label(font, position, HorizontalAlignment.Left, VerticalAlignment.Bottom, size1, "MEM:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblMemory2 = new Label(font, _lblMemory1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblFps1 = new Label(font, _lblMemory1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "FPS (U/D):", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblFps2 = new Label(font, _lblFps1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblGCCount1 = new Label(font, _lblFps1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "GC COUNT:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblGCCount2 = new Label(font, _lblGCCount1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
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

            _lblFps1.Draw(spriteBatch);
            _lblFps2.Draw(spriteBatch);
            _lblMemory1.Draw(spriteBatch);
            _lblMemory2.Draw(spriteBatch);
            _lblGCCount1.Draw(spriteBatch);
            _lblGCCount2.Draw(spriteBatch);

            DeviceManager.Instance.GraphicsDevice.Viewport = original;

            _fps.Draw();
        }
    }
}