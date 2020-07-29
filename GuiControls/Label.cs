using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Label : Control
    {
        private readonly string _fontName;
        private readonly Color _textColor;
        private readonly Color? _textShadowColor;
        private readonly Color? _backColor;
        private readonly Vector2 _position;
        private readonly Alignment _positionAlignment;
        private readonly Alignment _contentAlignment;
        private readonly bool _autoSize;

        private SpriteFont _font;

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                Resize(value);
            }
        }

        public Label(string name, Vector2 position, Alignment positionAlignment, string text, string fontName, Color textColor, Color? textShadowColor = null, Color? backColor = null, float layerDepth = 0.0f, IControl parent = null) :
            this(name, position, positionAlignment, Vector2.Zero, text, Alignment.TopLeft, fontName, textColor, textShadowColor, backColor, layerDepth, parent)
        {
            _autoSize = true;
        }

        public Label(string name, Vector2 position, Alignment positionAlignment, Vector2 size, string text, Alignment contentAlignment, string fontName, Color textColor, Color? textShadowColor = null, Color? backColor = null, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, positionAlignment, size, string.Empty, string.Empty, null, null, null, null, layerDepth, parent)
        {
            _fontName = fontName;
            Text = text;
            _textColor = textColor;
            _textShadowColor = textShadowColor;
            _backColor = backColor;
            _position = position;
            _positionAlignment = positionAlignment;
            _contentAlignment = contentAlignment;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = AssetsManager.Instance.GetSpriteFont(_fontName);
            Resize(Text);
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            base.Update(input, deltaTime, transform);
        }

        public override void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

            if (_backColor != null)
            {
                spriteBatch.FillRectangle(ActualDestinationRectangle, _backColor.Value, LayerDepth);
            }

            if (_textShadowColor != null)
            {
                // TODO: handle content alignment
                spriteBatch.DrawString(_font, Text, TopLeft.ToVector2() + Vector2.One, _textShadowColor.Value, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, LayerDepth);
            }

            // TODO: handle content alignment
            spriteBatch.DrawString(_font, Text, TopLeft.ToVector2(), _textColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, LayerDepth);

            EndSpriteBatch(spriteBatch);
        }

        private void Resize(string text)
        {
            if (_autoSize && _font != null && text != null)
            {
                var size = _font.MeasureString(text);
                DetermineArea(_position, _positionAlignment, size);
            }
        }
    }
}