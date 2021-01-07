using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PhoenixGameLibrary;
using PhoenixGamePresentation;
using Zen.Hexagons;
using Zen.Utilities;

namespace Phoenix2
{
    public class MainGame : Game
    {
        #region State
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private SpriteBatch _spriteBatch;

        private PhoenixGameView _phoenixGameView;
        private MetricsPanel _metricsPanel;
        #endregion

        public MainGame(PointI desiredResolution)
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            var gameMetadata = new GameMetadata();
            CallContext<GameMetadata>.SetData("GameMetadata", gameMetadata);
            var gameDataRepository = new GameDataRepository();
            CallContext<GameDataRepository>.SetData("GameDataRepository", gameDataRepository);

            var presentationContext = new GlobalContextPresentation();
            presentationContext.DesiredResolution = desiredResolution;
            CallContext<GlobalContextPresentation>.SetData("GlobalContextPresentation", presentationContext);
        }

        protected override void Initialize()
        {
            Logger.Instance.Log("MainGame Initializing...");

            Window.Position = new Point(0, 0);
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

            var world = PhoenixGame.MakeWorld();
            CallContext<World>.SetData("GameWorld", world);

            _phoenixGameView = new PhoenixGameView();
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
            context.ScreenRatio = new Point2F(actualWidth / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, actualHeight / (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        }

        protected override void LoadContent()
        {
            Logger.Instance.Log("Loading content...");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _phoenixGameView.LoadContent(Content);
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

            _phoenixGameView.Update(gameTime);
            _metricsPanel.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _phoenixGameView.Draw(_spriteBatch);
            _metricsPanel.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}