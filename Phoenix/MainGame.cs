using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AssetsLibrary;
using Input;
using PhoenixGameLibrary;
using Utilities;

namespace Phoenix
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        private MetricsPanel _metricsPanel;
        private PhoenixGame _game;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Logger.Instance.Log("Initializing...");

            VariableTimeStep();
            SetGraphicsResolution(1600, 800);
            DeviceManager.Instance.GraphicsDevice = GraphicsDevice;
            DeviceManager.Instance.IsMouseVisible = IsMouseVisible;
            KeyboardHandler.Initialize();
            MouseHandler.Initialize();
            _game = new PhoenixGame();

            Logger.Instance.LogComplete();

            base.Initialize();
        }

        private void SetGraphicsResolution(int width, int height)
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = width;
            _graphicsDeviceManager.PreferredBackBufferHeight = height;
            //_graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.ApplyChanges();
        }

        private void VariableTimeStep()
        {
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        private void FixedTimeStep()
        {
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60); // 60fps
        }

        protected override void LoadContent()
        {
            Logger.Instance.Log("Loading content...");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentLoader.LoadContent(GraphicsDevice);
            //_metricsPanel = new MetricsPanel(new Vector2(0.0f, _graphicsDeviceManager.GraphicsDevice.Viewport.Height));
            _metricsPanel = new MetricsPanel(new Vector2(0.0f, 200.0f));

            Logger.Instance.LogComplete();
        }

        protected override void UnloadContent()
        {
            Logger.Instance.Log("Unloading content...");

            _spriteBatch.Dispose();

            Logger.Instance.LogComplete();
        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                KeyboardHandler.Update();
                MouseHandler.Update();

                if (KeyboardHandler.IsKeyDown(Keys.Escape)) Exit();

                _game.Update(gameTime);
                _metricsPanel.Update(gameTime);

                base.Update(gameTime);
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError(ex);
                throw ex;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.Black);

                _game.Draw(_spriteBatch);
                _metricsPanel.Draw(_spriteBatch);

                base.Draw(gameTime);
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError(ex);
                throw ex;
            }
        }
    }
}