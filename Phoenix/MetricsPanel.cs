using System;
using Microsoft.Xna.Framework;
using GuiControls;
using Utilities;
using Microsoft.Xna.Framework.Input;

namespace Phoenix
{
    public class MetricsPanel
    {
        private FramesPerSecondCounter _fps;

        private Label _lblGCCount1;
        private Label _lblGCCount2;
        private Label _lblFps1;
        private Label _lblFps2;
        private Label _lblMemory1;
        private Label _lblMemory2;
        private Label _lblWorldHex1;
        private Label _lblWorldHex2;
        private Label _lblWorldPosition1;
        private Label _lblWorldPosition2;
        private Label _lblViewportPosition1;
        private Label _lblViewportPosition2;
        private Label _lblScreenPosition1;
        private Label _lblScreenPosition2;

        public MetricsPanel(Vector2 position)
        {
            _fps = new FramesPerSecondCounter();

            var size1 = new Vector2(142.0f, 20.0f);
            var size2 = new Vector2(142.0f, 20.0f);

            _lblScreenPosition1 = new Label("lblScreenPosition1", "CrimsonText-Regular-12", position, HorizontalAlignment.Left, VerticalAlignment.Bottom, size1, "SCREEN POS:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblScreenPosition2 = new Label("lblScreenPosition2", "CrimsonText-Regular-12", _lblScreenPosition1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblViewportPosition1 = new Label("lblViewportPosition1", "CrimsonText-Regular-12", _lblScreenPosition1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "VIEWPORT POS:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblViewportPosition2 = new Label("lblViewportPosition2", "CrimsonText-Regular-12", _lblViewportPosition1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblWorldPosition1 = new Label("lblWorldPosition1", "CrimsonText-Regular-12", _lblViewportPosition1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "WORLD POS:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldPosition2 = new Label("lblWorldPosition2", "CrimsonText-Regular-12", _lblWorldPosition1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblWorldHex1 = new Label("lblWorldHex1", "CrimsonText-Regular-12", _lblWorldPosition1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "WORLD HEX:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldHex2 = new Label("lblWorldHex2", "CrimsonText-Regular-12", _lblWorldHex1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblMemory1 = new Label("lblMemory1", "CrimsonText-Regular-12", _lblWorldHex1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "MEM:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblMemory2 = new Label("lblMemory2", "CrimsonText-Regular-12", _lblMemory1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblFps1 = new Label("lblFps1", "CrimsonText-Regular-12", _lblMemory1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "FPS (U/D):", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblFps2 = new Label("lblFps2", "CrimsonText-Regular-12", _lblFps1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblGCCount1 = new Label("lblGCCount1", "CrimsonText-Regular-12", _lblFps1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "GC COUNT:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblGCCount2 = new Label("lblGCCount2", "CrimsonText-Regular-12", _lblGCCount1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            _fps.Update(gameTime);

            MouseState mouseState = Mouse.GetState();

            var device = DeviceManager.Instance;
            _lblGCCount2.Text = $"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}";
            _lblFps2.Text = $"{_fps.UpdateFramesPerSecond}/{_fps.DrawFramesPerSecond}";
            _lblMemory2.Text = $"{GC.GetTotalMemory(false) / 1024} KB";
            _lblWorldHex2.Text = $"{device.WorldHex.X},{device.WorldHex.Y}";
            _lblWorldPosition2.Text = $"({device.WorldPosition.X},{device.WorldPosition.Y})";
            _lblViewportPosition2.Text = $"({mouseState.Position.X - device.MapViewport.X},{mouseState.Position.Y - device.MapViewport.Y})";
            _lblScreenPosition2.Text = $"({mouseState.Position.X},{mouseState.Position.Y})";
        }

        public void Draw()
        {
            DeviceManager.Instance.SetViewport(DeviceManager.Instance.MetricsViewport);

            _lblGCCount1.Draw();
            _lblGCCount2.Draw();
            _lblFps1.Draw();
            _lblFps2.Draw();
            _lblMemory1.Draw();
            _lblMemory2.Draw();
            _lblWorldHex1.Draw();
            _lblWorldHex2.Draw();
            _lblWorldPosition1.Draw();
            _lblWorldPosition2.Draw();
            _lblViewportPosition1.Draw();
            _lblViewportPosition2.Draw();
            _lblScreenPosition1.Draw();
            _lblScreenPosition2.Draw();

            DeviceManager.Instance.ResetViewport();

            _fps.Draw();
        }
    }
}