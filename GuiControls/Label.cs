using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    public class Label : Control
    {
        private readonly SpriteFont _font;
        private readonly Color _textColor;
        private readonly Color? _textShadowColor;
        private readonly Color? _borderColor;

        public string Text { get; set; }

        public Label(SpriteFont font, Vector2 position, Vector2 size, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, string text, Color textColor, Color? textShadowColor = null, Color? borderColor = null, float scale = 1.0f) :
            base(position, size, verticalAlignment, horizontalAlignment, scale)
        {
            _font = font;
            Text = text;
            _textColor = textColor;
            _textShadowColor = textShadowColor;
            _borderColor = borderColor;
        }

        public void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = TopLeft;

            if (_textShadowColor != null)
            {
                spriteBatch.DrawString(_font, Text, position + Vector2.One, _textShadowColor.Value, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0.0f);
            }

            if (_borderColor != null)
            {
                spriteBatch.DrawRectangle(Area, _borderColor.Value);
            }

            spriteBatch.DrawString(_font, Text, position, _textColor, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0.0f);
        }
    }
}