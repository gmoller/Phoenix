using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Input;
using Utilities;

namespace GuiControls
{
    public class Label : Control
    {
        private readonly SpriteFont _font;
        private readonly HorizontalAlignment _textAlignment;
        private readonly Color _textColor;
        private readonly Color? _textShadowColor;
        private readonly Color? _backColor;
        private readonly Color? _borderColor;
        private readonly Matrix? _transform;

        public string Text { get; set; }

        public event EventHandler Click;

        public Label(SpriteFont font, Vector2 position, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string text, HorizontalAlignment textAlignment, Color textColor, Color? textShadowColor = null, Color? backColor = null, Color ? borderColor = null, Matrix? transform = null) :
            base(position, horizontalAlignment, verticalAlignment, size)
        {
            _font = font;
            Text = text;
            _textAlignment = textAlignment;
            _textColor = textColor;
            _textShadowColor = textShadowColor;
            _backColor = backColor;
            _borderColor = borderColor;
            _transform = transform;
        }

        public Label(SpriteFont font, Control controlToDockTo, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string text, HorizontalAlignment textAlignment, Color textColor, Color? textShadowColor = null, Color? backColor = null, Color ? borderColor = null, Matrix? transform = null) :
            base(controlToDockTo, horizontalAlignment, verticalAlignment, size)
        {
            _font = font;
            Text = text;
            _textAlignment = textAlignment;
            _textColor = textColor;
            _textShadowColor = textShadowColor;
            _backColor = backColor;
            _borderColor = borderColor;
            _transform = transform;
        }

        public virtual void Update(GameTime gameTime)
        {
            Point mousePosition;
            if (_transform == null)
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

        public virtual void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetNewSpriteBatch();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, _transform);

            if (_backColor != null)
            {
                spriteBatch.FillRectangle(Area, _backColor.Value);
            }

            var textSize = _font.MeasureString(Text);

            var position = GetPosition(textSize);
            var origin = GetOrigin(textSize);

            if (_textShadowColor != null)
            {
                spriteBatch.DrawString(_font, Text, position + Vector2.One, _textShadowColor.Value, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
            }

            if (_borderColor != null)
            {
                spriteBatch.DrawRectangle(Area, _borderColor.Value);
            }

            spriteBatch.DrawString(_font, Text, position, _textColor, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.End();

            DeviceManager.Instance.ReturnSpriteBatchToPool(spriteBatch);
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