using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Image : Control
    {
        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureName, float layerDepth = 0.0f, IControl parent = null) : 
            this(name, position, alignment, size, string.Empty, textureName, layerDepth, parent)
        {
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, string textureName, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, alignment, size, textureAtlas, textureName, layerDepth, parent)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            if (TextureAtlas.HasValue())
            {
                Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
                var atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);
                SourceRectangle = atlas.Frames[TextureName].ToRectangle();
            }
            else
            {
                Texture = AssetsManager.Instance.GetTexture(TextureName);
                SourceRectangle = Texture.Bounds;
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