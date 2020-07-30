using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public abstract class Label : Control
    {
        protected readonly string FontName;
        protected readonly Color TextColor;
        protected readonly Color? TextShadowColor;
        protected readonly Color? BackColor;
        protected readonly Color? BorderColor;

        protected SpriteFont Font { get; set; }

        protected string Text { get; set; }

        protected Label(string name, Vector2 position, Alignment positionAlignment, Vector2 size, string fontName, Color textColor, Color? textShadowColor = null, Color? backColor = null, Color? borderColor = null, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, positionAlignment, size, string.Empty, string.Empty, null, null, null, null, layerDepth, parent)
        {
            FontName = fontName;
            TextColor = textColor;
            TextShadowColor = textShadowColor;
            BackColor = backColor;
            BorderColor = borderColor;
        }

        //public override void LoadContent(ContentManager content)
        //{
        //    _font = AssetsManager.Instance.GetSpriteFont(_fontName);
        //}

        //protected abstract void Draw(SpriteBatch spriteBatch, Matrix? transform = null);
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class LabelAutoSized : Control
    {
        private readonly string _fontName;
        private readonly Color _textColor;
        private readonly Color? _textShadowColor;
        private readonly Color? _backColor;
        private readonly Color? _borderColor;
        private readonly Vector2 _position;
        private readonly Alignment _positionAlignment;
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

        public LabelAutoSized(string name, Vector2 position, Alignment positionAlignment, string text, string fontName, Color textColor, Color? textShadowColor = null, Color? backColor = null, Color? borderColor = null, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, positionAlignment, Vector2.Zero, string.Empty, string.Empty, null, null, null, null, layerDepth, parent)
        {
            _fontName = fontName;
            Text = text;
            _textColor = textColor;
            _textShadowColor = textShadowColor;
            _backColor = backColor;
            _borderColor = borderColor;
            _position = position;
            _positionAlignment = positionAlignment;
            _autoSize = true;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = AssetsManager.Instance.GetSpriteFont(_fontName);
            Resize(Text);
        }

        protected override void Draw(SpriteBatch spriteBatch, Matrix? transform = null)
        {
            if (_backColor != null)
            {
                spriteBatch.FillRectangle(ActualDestinationRectangle, _backColor.Value, LayerDepth);
            }

            if (_textShadowColor != null)
            {
                spriteBatch.DrawString(_font, Text, TopLeft.ToVector2() + Vector2.One, _textShadowColor.Value, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, LayerDepth);
            }

            if (_borderColor != null)
            {
                spriteBatch.DrawRectangle(new Rectangle(ActualDestinationRectangle.X, ActualDestinationRectangle.Y, ActualDestinationRectangle.Width - 1, ActualDestinationRectangle.Height - 1), _borderColor.Value, LayerDepth);
            }

            spriteBatch.DrawString(_font, Text, TopLeft.ToVector2(), _textColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, LayerDepth);
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

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class LabelSized : Control
    {
        private readonly string _fontName;
        private readonly Color _textColor;
        private readonly Color? _textShadowColor;
        private readonly Color? _backColor;
        private readonly Color? _borderColor;
        private readonly Alignment _contentAlignment;

        private SpriteFont _font;

        public string Text { get; set; }

        public LabelSized(string name, Vector2 position, Alignment positionAlignment, Vector2 size, Alignment contentAlignment, string text, string fontName, Color textColor, Color? textShadowColor = null, Color? backColor = null, Color? borderColor = null, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, positionAlignment, size, string.Empty, string.Empty, null, null, null, null, layerDepth, parent)
        {
            _fontName = fontName;
            Text = text;
            _textColor = textColor;
            _textShadowColor = textShadowColor;
            _backColor = backColor;
            _borderColor = borderColor;
            _contentAlignment = contentAlignment;

            DetermineArea(position, positionAlignment, size);
        }

        public override void LoadContent(ContentManager content)
        {
            _font = AssetsManager.Instance.GetSpriteFont(_fontName);
        }

        protected override void Draw(SpriteBatch spriteBatch, Matrix? transform = null)
        {
            if (_backColor != null)
            {
                spriteBatch.FillRectangle(ActualDestinationRectangle, _backColor.Value, LayerDepth);
            }

            var offset = DetermineOffset(Size.ToVector2(), _contentAlignment, Text);
            if (_textShadowColor != null)
            {
                spriteBatch.DrawString(_font, Text, TopLeft.ToVector2() + offset + Vector2.One, _textShadowColor.Value, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, LayerDepth);
            }

            if (_borderColor != null)
            {
                spriteBatch.DrawRectangle(new Rectangle(ActualDestinationRectangle.X, ActualDestinationRectangle.Y, ActualDestinationRectangle.Width - 1, ActualDestinationRectangle.Height - 1), _borderColor.Value, LayerDepth);
            }

            spriteBatch.DrawString(_font, Text, TopLeft.ToVector2() + offset, _textColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, LayerDepth);
        }

        private Vector2 DetermineOffset(Vector2 size, Alignment contentAlignment, string text)
        {
            var textSize = _font.MeasureString(text);

            switch (contentAlignment)
            {
                case Alignment.TopLeft:
                    return Vector2.Zero;
                case Alignment.TopCenter:
                    return new Vector2((size.X - textSize.X) * 0.5f, 0.0f);
                case Alignment.TopRight:
                    return new Vector2(size.X - textSize.X, 0.0f);

                case Alignment.MiddleLeft:
                    return new Vector2(0.0f, (size.Y - textSize.Y) * 0.5f);
                case Alignment.MiddleCenter:
                    return new Vector2((size.X - textSize.X) * 0.5f, (size.Y - textSize.Y) * 0.5f);
                case Alignment.MiddleRight:
                    return new Vector2(size.X - textSize.X, (size.Y - textSize.Y) * 0.5f);

                case Alignment.BottomLeft:
                    return new Vector2(0.0f, size.Y - textSize.Y);
                case Alignment.BottomCenter:
                    return new Vector2((size.X - textSize.X) * 0.5f, size.Y - textSize.Y);
                case Alignment.BottomRight:
                    return new Vector2(size.X - textSize.X, size.Y - textSize.Y);

                default:
                    throw new Exception($"ContentAlignment [{contentAlignment}] is not implemented.");
            }
        }
    }
}