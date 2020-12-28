using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Zen.GuiControls;
using Zen.GuiControls.TheControls;
using Zen.MonoGameUtilities;
using Zen.MonoGameUtilities.ViewportAdapters;
using Zen.Utilities;

namespace PhoenixGamePresentation
{
    public class MetricsPanel
    {
        #region State
        private FramesPerSecondCounter _fps;
        private Controls Controls { get; }

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;
        #endregion

        public MetricsPanel()
        {
            var spec = ResourceReader.ReadResource("PhoenixGamePresentation.MetricsPanelControls.txt", Assembly.GetExecutingAssembly());
            Controls = ControlCreator.CreateFromSpecification(spec);
            Controls.SetOwner(this);

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
            Controls.LoadContent(content, true);
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

            ((Label)Controls["lblFps2"]).Text = $"{_fps.UpdateFramesPerSecond}/{_fps.DrawFramesPerSecond}";
            ((Label)Controls["lblGcCount2"]).Text = $"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}";
            ((Label)Controls["lblMemory2"]).Text = $"{(GC.GetTotalMemory(false) / 1024):N0} KB";
            ((Label)Controls["lblScreenPosition2"]).Text = $"({mouseState.Position.X},{mouseState.Position.Y})";
            ((Label)Controls["lblWorldPosition2"]).Text = $"({worldPosition.X:F0},{worldPosition.Y:F0})";
            ((Label)Controls["lblWorldHex2"]).Text = $"{worldHex.Col},{worldHex.Row}";
            ((Label)Controls["lblZoom2"]).Text = $"{camera.Zoom}";

            //((Label)Controls["lblResolution12"]).Text = $"{gameWindow.ClientBounds.Width}x{gameWindow.ClientBounds.Height}";
            //((Label)Controls["lblResolution22"]).Text = $"{_viewportAdapter.Viewport.Width}x{_viewportAdapter.Viewport.Height}";
            //((Label)Controls["lblResolution32"]).Text = $"{graphicsDevice.DisplayMode.Width}x{graphicsDevice.DisplayMode.Height}";
            //((Label)Controls["lblResolution42"]).Text = $"{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width}x{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height}";

            //("lblResolution11", "ClientBounds:"),
            //("lblResolution21", "Viewport:"),
            //("lblResolution31", "DisplayMode:"),
            //("lblResolution41", "CurrentDisplayMode:")
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = _viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: _viewportAdapter.GetScaleMatrix());

            Controls.Draw(spriteBatch);
            _fps.Draw();

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }
    }
}