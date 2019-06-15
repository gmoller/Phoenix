using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AssetsLibrary;
using GuiControls;

namespace Phoenix
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        private FramesPerSecondCounter _fps;
        private Label _lblFps;
        private Label _lblMemory;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _fps = new FramesPerSecondCounter();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentLoader.LoadContent(GraphicsDevice);
            var font = AssetsManager.Instance.GetSpriteFont("TimesSanSerif");
            Vector2 size = font.MeasureString("MEM: 2400 KB");
            Vector2 pos = new Vector2(0.0f, _graphicsDeviceManager.GraphicsDevice.Viewport.Height);
            _lblMemory = new Label(font, pos, VerticalAlignment.Bottom, HorizontalAlignment.Left, size, "MEM: ", Color.LawnGreen, Color.DarkRed, Color.White, 0.5f);
            _lblFps = new Label(font, _lblMemory, VerticalAlignment.Top, HorizontalAlignment.Left, size, "FPS: ", Color.LawnGreen, Color.DarkRed, Color.White, 0.5f);
        }

        protected override void UnloadContent()
        {
            _spriteBatch.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            _fps.Update(gameTime);
            _lblFps.Text = $"FPS: {_fps.FramesPerSecond}";
            _lblMemory.Text = $"MEM: {GC.GetTotalMemory(false) / 1024} KB";

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _lblFps.Draw(_spriteBatch);
            _lblMemory.Draw(_spriteBatch);

            _spriteBatch.End();

            _fps.Draw();

            base.Draw(gameTime);
        }
    }
}