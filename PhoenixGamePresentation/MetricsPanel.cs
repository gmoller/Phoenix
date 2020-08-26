using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using Utilities;
using Color = Microsoft.Xna.Framework.Color;

namespace PhoenixGamePresentation
{
    public class MetricsPanel
    {
        private readonly Vector2 _position;

        private FramesPerSecondCounter _fps;

        private readonly List<Label> _labels;

        private readonly Stack<Viewport> _viewports;

        public MetricsPanel(Vector2 position)
        {
            _position = position;
            _viewports = new Stack<Viewport>();

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
            var y = -30.0f;
            for (var i = 0; i < metrics.Count; i++)
            {
                var x = i % 2 == 0 ? 0.0f : 160.0f;
                if (i % 2 == 0)
                {
                    y += 30.0f;
                }
                var pos = new Vector2(x, y);
                var contentAlignment = i % 2 == 0 ? Alignment.MiddleLeft : Alignment.MiddleRight;
                var label = CreateControl(pos, contentAlignment, metrics[i].text, metrics[i].name);
                _labels.Add(label);
            }
        }

        public void LoadContent(ContentManager content)
        {
            _fps = new FramesPerSecondCounter();

            foreach (var label in _labels)
            {
                label.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime, ViewportAdapter viewportAdapter)
        {
            var context = CallContext<GlobalContext>.GetData("AmbientGlobalContext");
            var gameWindow = (GameWindow)context.GameWindow;
            var graphicsDevice = (GraphicsDevice)context.GraphicsDevice;

            _fps.Update(gameTime);

            var mouseState = Mouse.GetState();

            _labels[1].Text = $"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}";
            _labels[3].Text = $"({mouseState.Position.X},{mouseState.Position.Y})";
            _labels[5].Text = $"({context.WorldPositionPointedAtByMouseCursor.X},{context.WorldPositionPointedAtByMouseCursor.Y})";
            _labels[7].Text = $"{context.WorldHexPointedAtByMouseCursor.X},{context.WorldHexPointedAtByMouseCursor.Y}";
            _labels[9].Text = $"{GC.GetTotalMemory(false) / 1024} KB";
            _labels[11].Text = $"{_fps.UpdateFramesPerSecond}/{_fps.DrawFramesPerSecond}";
            _labels[13].Text = $"{gameWindow.ClientBounds.Width}x{gameWindow.ClientBounds.Height}";
            _labels[15].Text = $"{viewportAdapter.Viewport.Width}x{viewportAdapter.Viewport.Height}";
            _labels[17].Text = $"{graphicsDevice.DisplayMode.Width}x{graphicsDevice.DisplayMode.Height}";
            _labels[19].Text = $"{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width}x{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height}";
            //_labels[21].Text = $"{context.Zoom}";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SetViewport(GetViewport());

            foreach (var label in _labels)
            {
                label.Draw(spriteBatch);
            }

            ResetViewport();

            _fps.Draw();
        }

        private Label CreateControl(Vector2 position, Alignment contentAlignment, string text, string name)
        {
            var size = new Vector2(160.0f, 20.0f);
            var control = new LabelSized(position, Alignment.TopLeft, size, contentAlignment, text, "CrimsonText-Regular-12", Color.LawnGreen, name, Color.DarkRed, Color.DarkSlateGray * Constants.ONE_HALF, Color.White);

            return control;
        }

        private void SetViewport(Viewport newViewport)
        {
            var context = CallContext<GlobalContext>.GetData("AmbientGlobalContext");
            var graphicsDevice = (GraphicsDevice)context.GraphicsDevice;

            _viewports.Push(graphicsDevice.Viewport);
            graphicsDevice.Viewport = newViewport;
        }

        private void ResetViewport()
        {
            var context = CallContext<GlobalContext>.GetData("AmbientGlobalContext");
            var graphicsDevice = (GraphicsDevice)context.GraphicsDevice;

            var previousViewport = _viewports.Pop();
            graphicsDevice.Viewport = previousViewport;
        }

        private Viewport GetViewport()
        {
            var context = CallContext<GlobalContext>.GetData("AmbientGlobalContext");
            var graphicsDevice = (GraphicsDevice)context.GraphicsDevice;

            return new Viewport(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y, 300, 201, 0, 1);
        }
    }
}