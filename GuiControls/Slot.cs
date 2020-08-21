using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Slot : Control
    {
        public Slot(
            int x,
            int y,
            int width,
            int height,
            string textureAtlas,
            string textureName,
            string name,
            IControl parent) :
            base(
                new Vector2(x, y),
                Alignment.TopLeft,
                new Vector2(width, height),
                textureAtlas,
                textureName,
                textureName,
                textureName,
                textureName,
                textureName,
                name,
                0.0f,
                parent)
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