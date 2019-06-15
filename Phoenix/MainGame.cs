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
            _lblMemory = new Label(font, new Vector2(0.0f, _graphicsDeviceManager.GraphicsDevice.Viewport.Height), VerticalAlignment.Bottom, HorizontalAlignment.Left, "MEM: ", Color.LawnGreen, 0.5f) { TextShadow = true, TextShadowColor = Color.DarkRed };
            _lblFps = new Label(font, _lblMemory.TopLeft, VerticalAlignment.Bottom, HorizontalAlignment.Left, "FPS: ", Color.LawnGreen, 0.5f) { TextShadow = true, TextShadowColor = Color.DarkRed };
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