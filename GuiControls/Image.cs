﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Image : Control
    {
        public Image(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureAtlas,
            string textureName,
            IControl parent = null,
            string name = "") :
            this(
                position,
                positionAlignment,
                size,
                textureAtlas,
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
            float layerDepth, 
            IControl parent = null, 
            string name = "") :
            base(
                position, 
                positionAlignment, 
                size, 
                textureAtlas, 
                textureName, 
                textureName, 
                textureName, 
                textureName, 
                textureName, 
                layerDepth, 
                parent,
                name)
        {
        }

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

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            base.Update(input, deltaTime, transform);
        }

        protected override void Draw(SpriteBatch spriteBatch, Matrix? transform = null)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}