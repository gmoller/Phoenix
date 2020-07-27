using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;
using Microsoft.Xna.Framework.Content;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class LabelOld : ControlOld, IControl
    {
        private readonly IControl _parent; // TODO: move into base/interface?

        private readonly HorizontalAlignment _textAlignment;
        private readonly Color _textColor;
        private readonly Color? _textShadowColor;
        private readonly Color? _backColor;
        private readonly Color? _borderColor;

        public SpriteFont Font { get; }
        public string Text { get; set; }
        public Matrix? Transform { get; set; }

        public int Top => Area.Top;
        public int Bottom => Area.Bottom;
        public int Left => Area.Left;
        public int Right => Area.Right;
        public int Width => Area.Width;
        public int Height => Area.Height;
        public Microsoft.Xna.Framework.Point Center => Area.Center;
        public Microsoft.Xna.Framework.Point TopLeft => TopLeft;
        public Microsoft.Xna.Framework.Point TopRight => new Microsoft.Xna.Framework.Point(Right, Top);
        public Microsoft.Xna.Framework.Point BottomLeft => new Microsoft.Xna.Framework.Point(Left, Bottom);
        public Microsoft.Xna.Framework.Point BottomRight => new Microsoft.Xna.Framework.Point(Right, Bottom);
        public Microsoft.Xna.Framework.Point Size => Area.Size;

        //public Vector2 RelativePosition => new Vector2(_parent.RelativePosition.X + TopLeftPosition.X, _parent.RelativePosition.Y + TopLeftPosition.Y);
        public Vector2 RelativePosition => new Vector2(Left - _parent.TopLeft.X, Top - _parent.TopLeft.Y);

        public event EventHandler Click;

        public LabelOld(string name, string fontName, Vector2 position, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string text, HorizontalAlignment textAlignment, Color textColor, Color? textShadowColor = null, Color? backColor = null, Color ? borderColor = null, Matrix? transform = null, IControl parent = null) :
            base(name, position, horizontalAlignment, verticalAlignment, size)
        {
            _parent = parent;
            var font = AssetsManager.Instance.GetSpriteFont(fontName);
            if (size.Equals(Vector2.Zero))
            {
                throw new NotSupportedException("Autosize not currently supported.");
                //Size = font.MeasureString(text);
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

        public LabelOld(string name, string fontName, ControlOld controlToDockTo, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string text, HorizontalAlignment textAlignment, Color textColor, Color? textShadowColor = null, Color? backColor = null, Color ? borderColor = null, Matrix? transform = null, IControl parent = null) :
            base(name, controlToDockTo, horizontalAlignment, verticalAlignment, size)
        {
            _parent = parent;
            var font = AssetsManager.Instance.GetSpriteFont(fontName);
            if (size.Equals(Vector2.Zero))
            {
                throw new NotSupportedException("Autosize not currently supported.");
                //Size = font.MeasureString(text);
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

        public void SetTopLeftPosition(int x, int y)
        {
            //_actualDestinationRectangle.X = x;
            //_actualDestinationRectangle.Y = y;
        }

        public void LoadContent(ContentManager content)
        {
        }

        public virtual void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            Microsoft.Xna.Framework.Point mousePosition;
            if (Transform == null)
            {
                mousePosition = input.MousePosition;
            }
            else
            {
                var worldPosition = DeviceManager.Instance.WorldPosition;
                mousePosition = new Microsoft.Xna.Framework.Point(worldPosition.X, worldPosition.Y);
            }

            if (Area.Contains(mousePosition) && input.IsLeftMouseButtonReleased)
            {
                OnClick(new EventArgs());
            }
        }

        public virtual void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

            if (_backColor != null)
            {
                spriteBatch.FillRectangle(Area, _backColor.Value, 0.49f);
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

            EndSpriteBatch(spriteBatch);
        }

        private SpriteBatch BeginSpriteBatch(Matrix? transform)
        {
            var spriteBatch = DeviceManager.Instance.GetNewSpriteBatch();
            spriteBatch.Begin(transformMatrix: transform);

            return spriteBatch;
        }

        private void EndSpriteBatch(SpriteBatch spriteBatch)
        {
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
                    position = Center.ToVector2();
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

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => "";//$"{{Name={Name},TopLeftPosition={TopLeft},RelativePosition={RelativePosition},Size={Size}}}";
    }
}