using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;

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

            var font = AssetsManager.Instance.GetSpriteFont("TimesSanSerif");
            Vector2 size = new Vector2(270.0f, 50.0f);

            _lblMemory1 = new Label(font, position, HorizontalAlignment.Left, VerticalAlignment.Bottom, size, "MEM:", Color.LawnGreen, Color.DarkRed, Color.White);
            _lblMemory2 = new Label(font, _lblMemory1, HorizontalAlignment.Right, VerticalAlignment.Middle, new Vector2(size.X, size.Y), string.Empty, Color.LawnGreen, Color.DarkRed, Color.White);

            _lblFps1 = new Label(font, _lblMemory1, HorizontalAlignment.Center, VerticalAlignment.Top, size, "FPS (U/D):", Color.LawnGreen, Color.DarkRed, Color.White);
            _lblFps2 = new Label(font, _lblFps1, HorizontalAlignment.Right, VerticalAlignment.Middle, size, string.Empty, Color.LawnGreen, Color.DarkRed, Color.White);

            _lblGCCount1 = new Label(font, _lblFps1, HorizontalAlignment.Center, VerticalAlignment.Top, size, "GC COUNT:", Color.LawnGreen, Color.DarkRed, Color.White);
            _lblGCCount2 = new Label(font, _lblGCCount1, HorizontalAlignment.Right, VerticalAlignment.Middle, size, string.Empty, Color.LawnGreen, Color.DarkRed, Color.White);
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
            _lblFps1.Draw(spriteBatch);
            _lblFps2.Draw(spriteBatch);
            _lblMemory1.Draw(spriteBatch);
            _lblMemory2.Draw(spriteBatch);
            _lblGCCount1.Draw(spriteBatch);
            _lblGCCount2.Draw(spriteBatch);

            _fps.Draw();
        }
    }
}