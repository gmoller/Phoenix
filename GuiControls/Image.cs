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
                name,
                0.0f)
        {
        }

        public Image(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureAtlas,
            string textureName,
            string name) :
            this(
                position,
                positionAlignment,
                size,
                textureAtlas,
                textureName,
                textureName,
                name,
                0.0f)
        {
        }

        public Image(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureName,
            string name = null,
            float layerDepth = 0.0f) :
            this(
                position,
                positionAlignment,
                size,
                string.Empty,
                textureName,
                textureName,
                name,
                layerDepth)
        {
        }

        private Image(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureAtlas,
            string textureName,
            string textureNormal,
            string name,
            float layerDepth) :
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
                layerDepth)
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