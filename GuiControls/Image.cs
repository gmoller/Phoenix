using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Image : Control
    {
        /// <summary>
        /// Use this constructor if Image is to be used as a child of another control.
        /// When a control is a child of another control, it's position will be relative
        /// to the parent control. Therefore there is no need to pass in a position.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="textureAtlas"></param>
        /// <param name="textureName"></param>
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

        /// <summary>
        /// Use this constructor if Image is expected to be stand alone (have no parent).
        /// </summary>
        /// <param name="position"></param>
        /// <param name="positionAlignment"></param>
        /// <param name="size"></param>
        /// <param name="textureAtlas"></param>
        /// <param name="textureName"></param>
        /// <param name="name"></param>
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

        /// <summary>
        /// Use this constructor if Image is expected to be stand alone (have no parent).
        /// </summary>
        /// <param name="position"></param>
        /// <param name="positionAlignment"></param>
        /// <param name="size"></param>
        /// <param name="textureName"></param>
        /// <param name="name"></param>
        /// <param name="layerDepth"></param>
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

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}