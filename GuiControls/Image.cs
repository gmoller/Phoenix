using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Image : Control
    {
        public Image(
            string name,
            Vector2 size,
            string textureAtlas,
            string textureName) :
            this(
                Vector2.Zero, 
                Alignment.TopLeft,
                size,
                textureAtlas,
                textureName,
                textureName,
                0.0f,
                null,
                name)
        {
        }

        public Image(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureAtlas,
            string textureName,
            string name,
            IControl parent = null) :
            this(
                position,
                positionAlignment,
                size,
                textureAtlas,
                textureName,
                textureName,
                0.0f,
                parent,
                name)
        {
        }

        public Image(
            Vector2 position, 
            Alignment positionAlignment, 
            Vector2 size, 
            string textureName, 
            float layerDepth = 0.0f, 
            IControl parent = null, 
            string name = "") : 
            this(
                position,
                positionAlignment,
                size,
                string.Empty,
                textureName,
                textureName,
                layerDepth,
                parent,
                name)
        {
        }

        public Image(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureAtlas,
            string textureName,
            string textureNormal,
            float layerDepth,
            IControl parent = null,
            string name = "") :
            base(
                position,
                positionAlignment,
                size,
                textureAtlas,
                textureName,
                textureNormal,
                textureName,
                textureName,
                textureName,
                name,
                layerDepth,
                parent)
        {
        }

        protected Image(Image copyThis) : base(copyThis)
        {
        }

        public override IControl Clone() { return new Image(this); }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            //if (TextureAtlas.HasValue())
            //{
            //    var atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);
            //    Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
            //    SourceRectangle = atlas.Frames[TextureName].ToRectangle();
            //}
            //else
            //{
            //    Texture = AssetsManager.Instance.GetTexture(TextureName);
            //    SourceRectangle = Texture.Bounds;
            //}
        }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}