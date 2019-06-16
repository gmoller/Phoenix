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
        private Label _lblFps1;
        private Label _lblFps2;
        private Label _lblMemory1;
        private Label _lblMemory2;

        public MainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            SetGraphicsResolution(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
            _fps = new FramesPerSecondCounter();

            base.Initialize();
        }

        private void SetGraphicsResolution(int width, int height)
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = width;
            _graphicsDeviceManager.PreferredBackBufferHeight = height;
            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentLoader.LoadContent(GraphicsDevice);
            var font = AssetsManager.Instance.GetSpriteFont("TimesSanSerif");

            Vector2 size = font.MeasureString("MEM:");
            Vector2 pos = new Vector2(0.0f, _graphicsDeviceManager.GraphicsDevice.Viewport.Height);

            _lblMemory1 = new Label(font, pos, HorizontalAlignment.Left, VerticalAlignment.Bottom, size, "MEM:", Color.LawnGreen, Color.DarkRed, Color.White);
            _lblMemory2 = new Label(font, _lblMemory1, HorizontalAlignment.Right, VerticalAlignment.Middle, new Vector2(size.X + 50.0f, size.Y), string.Empty, Color.LawnGreen, Color.DarkRed, Color.White);

            _lblFps1 = new Label(font, _lblMemory1, HorizontalAlignment.Center, VerticalAlignment.Top, size, "FPS:", Color.LawnGreen, Color.DarkRed, Color.White);
            _lblFps2 = new Label(font, _lblFps1, HorizontalAlignment.Right, VerticalAlignment.Middle, size, string.Empty, Color.LawnGreen, Color.DarkRed, Color.White);
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
            _lblFps2.Text = $"{_fps.FramesPerSecond}";
            _lblMemory2.Text = $"{GC.GetTotalMemory(false) / 1024} KB";

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _lblFps1.Draw(_spriteBatch);
            _lblFps2.Draw(_spriteBatch);
            _lblMemory1.Draw(_spriteBatch);
            _lblMemory2.Draw(_spriteBatch);

            _spriteBatch.End();

            _fps.Draw();

            base.Draw(gameTime);
        }
    }
}