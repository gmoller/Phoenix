using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Button : Control
    {
        public Label Label { get; set; }

        public Button(string name, Vector2 position, Alignment positionAlignment, Vector2 size, string textureAtlas, string textureNormal, string textureActive, string textureDisabled, string textureHover, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, positionAlignment, size, textureAtlas, textureNormal, textureNormal, textureActive, textureHover, textureDisabled, layerDepth, parent)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }

        protected override void AfterUpdate(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            Label?.Update(input, deltaTime);
        }

        protected override void Draw(SpriteBatch spriteBatch, Matrix? transform = null)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }

        protected override void AfterDraw(Matrix? transform = null)
        {
            Label?.Draw();
        }
    }
}