using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;

namespace GuiControls
{
    public class Label : Control
    {
        private readonly HorizontalAlignment _textAlignment;
        private readonly Color _textColor;
        private readonly Color? _textShadowColor;
        private readonly Color? _backColor;
        private readonly Color? _borderColor;

        public SpriteFont Font { get; }
        public string Text { get; set; }
        public Matrix? Transform { get; set; }

        public event EventHandler Click;

        public Label(string name, string fontName, Vector2 position, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string text, HorizontalAlignment textAlignment, Color textColor, Color? textShadowColor = null, Color? backColor = null, Color ? borderColor = null, Matrix? transform = null) :
            base(name, position, horizontalAlignment, verticalAlignment, size)
        {
            var font = AssetsManager.Instance.GetSpriteFont(fontName);
            if (size.Equals(Vector2.Zero))
            {
                Size = font.MeasureString(text);
            }

            Font = font;
            Text = text;
            _textAlignment = textAlignment;
            _textColor = textColor;
            _textShadowColor = textShadowColor;
            _backColor = backColor;
            _borderColor = borderColor;
            Transform = transform;
        }

        public Label(string name, string fontName, Control controlToDockTo, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string text, HorizontalAlignment textAlignment, Color textColor, Color? textShadowColor = null, Color? backColor = null, Color ? borderColor = null, Matrix? transform = null) :
            base(name, controlToDockTo, horizontalAlignment, verticalAlignment, size)
        {
            var font = AssetsManager.Instance.GetSpriteFont(fontName);
            if (size.Equals(Vector2.Zero))
            {
                Size = font.MeasureString(text);
            }

            Font = font;
            Text = text;
            _textAlignment = textAlignment;
            _textColor = textColor;
            _textShadowColor = textShadowColor;
            _backColor = backColor;
            _borderColor = borderColor;
            Transform = transform;
        }

        public virtual void Update(GameTime gameTime)
        {
            Point mousePosition;
            if (Transform == null)
            {
                mousePosition = MouseHandler.MousePosition;
            }
            else
            {
                Point worldPosition = DeviceManager.Instance.WorldPosition;
                mousePosition = new Point(worldPosition.X, worldPosition.Y);
            }

            if (Area.Contains(mousePosition) && MouseHandler.IsLeftButtonReleased())
            {
                OnClick(new EventArgs());
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch = null)
        {
            bool newSpritebatch = spriteBatch == null;
            if (newSpritebatch)
            {
                spriteBatch = DeviceManager.Instance.GetNewSpriteBatch();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Transform);
            }

            if (_backColor != null)
            {
                spriteBatch.FillRectangle(Area, _backColor.Value, 0.5f);
            }

            var textSize = Font.MeasureString(Text);

            var position = GetPosition(textSize);
            var origin = GetOrigin(textSize);

            if (_textShadowColor != null)
            {
                spriteBatch.DrawString(Font, Text, position + Vector2.One, _textShadowColor.Value, 0.0f, origin, 1.0f, SpriteEffects.None, 0.5f);
            }

            if (_borderColor != null)
            {
                spriteBatch.DrawRectangle(Area, _borderColor.Value, 0.5f);
            }

            spriteBatch.DrawString(Font, Text, position, _textColor, 0.0f, origin, 1.0f, SpriteEffects.None, 0.5f);

            if (newSpritebatch)
            {
                spriteBatch.End();
                DeviceManager.Instance.ReturnSpriteBatchToPool(spriteBatch);
            }
        }

        private Vector2 GetPosition(Vector2 textSize)
        {
            var position = Vector2.Zero;
            switch (_textAlignment)
            {
                case HorizontalAlignment.Left:
                    position = new Vector2(Left, Center.Y);
                    break;
                case HorizontalAlignment.Right:
                    position = new Vector2(Right - textSize.X, Center.Y);
                    break;
                case HorizontalAlignment.Center:
                    position = Center;
                    break;
            }

            return position;
        }

        private Vector2 GetOrigin(Vector2 textSize)
        {
            var origin = Vector2.Zero;
            switch (_textAlignment)
            {
                case HorizontalAlignment.Left:
                    origin = new Vector2(0.0f, textSize.Y / 2.0f);
                    break;
                case HorizontalAlignment.Right:
                    origin = new Vector2(0.0f, textSize.Y / 2.0f);
                    break;
                case HorizontalAlignment.Center:
                    origin = new Vector2(textSize.X / 2.0f, textSize.Y / 2.0f);
                    break;
            }

            return origin;
        }

        private void OnClick(EventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}