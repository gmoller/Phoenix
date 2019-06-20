using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

namespace GuiControls
{
    public class Label : Control
    {
        private readonly SpriteFont _font;
        private readonly Color _textColor;
        private readonly Color? _textShadowColor;
        private readonly Color? _borderColor;

        public string Text { get; set; }

        public Label(SpriteFont font, Vector2 position, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string text, Color textColor, Color? textShadowColor = null, Color? borderColor = null, Texture2D texture = null) :
            base(position, horizontalAlignment, verticalAlignment, size, texture)
        {
            _font = font;
            Text = text;
            _textColor = textColor;
            _textShadowColor = textShadowColor;
            _borderColor = borderColor;
        }

        public Label(SpriteFont font, Control controlToDockTo, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string text, Color textColor, Color? textShadowColor = null, Color? borderColor = null, Texture2D texture = null) :
            base(controlToDockTo, horizontalAlignment, verticalAlignment, size, texture)
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 position = TopLeft;

            if (_textShadowColor != null)
            {
                spriteBatch.DrawString(_font, Text, position + Vector2.One, _textShadowColor.Value, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }

            if (_borderColor != null)
            {
                spriteBatch.DrawRectangle(Area, _borderColor.Value);
            }

            //Vector2 textSize = _font.MeasureString(Text);
            //spriteBatch.DrawString(_font, Text, Position, _textColor, 0.0f, textSize / 2.0f, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(_font, Text, position, _textColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}