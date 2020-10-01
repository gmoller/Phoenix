using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Slot : ControlWithSingleTexture
    {
        /// <summary>
        /// Use this constructor if Slot is to be used as a child of another control.
        /// When a control is a child of another control, it's position will be relative
        /// to the parent control. Therefore there is no need to pass in a position.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="textureName"></param>
        public Slot(
            string name,
            Vector2 size,
            string textureName) :
            this(
                Vector2.Zero,
                size,
                textureName,
                name)
        {
        }

        private Slot(
            Vector2 position,
            Vector2 size,
            string textureName,
            string name) :
            base(
                position,
                Alignment.TopLeft,
                size,
                textureName,
                name)
        {
        }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}