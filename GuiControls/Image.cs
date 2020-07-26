using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;
using Microsoft.Xna.Framework.Content;

namespace GuiControls
{
    public class Image : IControl
    {
        private readonly IControl _parent; // TODO: move into base/interface?

        private readonly string _name;
        private readonly string _textureAtlas;
        private readonly string _textureName;
        private readonly byte? _textureId;
        private readonly Color _color;
        private readonly float _layerDepth;

        private Texture2D _texture;
        private Rectangle _actualDestinationRectangle;
        private Rectangle _sourceRectangle;

        // offset from parent
        public int Top => _actualDestinationRectangle.Top;
        public int Bottom => _actualDestinationRectangle.Bottom;
        public int Left => _actualDestinationRectangle.Left;
        public int Right => _actualDestinationRectangle.Right;
        public int Width => _actualDestinationRectangle.Width;
        public int Height => _actualDestinationRectangle.Height;
        public Microsoft.Xna.Framework.Point Center => _actualDestinationRectangle.Center;
        public Microsoft.Xna.Framework.Point TopLeft => new Microsoft.Xna.Framework.Point(Left, Top);
        public Microsoft.Xna.Framework.Point TopRight => new Microsoft.Xna.Framework.Point(Right, Top);
        public Microsoft.Xna.Framework.Point BottomLeft => new Microsoft.Xna.Framework.Point(Left, Bottom);
        public Microsoft.Xna.Framework.Point BottomRight => new Microsoft.Xna.Framework.Point(Right, Bottom);

        public Vector2 TopLeftPosition
        {
            get => new Vector2(_actualDestinationRectangle.Left, _actualDestinationRectangle.Top);

            set
            {
                _actualDestinationRectangle.X = (int)value.X;
                _actualDestinationRectangle.Y = (int)value.Y;
            }
        }
        public Vector2 TopRightPosition => new Vector2(_actualDestinationRectangle.Right, _actualDestinationRectangle.Top);
        public Vector2 BottomLeftPosition => new Vector2(_actualDestinationRectangle.Left, _actualDestinationRectangle.Bottom);
        public Vector2 BottomRightPosition => new Vector2(_actualDestinationRectangle.Right, _actualDestinationRectangle.Bottom);

        public Vector2 RelativePosition => new Vector2(Left - _parent.TopLeftPosition.X, Top - _parent.TopLeftPosition.Y);

        public Image(string name, Vector2 position, Vector2 size, string textureName, float layerDepth = 0.0f, IControl parent = null) : 
            this(name, position, ContentAlignment.TopLeft, size, string.Empty, textureName, null, layerDepth, parent)
        {
        }

        public Image(string name, Vector2 position, Vector2 size, string textureAtlas, string textureName, float layerDepth = 0.0f, IControl parent = null) : 
            this(name, position, ContentAlignment.TopLeft, size, textureAtlas, textureName, null, layerDepth, parent)
        {
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureName, float layerDepth = 0.0f, IControl parent = null) : 
            this(name, position, alignment, size, string.Empty, textureName, null, layerDepth, parent)
        {
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, byte textureId, float layerDepth = 0.0f, IControl parent = null) :
            this(name, position, alignment, size, textureAtlas, string.Empty, textureId, layerDepth, parent)
        {
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, string textureName, byte? textureId = null, float layerDepth = 0.0f, IControl parent = null)
        {
            _parent = parent;

            _name = name;
            _textureAtlas = textureAtlas;
            _textureName = textureName;
            _textureId = textureId;

            var topLeft = DetermineTopLeft(position * DeviceManager.Instance.SizeRatio, alignment, size * DeviceManager.Instance.SizeRatio);
            if (_parent == null)
            {
                // the same
                _actualDestinationRectangle = new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)size.X, (int)size.Y);
            }
            else
            {
                // offset from parent's position
                int x =  (int)(_parent.TopLeftPosition.X + topLeft.X);
                int y =  (int)(_parent.TopLeftPosition.Y + topLeft.Y);
                _actualDestinationRectangle = new Rectangle(x, y, (int)size.X, (int)size.Y);
            }
            
            _color = Color.White;
            _layerDepth = layerDepth;
        }

        public void LoadContent(ContentManager content)
        {
            if (string.IsNullOrEmpty(_textureAtlas))
            {
                _texture = AssetsManager.Instance.GetTexture(_textureName);
                _sourceRectangle = _texture.Bounds;
            }
            else
            {
                _texture = AssetsManager.Instance.GetTexture(_textureAtlas);
                var spec = AssetsManager.Instance.GetAtlas(_textureAtlas);
                _sourceRectangle = _textureId == null ? spec.Frames[_textureName].ToRectangle() : spec.Frames[(int)_textureId].ToRectangle();
            }
        }

        public void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
        }

        public void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

            spriteBatch.Draw(_texture, _actualDestinationRectangle, _sourceRectangle, _color, 0.0f, Vector2.Zero, SpriteEffects.None, _layerDepth);

            EndSpriteBatch(spriteBatch);
        }

        private Vector2 DetermineTopLeft(Vector2 position, ContentAlignment alignment, Vector2 size)
        {
            Vector2 topLeft;
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    topLeft = position;
                    break;
                case ContentAlignment.TopCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y);
                    break;
                case ContentAlignment.TopRight:
                    topLeft = new Vector2(position.X - size.X, position.Y);
                    break;
                case ContentAlignment.MiddleLeft:
                    topLeft = new Vector2(position.X, position.Y - size.Y * 0.5f);
                    break;
                case ContentAlignment.MiddleCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y - size.Y * 0.5f);
                    break;
                case ContentAlignment.MiddleRight:
                    topLeft = new Vector2(position.X - size.X, position.Y - size.Y * 0.5f);
                    break;
                case ContentAlignment.BottomLeft:
                    topLeft = new Vector2(position.X, position.Y - size.Y);
                    break;
                case ContentAlignment.BottomCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y - size.Y);
                    break;
                case ContentAlignment.BottomRight:
                    topLeft = new Vector2(position.X - size.X, position.Y - size.Y);
                    break;
                default:
                    throw new Exception($"Alignment [{alignment}] not implemented.");
            }

            return topLeft;
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
    }
}