using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhoenixGameLibrary;
using PhoenixGamePresentation;
using PhoenixGamePresentation.Views;
using Utilities;
 
namespace Phoenix2
{
    public class MainGame : Game
    {
        #region State
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private SpriteBatch _spriteBatch;

        private PhoenixGame _phoenixGame;
        private PhoenixGameView _phoenixGameView;
        private MetricsPanel _metricsPanel;
        #endregion

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Logger.Instance.Log("MainGame Initializing...");

            Window.Position = new Microsoft.Xna.Framework.Point(0, 0);
            VariableTimeStep();

            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");

            context.GraphicsDevice = GraphicsDevice;
            context.GraphicsDeviceManager = _graphicsDeviceManager;
            context.GameWindow = Window;

            if (context.DesiredResolution == PointI.Empty)
            {
                SetScreenResolution(_graphicsDeviceManager, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            }
            else
            {
                SetScreenResolution(_graphicsDeviceManager, context.DesiredResolution.X, context.DesiredResolution.Y);
            }

            _phoenixGame = new PhoenixGame();
            _phoenixGameView = new PhoenixGameView(_phoenixGame);
            _metricsPanel = new MetricsPanel();

            Logger.Instance.LogComplete();

            base.Initialize();
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

        private void SetScreenResolution(GraphicsDeviceManager graphicsDeviceManager, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");

            graphicsDeviceManager.PreferredBackBufferWidth = width;
            graphicsDeviceManager.PreferredBackBufferHeight = height;
            //graphicsDeviceManager.IsFullScreen = true;
            graphicsDeviceManager.ApplyChanges();

            var actualWidth = graphicsDeviceManager.GraphicsDevice.Viewport.Width;
            var actualHeight = graphicsDeviceManager.GraphicsDevice.Viewport.Height;
            context.ActualResolution = new PointI(actualWidth, actualHeight);
            context.ScreenRatio = new PointF(actualWidth / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, actualHeight / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        }

        protected override void LoadContent()
        {
            Logger.Instance.Log("Loading content...");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _phoenixGameView.LoadContent(GraphicsDevice, Content);
            _metricsPanel.LoadContent(Content);

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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            _phoenixGame.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            _phoenixGameView.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds); // here for controls updates
            _metricsPanel.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.DarkBlue);

            _phoenixGameView.Draw(_spriteBatch);
            _metricsPanel.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}