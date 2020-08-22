using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Slot : Control
    {
        public Slot(
            string name,
            Vector2 size,
            string textureAtlas,
            string textureName) :
            this(
                Vector2.Zero,
                size,
                textureAtlas,
                textureName,
                name)
        {
        }

        private Slot(
            Vector2 position,
            Vector2 size,
            string textureAtlas,
            string textureName,
            string name) :
            base(
                position,
                Alignment.TopLeft,
                size,
                textureAtlas,
                textureName,
                textureName,
                textureName,
                textureName,
                textureName,
                name)
        {
        }

        protected Slot(Slot copyThis) : base(copyThis)
        {
        }

        public override IControl Clone() { return new Slot(this); }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}