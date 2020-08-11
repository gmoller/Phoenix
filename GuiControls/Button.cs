using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Button : Control
    {
        public Button(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureAtlas,
            string textureNormal,
            string textureActive,
            string textureDisabled,
            string textureHover,
            string name,
            float layerDepth = 0.0f,
            IControl parent = null) :
            base(
                position,
                positionAlignment,
                size,
                textureAtlas,
                textureNormal,
                textureNormal,
                textureActive,
                textureHover,
                textureDisabled,
                name,
                layerDepth,
                parent)
        {
        }

        protected Button(Button copyThis) : base(copyThis)
        {
        }

        public override IControl Clone() { return new Button(this); }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}