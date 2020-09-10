using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using MonoGameUtilities.ViewportAdapters;
using Utilities;

namespace PhoenixGamePresentation
{
    public class MetricsPanel
    {
        #region State
        private FramesPerSecondCounter _fps;

        private readonly List<Label> _labels;

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;
        #endregion

        public MetricsPanel()
        {
            List<(string name, string text)> metrics = new List<(string, string)>
            {
                ("lblFps", "FPS (Update/Draw):"),
                ("lblGcCount", "GC COUNT:"),
                ("lblMemory", "MEMORY:"),
                ("lblScreenPosition", "SCREEN POS:"),
                ("lblWorldPosition", "WORLD POS:"),
                ("lblWorldHex", "WORLD HEX:"),
                ("lblZoom", "ZOOM:"),
                //("lblResolution1", "ClientBounds:"),
                //("lblResolution2", "Viewport:"),
                //("lblResolution3", "DisplayMode:"),
                //("lblResolution4", "CurrentDisplayMode:")
            };

            _labels = new List<Label>();

            var y = 0.0f;
            for (var i = 0; i < metrics.Count; i++)
            {
                var x = i * 320;
                var pos = new Vector2(x, y);
                var label1 = CreateControl(pos, new Vector2(160.0f, 20.0f), Alignment.MiddleLeft, metrics[i].text, $"{metrics[i].name}1");
                _labels.Add(label1);
                var label2 = CreateControl(pos + new Vector2(160.0f, 0.0f), new Vector2(80.0f, 20.0f), Alignment.MiddleLeft, string.Empty, $"{metrics[i].name}2");
                _labels.Add(label2);
            }

            SetupViewport(0, 1060, 1920, 20);
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            _viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            _viewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
        }

        public void LoadContent(ContentManager content)
        {
            _fps = new FramesPerSecondCounter();

            foreach (var label in _labels)
            {
                label.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            var gameWindow = context.GameWindow;
            var graphicsDevice = context.GraphicsDevice;
            var camera = context.Camera;

            _fps.Update(gameTime, _viewport);

            var mouseState = Mouse.GetState();
            var worldPosition = camera.ScreenPixelToWorldPixel(mouseState.Position);
            var worldHex = camera.ScreenPixelToWorldHex(mouseState.Position);

            _labels[1].Text = $"{_fps.UpdateFramesPerSecond}/{_fps.DrawFramesPerSecond}";
            _labels[3].Text = $"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}";
            _labels[5].Text = $"{(GC.GetTotalMemory(false) / 1024):N0} KB";
            _labels[7].Text = $"({mouseState.Position.X},{mouseState.Position.Y})";
            _labels[9].Text = $"({worldPosition.X:F0},{worldPosition.Y:F0})";
            _labels[11].Text = $"{worldHex.Col},{worldHex.Row}";
            _labels[13].Text = $"{camera.Zoom}";
            //_labels[15].Text = $"{gameWindow.ClientBounds.Width}x{gameWindow.ClientBounds.Height}";
            //_labels[17].Text = $"{_viewportAdapter.Viewport.Width}x{_viewportAdapter.Viewport.Height}";
            //_labels[19].Text = $"{graphicsDevice.DisplayMode.Width}x{graphicsDevice.DisplayMode.Height}";
            //_labels[21].Text = $"{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width}x{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height}";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = _viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: _viewportAdapter.GetScaleMatrix());

            foreach (var label in _labels)
            {
                label.Draw(spriteBatch);
            }

            _fps.Draw();

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        private Label CreateControl(Vector2 position, Vector2 size, Alignment contentAlignment, string text, string name)
        {
            var control = new LabelSized(position, Alignment.TopLeft, size, contentAlignment, text, "Arial-12", Color.LawnGreen, name, Color.DarkRed, Color.DarkSlateGray * Constants.ONE_HALF, Color.White);

            return control;
        }
    }
}