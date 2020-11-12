using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Utilities.ExtensionMethods;

namespace GuiControls
{
    public abstract class ControlWithSingleTexture : Control
    {
        #region State
        protected string TextureAtlas { get; }
        protected string TextureName { get; }
        protected Color Color { get; }

        protected AtlasSpec2 Atlas { get; set; }
        protected Texture2D Texture { get; set; }
        protected Rectangle SourceRectangle { get; private set; }
        #endregion

        protected ControlWithSingleTexture(Vector2 position, Alignment positionAlignment, Vector2 size, string textureName, string name, float layerDepth = 0.0f)
            : base(position, positionAlignment, size, name, layerDepth)
        {
            if (textureName.HasValue())
            {
                TextureAtlas = ControlHelper.GetTextureAtlas(textureName);
                TextureName = ControlHelper.GetTextureName(textureName);
            }

            Color = Color.White;
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            if (TextureAtlas.HasValue())
            {
                Atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);
                Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
                SourceRectangle = Atlas.Frames[TextureName].ToRectangle();
            }
            else // no atlas
            {
                SetTexture(TextureName);
            }

            base.LoadContent(content, loadChildrenContent);
        }

        protected void SetTexture(string textureName)
        {
            if (Atlas.HasValue())
            {
                var f = Atlas.Frames[textureName];
                SourceRectangle = new Rectangle(f.X, f.Y, f.Width, f.Height);
            }
            else
            {
                if (textureName.HasValue())
                {
                    Texture = AssetsManager.Instance.GetTexture(textureName);
                    SourceRectangle = Texture.Bounds;
                }
            }
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
            SourceRectangle = Texture.Bounds;
        }
    }
}