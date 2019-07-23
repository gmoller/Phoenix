using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;

namespace GuiControls
{
    public class Image
    {
        private readonly Texture2D _texture;
        private readonly Rectangle _destinationRectangle;
        private readonly Rectangle _sourceRectangle;
        private readonly Color _color;
        private readonly float _layerDepth;

        public Image(string name, Vector2 topLeftPosition, Texture2D texture, float layerDepth = 0.0f)
        {
            _texture = texture;
            _sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            _destinationRectangle = new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, texture.Width, texture.Height);
            _color = Color.White;
            _layerDepth = layerDepth;
        }

        public Image(string name, Vector2 topLeftPosition, string textureAtlas, string textureName, float layerDepth = 0.0f)
        {
            _texture = AssetsManager.Instance.GetTexture(textureAtlas);
            var spec = AssetsManager.Instance.GetAtlas(textureAtlas);
            _sourceRectangle = spec.Frames[textureName].ToRectangle();
            _destinationRectangle = new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, _sourceRectangle.Width, _sourceRectangle.Height);
            _color = Color.White;
            _layerDepth = layerDepth;
        }

        public Image(string name, Vector2 topLeftPosition, Vector2 size, Texture2D texture, float layerDepth = 0.0f)
        {
            _texture = texture;
            _sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            _destinationRectangle = new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, (int)size.X, (int)size.Y);
            _color = Color.White;
            _layerDepth = layerDepth;
        }

        public Image(string name, Vector2 topLeftPosition, Vector2 size, string textureAtlas, string textureName, float layerDepth = 0.0f)
        {
            _texture = AssetsManager.Instance.GetTexture(textureAtlas);
            var spec = AssetsManager.Instance.GetAtlas(textureAtlas);
            _sourceRectangle = spec.Frames[textureName].ToRectangle();
            _destinationRectangle = new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, (int)size.X, (int)size.Y);
            _color = Color.White;
            _layerDepth = layerDepth;
        }

        public virtual void Draw(SpriteBatch spriteBatch = null)
        {
            bool newSpriteBatch = spriteBatch == null;
            spriteBatch = BeginSpriteBatch(spriteBatch, newSpriteBatch);

            spriteBatch.Draw(_texture, _destinationRectangle, _sourceRectangle, _color, 0.0f, Vector2.Zero, SpriteEffects.None, _layerDepth);

            EndSpriteBatch(spriteBatch, newSpriteBatch);
        }

        private SpriteBatch BeginSpriteBatch(SpriteBatch spriteBatch, bool newSpriteBatch)
        {
            if (newSpriteBatch)
            {
                var shinyNewSpriteBatch = DeviceManager.Instance.GetNewSpriteBatch();
                shinyNewSpriteBatch.Begin();
                return shinyNewSpriteBatch;
            }

            return spriteBatch;
        }

        private void EndSpriteBatch(SpriteBatch spriteBatch, bool newSpriteBatch)
        {
            if (newSpriteBatch)
            {
                spriteBatch.End();
                DeviceManager.Instance.ReturnSpriteBatchToPool(spriteBatch);
            }
        }
    }
}