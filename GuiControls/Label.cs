using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    public class Label : Control
    {
        private readonly SpriteFont _font;
        private readonly Color _textColor;
        private string _text;

        public bool TextShadow { get; set; }
        public Vector2 TextShadowOffset { get; set; } = Vector2.One;
        public Color TextShadowColor { get; set; } = Color.Gray;

        public bool DrawBorder { get; set; }
        public Color DrawBorderColor { get; set; } = Color.DarkGray;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                AutoSize(_text);
            }
        }

        public Label(SpriteFont font, Vector2 position, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, string text, Color textColor, float scale = 1.0f, float alpha = 1.0f) :
            base(verticalAlignment, horizontalAlignment, position)
        {
            _font = font;
            _text = text;
            _textColor = textColor;
            Scale = scale;
            Alpha = alpha;

            AutoSize(text);
        }

        private void AutoSize(string text)
        {
            Vector2 v = _font.MeasureString(text);
            Size = new Vector2(v.X, v.Y);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = DetermineTopLeftPosition(VerticalAlignment, HorizontalAlignment, Position, ScaledWidth, ScaledHeight);

            if (TextShadow)
            {
                spriteBatch.DrawString(_font, _text, position + TextShadowOffset, TextShadowColor * Alpha, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0.0f);
            }

            spriteBatch.DrawString(_font, _text, position, _textColor * Alpha, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0.0f);

            if (DrawBorder)
            {
                //spriteBatch.DrawRectangle(Area, DrawBorderColor);
            }
        }
    }
}