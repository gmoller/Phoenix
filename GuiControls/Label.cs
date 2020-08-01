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
        public virtual string Text { get; set; }

        protected Label(
            Vector2 position, 
            Alignment positionAlignment, 
            Vector2 size, 
            string text, 
            string fontName, 
            Color textColor, 
            Color? textShadowColor = null, 
            Color? backColor = null, 
            Color? borderColor = null, 
            float layerDepth = 0.0f, 
            IControl parent = null,
            string name = "") : 
            base(
                position, 
                positionAlignment, 
                size, 
                string.Empty, 
                string.Empty, 
                null, 
                null, 
                null, 
                null, 
                layerDepth, 
                parent,
                name)
        {
            Text = text;
            FontName = fontName;
            TextColor = textColor;
            TextShadowColor = textShadowColor;
            BackColor = backColor;
            BorderColor = borderColor;
        }

        public override void LoadContent(ContentManager content)
        {
            Font = AssetsManager.Instance.GetSpriteFont(FontName);
        }

        protected abstract Vector2 DetermineOffset(SpriteFont font, Vector2 size, string text);

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            if (BackColor != null)
            {
                spriteBatch.FillRectangle(
                    ActualDestinationRectangle, 
                    BackColor.Value, 
                    LayerDepth);
            }

            var offset = DetermineOffset(Font, Size.ToVector2(), Text);
            if (TextShadowColor != null)
            {
                spriteBatch.DrawString(
                    Font, 
                    Text, 
                    TopLeft.ToVector2() + offset + Vector2.One, 
                    TextShadowColor.Value, 
                    0.0f, 
                    Vector2.Zero, 
                    1.0f, 
                    SpriteEffects.None, 
                    LayerDepth);
            }

            if (BorderColor != null)
            {
                spriteBatch.DrawRectangle(
                    new Rectangle(
                        ActualDestinationRectangle.X,
                        ActualDestinationRectangle.Y,
                        ActualDestinationRectangle.Width - 1,
                        ActualDestinationRectangle.Height - 1),
                    BorderColor.Value, 
                    LayerDepth);
            }

            spriteBatch.DrawString(
                Font, 
                Text, 
                TopLeft.ToVector2() + offset, 
                TextColor, 
                0.0f, 
                Vector2.Zero, 
                1.0f, 
                SpriteEffects.None, 
                LayerDepth);
        }
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class LabelAutoSized : Label
    {
        private readonly Vector2 _position;
        private readonly Alignment _positionAlignment;
        private readonly bool _autoSize;

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                Resize(value);
            }
        }

        public LabelAutoSized(
            Vector2 position,
            Alignment positionAlignment,
            string text,
            string fontName,
            Color textColor,
            IControl parent,
            string name = "") :
            this(
                position,
                positionAlignment,
                text,
                fontName,
                textColor,
                null,
                null,
                null,
                0.0f,
                parent,
                name)
        {
        }

        public LabelAutoSized(
            Vector2 position, 
            Alignment positionAlignment, 
            string text, 
            string fontName, 
            Color textColor, 
            Color? textShadowColor = null, 
            Color? backColor = null, 
            Color? borderColor = null, 
            float layerDepth = 0.0f, 
            IControl parent = null,
            string name = "") :
            base(
                position, 
                positionAlignment, 
                Vector2.Zero, 
                text, 
                fontName, 
                textColor, 
                textShadowColor, 
                backColor, 
                borderColor, 
                layerDepth, 
                parent,
                name)
        {
            _position = position;
            _positionAlignment = positionAlignment;
            _autoSize = true;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            Resize(Text);
        }

        protected override Vector2 DetermineOffset(SpriteFont font, Vector2 size, string text)
        {
            return Vector2.Zero;
        }

        private void Resize(string text)
        {
            if (_autoSize && Font != null && text != null)
            {
                var size = Font.MeasureString(text);
                DetermineArea(_position, _positionAlignment, size);
            }
        }
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class LabelSized : Label
    {
        private readonly Alignment _contentAlignment;

        public LabelSized(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            Alignment contentAlignment,
            string text,
            string fontName,
            Color textColor,
            Color? textShadowColor = null,
            IControl parent = null,
            string name = "") :
            this(
                position,
                positionAlignment,
                size,
                contentAlignment,
                text,
                fontName,
                textColor,
                textShadowColor,
                null,
                null,
                0.0f,
                parent)
        {
        }

        public LabelSized(
            Vector2 position, 
            Alignment positionAlignment, 
            Vector2 size, 
            Alignment contentAlignment, 
            string text, 
            string fontName, 
            Color textColor, 
            Color? textShadowColor = null, 
            Color? backColor = null, 
            Color? borderColor = null, 
            float layerDepth = 0.0f, 
            IControl parent = null,
            string name = "") :
            base(
                position, 
                positionAlignment, 
                size, 
                text, 
                fontName, 
                textColor, 
                textShadowColor, 
                backColor, 
                borderColor, 
                layerDepth, 
                parent,
                name)
        {
            _contentAlignment = contentAlignment;
            DetermineArea(position, positionAlignment, size);
        }

        protected override Vector2 DetermineOffset(SpriteFont font, Vector2 size, string text)
        {
            var textSize = font.MeasureString(text);

            switch (_contentAlignment)
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
                    throw new Exception($"ContentAlignment [{_contentAlignment}] is not implemented.");
            }
        }
    }
}