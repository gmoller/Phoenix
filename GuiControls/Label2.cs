using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;

namespace GuiControls
{
    public class Label2 : IControl
    {
        private readonly IControl _parent;
        private readonly string _name;
        private readonly string _fontName;
        private readonly Color _textColor;
        private readonly Color _backColor;
        private readonly float _scale;
        private readonly float _layerDepth;
        private readonly ContentAlignment _alignment;

        private SpriteFont _font;
        private Rectangle _area;
        private Rectangle _originalScissorRectangle;
        private string _text;
        private Vector2 _origin;

        private bool _contentLoaded;

        public Vector2 Position
        {
            get => new Vector2(_area.X, _area.Y);

            set
            {
                _area.X = (int)value.X;
                _area.Y = (int)value.Y;
            }
        }

        private Rectangle DestinationRectangle => new Rectangle(_area.X - (int)_origin.X, _area.Y - (int)_origin.Y, _area.Width, _area.Height);

        public Label2(string name, Vector2 position, Vector2 size, string text, string fontName, Color textColor, Color backColor, float layerDepth = 0.0f, IControl parent = null) : 
            this(name, position, ContentAlignment.TopLeft, size, text, fontName, textColor, backColor, layerDepth, parent)
        {
        }

        public Label2(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string text, string fontName, Color textColor, Color backColor, float layerDepth = 0.0f, IControl parent = null)
        {
            _parent = parent;
            _name = name;
            _fontName = fontName;
            _area = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            _text = text;
            _textColor = textColor;
            _backColor = backColor;
            _scale = 1.0f;
            _layerDepth = layerDepth;
            _alignment = alignment;
            _contentLoaded = false;
        }

        public void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            var mousePosition = DetermineMousePosition(input, transform);
        }

        public void Draw(Matrix? transform = null)
        {
            LoadContent();

            var spriteBatch = BeginSpriteBatch(transform);

            if (_backColor != null)
            {
                spriteBatch.FillRectangle(DestinationRectangle, _backColor, _layerDepth);
            }

            spriteBatch.DrawString(_font, _text, Position, _textColor, 0.0f, _origin, _scale, SpriteEffects.None, _layerDepth);

            EndSpriteBatch(spriteBatch);
        }

        private void LoadContent()
        {
            if (_contentLoaded) return;

            _font = AssetsManager.Instance.GetSpriteFont(_fontName);

            DetermineOrigin(_alignment);

            _contentLoaded = true;
        }

        private void DetermineOrigin(ContentAlignment alignment)
        {
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    _origin = new Vector2(0.0f, 0.0f);
                    break;
                case ContentAlignment.TopCenter:
                    _origin = new Vector2(_area.Width / 2, 0.0f);
                    break;
                case ContentAlignment.TopRight:
                    _origin = new Vector2(_area.Width, 0.0f);
                    break;
                case ContentAlignment.MiddleLeft:
                    _origin = new Vector2(0.0f, _area.Height / 2);
                    break;
                case ContentAlignment.MiddleCenter:
                    _origin = new Vector2(_area.Width / 2, _area.Height / 2);
                    break;
                case ContentAlignment.MiddleRight:
                    _origin = new Vector2(_area.Width, _area.Height / 2);
                    break;
                case ContentAlignment.BottomLeft:
                    _origin = new Vector2(0.0f, _area.Height);
                    break;
                case ContentAlignment.BottomCenter:
                    _origin = new Vector2(_area.Width / 2, _area.Height);
                    break;
                case ContentAlignment.BottomRight:
                    _origin = new Vector2(_area.Width, _area.Height);
                    break;
                default:
                    throw new Exception("Not implemented.");
            }
        }

        private Microsoft.Xna.Framework.Point DetermineMousePosition(InputHandler input, Matrix? transform)
        {
            if (transform == null)
            {
                return input.MousePosition;
            }
            else
            {
                var worldPosition = DeviceManager.Instance.WorldPosition;
                return new Microsoft.Xna.Framework.Point(worldPosition.X, worldPosition.Y);
            }
        }

        private SpriteBatch BeginSpriteBatch(Matrix? transform)
        {
            var spriteBatch = DeviceManager.Instance.GetNewSpriteBatch();
            spriteBatch.Begin(rasterizerState: new RasterizerState { ScissorTestEnable = true }, transformMatrix: transform);

            _originalScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            spriteBatch.GraphicsDevice.ScissorRectangle = DestinationRectangle;

            return spriteBatch;
        }

        private void EndSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.ScissorRectangle = _originalScissorRectangle;
            DeviceManager.Instance.ReturnSpriteBatchToPool(spriteBatch);
        }
    }
}