using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Image : Control
    {
        public Image(string name, Vector2 position, Vector2 size, string textureName, float layerDepth = 0.0f, IControl parent = null) : 
            this(name, position, ContentAlignment.TopLeft, size, string.Empty, textureName, null, layerDepth, parent)
        {
        }

        public Image(string name, Vector2 position, Vector2 size, string textureAtlas, string textureName, float layerDepth = 0.0f, IControl parent = null) : 
            this(name, position, ContentAlignment.TopLeft, size, textureAtlas, textureName, null, layerDepth, parent)
        {
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureName, float layerDepth = 0.0f, IControl parent = null) : 
            this(name, position, alignment, size, string.Empty, textureName, null, layerDepth, parent)
        {
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, byte textureId, float layerDepth = 0.0f, IControl parent = null) :
            this(name, position, alignment, size, textureAtlas, string.Empty, textureId, layerDepth, parent)
        {
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, string textureName, byte? textureId = null, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, alignment, size, textureAtlas, textureName, textureId, layerDepth, parent)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            if (string.IsNullOrEmpty(TextureAtlas))
            {
                Texture = AssetsManager.Instance.GetTexture(TextureName);
                SourceRectangle = Texture.Bounds;
            }
            else
            {
                Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
                var atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);
                SourceRectangle = TextureId == null ? atlas.Frames[TextureName].ToRectangle() : atlas.Frames[(int)TextureId].ToRectangle();
            }
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
        }

        public override void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);

            EndSpriteBatch(spriteBatch);
        }
    }
}