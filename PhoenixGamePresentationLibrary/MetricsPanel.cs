using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using GuiControls;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class MetricsPanel
    {
        private readonly Vector2 _position;

        private FramesPerSecondCounter _fps;

        private Label _lblGcCount1;
        private Label _lblGcCount2;
        private Label _lblScreenPosition1;
        private Label _lblScreenPosition2;
        private Label _lblWorldPosition1;
        private Label _lblWorldPosition2;
        private Label _lblWorldHex1;
        private Label _lblWorldHex2;
        private Label _lblMemory1;
        private Label _lblMemory2;
        private Label _lblFps1;
        private Label _lblFps2;
        private Label _lblResolution1;
        private Label _lblResolution2;
        private Label _lblResolution3;
        private Label _lblResolution4;
        private Label _lblResolution5;
        private Label _lblResolution6;
        private Label _lblResolution7;
        private Label _lblResolution8;

        public MetricsPanel(Vector2 position)
        {
            _position = position;
        }

        public void LoadContent(ContentManager content)
        {
            _fps = new FramesPerSecondCounter();

            var size1 = new Vector2(150.0f, 20.0f);
            var size2 = new Vector2(140.0f, 20.0f);

            _lblGcCount1 = new LabelSized(Vector2.Zero, Alignment.TopLeft, size1, Alignment.MiddleLeft, "GC COUNT:", "CrimsonText-Regular-12", Color.LawnGreen, "lblGcCount1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblGcCount1.LoadContent(content);
            _lblGcCount2 = new LabelSized(new Vector2(150.0f, 0.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblGcCount2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblGcCount2.LoadContent(content);

            _lblScreenPosition1 = new LabelSized(new Vector2(0.0f, 30.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "SCREEN POS:", "CrimsonText-Regular-12", Color.LawnGreen, "lblScreenPosition1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblScreenPosition1.LoadContent(content);
            _lblScreenPosition2 = new LabelSized(new Vector2(150.0f, 30.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblScreenPosition2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblScreenPosition2.LoadContent(content);

            _lblWorldPosition1 = new LabelSized(new Vector2(0.0f, 60.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "WORLD POS:", "CrimsonText-Regular-12", Color.LawnGreen, "lblWorldPosition1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldPosition1.LoadContent(content);
            _lblWorldPosition2 = new LabelSized(new Vector2(150.0f, 60.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblWorldPosition2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldPosition2.LoadContent(content);

            _lblWorldHex1 = new LabelSized(new Vector2(0.0f, 90.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "WORLD HEX:", "CrimsonText-Regular-12", Color.LawnGreen, "lblWorldHex1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldHex1.LoadContent(content);
            _lblWorldHex2 = new LabelSized(new Vector2(150.0f, 90.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblWorldHex2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldHex2.LoadContent(content);

            _lblMemory1 = new LabelSized(new Vector2(0.0f, 120.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "MEMORY:", "CrimsonText-Regular-12", Color.LawnGreen, "lblMemory1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblMemory1.LoadContent(content);
            _lblMemory2 = new LabelSized(new Vector2(150.0f, 120.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblMemory2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblMemory2.LoadContent(content);

            _lblFps1 = new LabelSized(new Vector2(0.0f, 150.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "FPS (Update/Draw):", "CrimsonText-Regular-12", Color.LawnGreen, "lblFps1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblFps1.LoadContent(content);
            _lblFps2 = new LabelSized(new Vector2(150.0f, 150.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblFps2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblFps2.LoadContent(content);

            _lblResolution1 = new LabelSized(new Vector2(0.0f, 180.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "ClientBounds:", "CrimsonText-Regular-12", Color.LawnGreen, "lblResolution1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblResolution1.LoadContent(content);
            _lblResolution2 = new LabelSized(new Vector2(150.0f, 180.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblResolution2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblResolution2.LoadContent(content);

            _lblResolution3 = new LabelSized(new Vector2(0.0f, 210.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "Viewport:", "CrimsonText-Regular-12", Color.LawnGreen, "lblResolution1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblResolution3.LoadContent(content);
            _lblResolution4 = new LabelSized(new Vector2(150.0f, 210.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblResolution2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblResolution4.LoadContent(content);

            _lblResolution5 = new LabelSized(new Vector2(0.0f, 240.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "DisplayMode:", "CrimsonText-Regular-12", Color.LawnGreen, "lblResolution1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblResolution5.LoadContent(content);
            _lblResolution6 = new LabelSized(new Vector2(150.0f, 240.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblResolution2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblResolution6.LoadContent(content);

            _lblResolution7 = new LabelSized(new Vector2(0.0f, 270.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "CurrentDisplayMode:", "CrimsonText-Regular-12", Color.LawnGreen, "lblResolution1", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblResolution7.LoadContent(content);
            _lblResolution8 = new LabelSized(new Vector2(150.0f, 270.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, "lblResolution2", Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblResolution8.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            _fps.Update(gameTime);

            MouseState mouseState = Mouse.GetState();

            var device = DeviceManager.Instance;
            _lblGcCount2.Text = $"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}";
            _lblFps2.Text = $"{_fps.UpdateFramesPerSecond}/{_fps.DrawFramesPerSecond}";
            _lblMemory2.Text = $"{GC.GetTotalMemory(false) / 1024} KB";
            _lblWorldHex2.Text = $"{device.WorldHexPointedAtByMouseCursor.X},{device.WorldHexPointedAtByMouseCursor.Y}";
            _lblWorldPosition2.Text = $"({device.WorldPositionPointedAtByMouseCursor.X},{device.WorldPositionPointedAtByMouseCursor.Y})";
            _lblScreenPosition2.Text = $"({mouseState.Position.X},{mouseState.Position.Y})";
            _lblResolution2.Text = $"{device.Window.ClientBounds.Width}x{device.Window.ClientBounds.Height}";
            _lblResolution4.Text = $"{device.ViewportAdapter.Viewport.Width}x{device.ViewportAdapter.Viewport.Height}";
            _lblResolution6.Text = $"{device.GraphicsDevice.DisplayMode.Width}x{device.GraphicsDevice.DisplayMode.Height}";
            _lblResolution8.Text = $"{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width}x{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height}";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DeviceManager.Instance.SetViewport(DeviceManager.Instance.MetricsViewport);

            _lblGcCount1.Draw(spriteBatch);
            _lblGcCount2.Draw(spriteBatch);
            _lblScreenPosition1.Draw(spriteBatch);
            _lblScreenPosition2.Draw(spriteBatch);
            _lblWorldPosition1.Draw(spriteBatch);
            _lblWorldPosition2.Draw(spriteBatch);
            _lblWorldHex1.Draw(spriteBatch);
            _lblWorldHex2.Draw(spriteBatch);
            _lblMemory1.Draw(spriteBatch);
            _lblMemory2.Draw(spriteBatch);
            _lblFps1.Draw(spriteBatch);
            _lblFps2.Draw(spriteBatch);
            _lblResolution1.Draw(spriteBatch);
            _lblResolution2.Draw(spriteBatch);
            _lblResolution3.Draw(spriteBatch);
            _lblResolution4.Draw(spriteBatch);
            _lblResolution5.Draw(spriteBatch);
            _lblResolution6.Draw(spriteBatch);
            _lblResolution7.Draw(spriteBatch);
            _lblResolution8.Draw(spriteBatch);

            DeviceManager.Instance.ResetViewport();

            _fps.Draw();
        }
    }
}