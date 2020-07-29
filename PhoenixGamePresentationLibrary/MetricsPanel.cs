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

        private Label _lblGCCount1;
        private Label _lblGCCount2;
        private LabelOld _lblFps1;
        private LabelOld _lblFps2;
        private LabelOld _lblMemory1;
        private LabelOld _lblMemory2;
        private LabelOld _lblWorldHex1;
        private LabelOld _lblWorldHex2;
        private LabelOld _lblWorldPosition1;
        private LabelOld _lblWorldPosition2;
        private LabelOld _lblViewportPosition1;
        private LabelOld _lblViewportPosition2;
        private LabelOld _lblScreenPosition1;
        private LabelOld _lblScreenPosition2;

        public MetricsPanel(Vector2 position)
        {
            _position = position;
        }

        public void LoadContent(ContentManager content)
        {
            _fps = new FramesPerSecondCounter();

            var size1 = new Vector2(142.0f, 20.0f);
            var size2 = new Vector2(142.0f, 20.0f);

            _lblGCCount1 = new Label("lblGCCount1", Vector2.Zero, Alignment.TopLeft, "GC COUNT:", "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f);
            _lblGCCount1.LoadContent(content);
            _lblGCCount2 = new Label("lblGCCount2", new Vector2(100.0f, 0.0f), Alignment.TopLeft, string.Empty, "CrimsonText-Regular-12", Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f);
            _lblGCCount2.LoadContent(content);

            _lblScreenPosition1 = new LabelOld("lblScreenPosition1", "CrimsonText-Regular-12", _position, HorizontalAlignment.Left, VerticalAlignment.Bottom, size1, "SCREEN POS:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblScreenPosition2 = new LabelOld("lblScreenPosition2", "CrimsonText-Regular-12", _lblScreenPosition1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblViewportPosition1 = new LabelOld("lblViewportPosition1", "CrimsonText-Regular-12", _lblScreenPosition1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "VIEWPORT POS:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblViewportPosition2 = new LabelOld("lblViewportPosition2", "CrimsonText-Regular-12", _lblViewportPosition1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblWorldPosition1 = new LabelOld("lblWorldPosition1", "CrimsonText-Regular-12", _lblViewportPosition1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "WORLD POS:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldPosition2 = new LabelOld("lblWorldPosition2", "CrimsonText-Regular-12", _lblWorldPosition1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblWorldHex1 = new LabelOld("lblWorldHex1", "CrimsonText-Regular-12", _lblWorldPosition1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "WORLD HEX:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblWorldHex2 = new LabelOld("lblWorldHex2", "CrimsonText-Regular-12", _lblWorldHex1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblMemory1 = new LabelOld("lblMemory1", "CrimsonText-Regular-12", _lblWorldHex1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "MEM:", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblMemory2 = new LabelOld("lblMemory2", "CrimsonText-Regular-12", _lblMemory1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);

            _lblFps1 = new LabelOld("lblFps1", "CrimsonText-Regular-12", _lblMemory1, HorizontalAlignment.Center, VerticalAlignment.Top, size1, "FPS (U/D):", HorizontalAlignment.Left, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
            _lblFps2 = new LabelOld("lblFps2", "CrimsonText-Regular-12", _lblFps1, HorizontalAlignment.Right, VerticalAlignment.Middle, size2, string.Empty, HorizontalAlignment.Right, Color.LawnGreen, Color.DarkRed, Color.DarkSlateGray * 0.5f, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            _fps.Update(gameTime);

            MouseState mouseState = Mouse.GetState();

            var device = DeviceManager.Instance;
            _lblGCCount2.Text = $"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}";
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