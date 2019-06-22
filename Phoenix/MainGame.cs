using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using PhoenixGameLibrary;
using Utilities;

namespace Phoenix
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        private InputHandler _input;
        private MetricsPanel _metricsPanel;
        private PhoenixGame _game;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Logger.Instance.Log("Initializing...");

            VariableTimeStep();
            SetGraphicsResolution(1600, 800);
            DeviceManager.Instance.GraphicsDevice = GraphicsDevice;
            DeviceManager.Instance.IsMouseVisible = IsMouseVisible;

            _input = new InputHandler();
            _input.Initialize();
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
            AssetsManager.Instance.ContentManager = Content;
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-12", "Fonts\\CrimsonText-Regular-12");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-18", "Fonts\\CrimsonText-Regular-18");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-24", "Fonts\\CrimsonText-Regular-24");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-36", "Fonts\\CrimsonText-Regular-36");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-48", "Fonts\\CrimsonText-Regular-48");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-60", "Fonts\\CrimsonText-Regular-60");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-72", "Fonts\\CrimsonText-Regular-72");
            _metricsPanel = new MetricsPanel(new Vector2(0.0f, 200.0f));

            _game.LoadContent(Content);

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
                _input.Update(gameTime);
                if (_input.Exit) Exit();

                _game.Update(gameTime, _input);
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