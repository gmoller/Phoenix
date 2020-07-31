using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using GuiControls;
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
        private Label _lblViewportPosition1;
        private Label _lblViewportPosition2;
        private Label _lblWorldPosition1;
        private Label _lblWorldPosition2;
        private Label _lblWorldHex1;
        private Label _lblWorldHex2;
        private Label _lblMemory1;
        private Label _lblMemory2;
        private Label _lblFps1;
        private Label _lblFps2;

        public MetricsPanel(Vector2 position)
        {
            _position = position;
        }

        public void LoadContent(ContentManager content)
        {
            _fps = new FramesPerSecondCounter();

            var size1 = new Vector2(140.0f, 20.0f);
            var size2 = new Vector2(140.0f, 20.0f);

            _lblGcCount1 = new LabelSized(Vector2.Zero, Alignment.TopLeft, size1, Alignment.MiddleLeft, "GC COUNT:", "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblGcCount1.LoadContent(content);
            _lblGcCount2 = new LabelSized(new Vector2(150.0f, 0.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblGcCount2.LoadContent(content);

            _lblScreenPosition1 = new LabelSized(new Vector2(0.0f, 30.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "SCREEN POS:", "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblScreenPosition1.LoadContent(content);
            _lblScreenPosition2 = new LabelSized(new Vector2(150.0f, 30.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblScreenPosition2.LoadContent(content);

            _lblViewportPosition1 = new LabelSized(new Vector2(0.0f, 60.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "VIEWPORT POS:", "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblViewportPosition1.LoadContent(content);
            _lblViewportPosition2 = new LabelSized(new Vector2(150.0f, 60.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblViewportPosition2.LoadContent(content);

            _lblWorldPosition1 = new LabelSized(new Vector2(0.0f, 90.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "WORLD POS:", "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldPosition1.LoadContent(content);
            _lblWorldPosition2 = new LabelSized(new Vector2(150.0f, 90.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldPosition2.LoadContent(content);

            _lblWorldHex1 = new LabelSized(new Vector2(0.0f, 120.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "WORLD HEX:", "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldHex1.LoadContent(content);
            _lblWorldHex2 = new LabelSized(new Vector2(150.0f, 120.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldHex2.LoadContent(content);

            _lblMemory1 = new LabelSized(new Vector2(0.0f, 150.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "MEMORY:", "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblMemory1.LoadContent(content);
            _lblMemory2 = new LabelSized(new Vector2(150.0f, 150.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblMemory2.LoadContent(content);

            _lblFps1 = new LabelSized(new Vector2(0.0f, 180.0f), Alignment.TopLeft, size1, Alignment.MiddleLeft, "FPS:", "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblFps1.LoadContent(content);
            _lblFps2 = new LabelSized(new Vector2(150.0f, 180.0f), Alignment.TopLeft, size2, Alignment.MiddleRight, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblFps2.LoadContent(content);
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
            _lblViewportPosition2.Text = $"({mouseState.Position.X - device.MapViewport.X},{mouseState.Position.Y - device.MapViewport.Y})";
            _lblScreenPosition2.Text = $"({mouseState.Position.X},{mouseState.Position.Y})";
        }

        public void Draw()
        {
            DeviceManager.Instance.SetViewport(DeviceManager.Instance.MetricsViewport);

            _lblGcCount1.Draw();
            _lblGcCount2.Draw();
            _lblScreenPosition1.Draw();
            _lblScreenPosition2.Draw();
            _lblViewportPosition1.Draw();
            _lblViewportPosition2.Draw();
            _lblWorldPosition1.Draw();
            _lblWorldPosition2.Draw();
            _lblWorldHex1.Draw();
            _lblWorldHex2.Draw();
            _lblMemory1.Draw();
            _lblMemory2.Draw();
            _lblFps1.Draw();
            _lblFps2.Draw();

            DeviceManager.Instance.ResetViewport();

            _fps.Draw();
        }
    }
}