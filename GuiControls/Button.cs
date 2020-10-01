using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Button : ControlWithMultipleTextures
    {
        /// <summary>
        /// Use this constructor if Button is to be used as a child of another control.
        /// When a control is a child of another control, it's position will be relative
        /// to the parent control. Therefore there is no need to pass in a position.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="textureNormal"></param>
        /// <param name="textureActive"></param>
        /// /// <param name="textureHover"></param>
        /// <param name="textureDisabled"></param>
        /// <param name="layerDepth"></param>
        public Button(
            string name,
            Vector2 size,
            string textureNormal,
            string textureActive,
            string textureHover,
            string textureDisabled,
            float layerDepth = 0.0f) :
            this(
                Vector2.Zero,
                Alignment.TopLeft,
                size,
                textureNormal,
                textureActive,
                textureHover,
                textureDisabled,
                name,
                layerDepth)
        {
        }

        /// <summary>
        /// Use this constructor if Button is expected to be stand alone (have no parent).
        /// </summary>
        /// <param name="position"></param>
        /// <param name="positionAlignment"></param>
        /// <param name="size"></param>
        /// <param name="textureNormal"></param>
        /// <param name="textureActive"></param>
        /// /// <param name="textureHover"></param>
        /// <param name="textureDisabled"></param>
        /// <param name="name"></param>
        public Button(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureNormal,
            string textureActive,
            string textureHover,
            string textureDisabled,
            string name) :
            this(
                position,
                positionAlignment,
                size,
                textureNormal,
                textureActive,
                textureHover,
                textureDisabled,
                name,
                0.0f)
        {
        }

        private Button(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureNormal,
            string textureActive,
            string textureHover,
            string textureDisabled,
            string name,
            float layerDepth = 0.0f) :
            base(
                position,
                positionAlignment,
                size,
                textureNormal,
                textureNormal,
                textureActive,
                textureHover,
                textureDisabled,
                name,
                layerDepth)
        {
        }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}