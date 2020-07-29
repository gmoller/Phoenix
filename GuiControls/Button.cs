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

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            base.Update(input, deltaTime, transform);

            Label?.Update(input, deltaTime);
        }

        public override void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);

            EndSpriteBatch(spriteBatch);

            Label?.Draw();
        }
    }
}