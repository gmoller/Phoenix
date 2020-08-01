using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Input;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Button : Control
    {
        public Label Label { get; set; }

        public Button(
            Vector2 position, 
            Alignment positionAlignment,
            Vector2 size, 
            string textureAtlas, 
            string textureNormal, 
            string textureActive, 
            string textureDisabled, 
            string textureHover, 
            float layerDepth = 0.0f, 
            IControl parent = null, 
            string name = "") :
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
                layerDepth, 
                parent,
                name)
        {
        }

        protected override void AfterUpdate(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            Label?.Update(input, deltaTime);
        }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }

        protected override void AfterDraw(SpriteBatch spriteBatch)
        {
            Label?.Draw(spriteBatch);
        }

        protected override void AfterDraw(Matrix? transform = null)
        {
            Label?.Draw(transform);
        }
    }
}