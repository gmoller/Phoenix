using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGamePresentationLibrary;
using Utilities;
using Microsoft.Xna.Framework.Input;

namespace Phoenix
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

        private PhoenixGame _phoenixGame;
        private PhoenixGameView _phoenixGameView;
        private MetricsPanel _metricsPanel;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Logger.Instance.Log("Initializing...");

            Window.Position = new Microsoft.Xna.Framework.Point(0, 0);
            VariableTimeStep();
            SetGraphicsResolution(1920, 1080); // 1600, 800
            DeviceManager.Instance.GraphicsDevice = GraphicsDevice;

            _phoenixGame = new PhoenixGame();
            _phoenixGameView = new PhoenixGameView(_phoenixGame);

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

            DeviceManager.Instance.SetCurrentSpriteBatch(new SpriteBatch(GraphicsDevice));

            _phoenixGameView.LoadContent(GraphicsDevice, Content);

            _metricsPanel = new MetricsPanel(new Vector2(0.0f, 200.0f));

            Logger.Instance.LogComplete();
        }

        protected override void UnloadContent()
        {
            Logger.Instance.Log("Unloading content...");

            DeviceManager.Instance.DisposeSpriteBatches();

            Logger.Instance.LogComplete();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _phoenixGame.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            _phoenixGameView.Update(gameTime); // here for controls updates
            _metricsPanel.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _phoenixGameView.Draw(DeviceManager.Instance.GetCurrentSpriteBatch());
            _metricsPanel.Draw();

            base.Draw(gameTime);
        }
    }
}