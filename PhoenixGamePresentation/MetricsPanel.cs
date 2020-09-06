using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using MonoGameUtilities.ViewportAdapters;
using Utilities;
using Color = Microsoft.Xna.Framework.Color;
using Utilities.ExtensionMethods;

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
            List<(string text, string name)> metrics = new List<(string, string)>
            {
                ("GC COUNT:", "lblGcCount1"), (string.Empty, "lblGcCount2"),
                ("SCREEN POS:", "lblScreenPosition1"), (string.Empty, "lblScreenPosition2"),
                ("WORLD POS:",  "lblWorldPosition1"), (string.Empty, "lblWorldPosition2"),
                ("WORLD HEX:", "lblWorldHex1"), (string.Empty, "lblWorldHex2"),
                ("MEMORY:", "lblMemory1"), (string.Empty, "lblMemory2"),
                ("FPS (Update/Draw):", "lblFps1"), (string.Empty, "lblFps2"),
                ("ClientBounds:", "lblResolution11"), (string.Empty, "lblResolution12"),
                ("Viewport:", "lblResolution21"), (string.Empty, "lblResolution22"),
                ("DisplayMode:", "lblResolution31"), (string.Empty, "lblResolution32"),
                ("CurrentDisplayMode:", "lblResolution41"), (string.Empty, "lblResolution42"),
                ("Zoom:", "lblZoom1"), (string.Empty, "lblZoom2")
            };

            _labels = new List<Label>();
            var y0 = -30.0f;
            for (var i = 0; i < metrics.Count; i++)
            {
                var x0 = i.IsEven() ? 0.0f : 160.0f;
                if (i.IsEven())
                {
                    y0 += 30.0f;
                }
                var pos = new Vector2(x0, y0);
                var contentAlignment = i.IsEven() ? Alignment.MiddleLeft : Alignment.MiddleRight;
                var label = CreateControl(pos, contentAlignment, metrics[i].text, metrics[i].name);
                _labels.Add(label);
            }

            SetupViewport(0, 0, 320, 330);
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

            _labels[1].Text = $"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}";
            _labels[3].Text = $"({mouseState.Position.X},{mouseState.Position.Y})";
            _labels[5].Text = $"({worldPosition.X},{worldPosition.Y})";
            _labels[7].Text = $"{worldHex.Col},{worldHex.Row}";
            _labels[9].Text = $"{GC.GetTotalMemory(false) / 1024} KB";
            _labels[11].Text = $"{_fps.UpdateFramesPerSecond}/{_fps.DrawFramesPerSecond}";
            _labels[13].Text = $"{gameWindow.ClientBounds.Width}x{gameWindow.ClientBounds.Height}";
            _labels[15].Text = $"{_viewportAdapter.Viewport.Width}x{_viewportAdapter.Viewport.Height}";
            _labels[17].Text = $"{graphicsDevice.DisplayMode.Width}x{graphicsDevice.DisplayMode.Height}";
            _labels[19].Text = $"{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width}x{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height}";
            _labels[21].Text = $"{camera.Zoom}";
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

        private Label CreateControl(Vector2 position, Alignment contentAlignment, string text, string name)
        {
            var size = new Vector2(160.0f, 20.0f);
            var control = new LabelSized(position, Alignment.TopLeft, size, contentAlignment, text, "CrimsonText-Regular-12", Color.LawnGreen, name, Color.DarkRed, Color.DarkSlateGray * Constants.ONE_HALF, Color.White);

            return control;
        }
    }
}