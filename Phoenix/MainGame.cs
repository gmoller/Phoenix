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

        private InputHandler _input;
        private PhoenixGame _phoenixGame;
        private MetricsPanel _metricsPanel;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Logger.Instance.Log("Initializing...");

            Window.Position = new Point(0, 0);
            VariableTimeStep();
            SetGraphicsResolution(1920, 1080); // 1600, 800
            DeviceManager.Instance.GraphicsDevice = GraphicsDevice;

            _input = new InputHandler();
            _input.Initialize();
            _phoenixGame = new PhoenixGame();

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

            ContentLoader.LoadContent(GraphicsDevice);
            AssetsManager.Instance.ContentManager = Content;
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-12", "Fonts\\Maleficio-Regular-12");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-18", "Fonts\\Maleficio-Regular-18");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-24", "Fonts\\Maleficio-Regular-24");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-36", "Fonts\\Maleficio-Regular-36");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-48", "Fonts\\Maleficio-Regular-48");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-60", "Fonts\\Maleficio-Regular-60");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-72", "Fonts\\Maleficio-Regular-72");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-12", "Fonts\\Carolingia-Regular-12");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-18", "Fonts\\Carolingia-Regular-18");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-24", "Fonts\\Carolingia-Regular-24");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-36", "Fonts\\Carolingia-Regular-36");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-48", "Fonts\\Carolingia-Regular-48");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-60", "Fonts\\Carolingia-Regular-60");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-72", "Fonts\\Carolingia-Regular-72");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-12", "Fonts\\CrimsonText-Regular-12");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-18", "Fonts\\CrimsonText-Regular-18");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-24", "Fonts\\CrimsonText-Regular-24");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-36", "Fonts\\CrimsonText-Regular-36");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-48", "Fonts\\CrimsonText-Regular-48");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-60", "Fonts\\CrimsonText-Regular-60");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-72", "Fonts\\CrimsonText-Regular-72");

            AssetsManager.Instance.AddTexture("Cursor", "Textures\\cursor");
            AssetsManager.Instance.AddTexture("VillageSmall00", "Textures\\villageSmall00");

            AssetsManager.Instance.AddTexture("terrain_hextiles_basic_1", "TextureAtlases\\terrain_hextiles_basic_1");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_basic_1", "TextureAtlases\\terrain_hextiles_basic_1");
            AssetsManager.Instance.AddTexture("terrain_hextiles_basic_2", "TextureAtlases\\terrain_hextiles_basic_2");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_basic_2", "TextureAtlases\\terrain_hextiles_basic_2");
            AssetsManager.Instance.AddTexture("terrain_hextiles_cold_1", "TextureAtlases\\terrain_hextiles_cold_1");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_cold_1", "TextureAtlases\\terrain_hextiles_cold_1");
            AssetsManager.Instance.AddTexture("terrain_hextiles_cold_2", "TextureAtlases\\terrain_hextiles_cold_2");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_cold_2", "TextureAtlases\\terrain_hextiles_cold_2");

            AssetsManager.Instance.AddTexture("GUI_Textures_1", "TextureAtlases\\GUI_Textures_1");
            AssetsManager.Instance.AddAtlas("GUI_Textures_1", "TextureAtlases\\GUI_Textures_1");

            AssetsManager.Instance.AddTexture("Icons_1", "TextureAtlases\\Icons_1");
            AssetsManager.Instance.AddAtlas("Icons_1", "TextureAtlases\\Icons_1");

            AssetsManager.Instance.AddTexture("Buildings", "TextureAtlases\\Buildings");
            AssetsManager.Instance.AddAtlas("Buildings", "TextureAtlases\\Buildings");

            _metricsPanel = new MetricsPanel(new Vector2(0.0f, 200.0f));

            _phoenixGame.LoadContent(Content);

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
            try
            {
                _input.Update(gameTime);
                if (_input.Exit) Exit();

                _phoenixGame.Update(gameTime, _input);
                _metricsPanel.Update(gameTime, _input);

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
            //try
            //{
                GraphicsDevice.Clear(Color.Black);

                _phoenixGame.Draw();
                _metricsPanel.Draw();

                base.Draw(gameTime);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Instance.LogError(ex);
            //    throw ex;
            //}
        }
    }
}