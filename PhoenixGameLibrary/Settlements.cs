using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;

namespace PhoenixGameLibrary
{
    public class Settlements
    {
        private readonly Camera _camera;
        private Texture2D _texture;

        public Settlements(Camera camera)
        {
            _camera = camera;
        }

        public void LoadContent(ContentManager content)
        {
            AssetsManager.Instance.AddTexture("VillageSmall00", "Textures\\villageSmall00");
            _texture = AssetsManager.Instance.GetTexture("VillageSmall00");
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var position = World.CalculateWorldPosition(6, 6, _camera);
            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);

            var original = DeviceManager.Instance.GraphicsDevice.Viewport;
            DeviceManager.Instance.GraphicsDevice.Viewport = DeviceManager.Instance.MapViewport;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, _camera.Transform);
            spriteBatch.Draw(_texture, position, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, Constants.HEX_SCALE, SpriteEffects.None, 0.0f);
            spriteBatch.End();

            DeviceManager.Instance.GraphicsDevice.Viewport = original;
        }
    }
}