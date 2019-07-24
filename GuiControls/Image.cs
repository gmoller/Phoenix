using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;

namespace GuiControls
{
    public class Image : IControl
    {
        private readonly string _textureAtlas;
        private readonly string _textureName;
        private readonly byte? _textureId;
        private readonly Color _color;
        private readonly float _layerDepth;
        private readonly ContentAlignment _alignment;

        private Texture2D _texture;
        private Rectangle _destinationRectangle;
        private Rectangle _sourceRectangle;
        private Vector2 _origin;

        private bool _contentLoaded;

        public int Top => _destinationRectangle.Top;
        public int Bottom => _destinationRectangle.Bottom;
        public int Left => _destinationRectangle.Left;
        public int Right => _destinationRectangle.Right;
        public Point Center => _destinationRectangle.Center;
        public Point TopLeft => new Point(Left, Top);
        public Point TopRight => new Point(Right, Top);
        public Point BottomLeft => new Point(Left, Bottom);
        public Point BottomRight => new Point(Right, Bottom);

        public Vector2 Position
        {
            get => new Vector2(_destinationRectangle.X, _destinationRectangle.Y);

            set
            {
                _destinationRectangle.X = (int)value.X;
                _destinationRectangle.Y = (int)value.Y;
            }
        }

        public Image(string name, Vector2 position, Vector2 size, string textureName, float layerDepth = 0.0f) : this(name, position, ContentAlignment.TopLeft, size, string.Empty, textureName, layerDepth)
        {
        }

        public Image(string name, Vector2 position, Vector2 size, string textureAtlas, string textureName, float layerDepth = 0.0f) : this(name, position, ContentAlignment.TopLeft, size, textureAtlas, textureName, layerDepth)
        {
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureName, float layerDepth = 0.0f) : this(name, position, alignment, size, string.Empty, textureName, layerDepth)
        {
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, byte textureId, float layerDepth = 0.0f)
        {
            _textureAtlas = textureAtlas;
            _textureName = string.Empty;
            _textureId = textureId;
            _destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            _color = Color.White;
            _layerDepth = layerDepth;
            _alignment = alignment;
            _contentLoaded = false;
        }

        public Image(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, string textureName, float layerDepth = 0.0f)
        {
            _textureAtlas = textureAtlas;
            _textureName = textureName;
            _textureId = null;
            _destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            _color = Color.White;
            _layerDepth = layerDepth;
            _alignment = alignment;
            _contentLoaded = false;
        }

        public void Update(GameTime gameTime, Matrix? transform = null)
        {
        }

        public void Draw(SpriteBatch spriteBatch = null, Matrix? transform = null)
        {
            LoadContent();

            bool newSpriteBatch = spriteBatch == null;
            spriteBatch = BeginSpriteBatch(spriteBatch, newSpriteBatch, transform);

            spriteBatch.Draw(_texture, _destinationRectangle, _sourceRectangle, _color, 0.0f, _origin, SpriteEffects.None, _layerDepth);

            EndSpriteBatch(spriteBatch, newSpriteBatch);
        }

        private SpriteBatch BeginSpriteBatch(SpriteBatch spriteBatch, bool newSpriteBatch, Matrix? transform)
        {
            if (newSpriteBatch)
            {
                var shinyNewSpriteBatch = DeviceManager.Instance.GetNewSpriteBatch();
                shinyNewSpriteBatch.Begin(transformMatrix: transform);
                return shinyNewSpriteBatch;
            }

            return spriteBatch;
        }

        private void EndSpriteBatch(SpriteBatch spriteBatch, bool newSpriteBatch)
        {
            if (newSpriteBatch)
            {
                spriteBatch.End();
                DeviceManager.Instance.ReturnSpriteBatchToPool(spriteBatch);
            }
        }

        private void LoadContent()
        {
            if (_contentLoaded) return;

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
                    _origin = new Vector2(_sourceRectangle.Center.X, 0.0f);
                    break;
                case ContentAlignment.TopRight:
                    _origin = new Vector2(_sourceRectangle.Width, 0.0f);
                    break;
                case ContentAlignment.MiddleLeft:
                    _origin = new Vector2(0.0f, _sourceRectangle.Center.Y);
                    break;
                case ContentAlignment.MiddleCenter:
                    _origin = new Vector2(_sourceRectangle.Center.X, _sourceRectangle.Center.Y);
                    break;
                case ContentAlignment.MiddleRight:
                    _origin = new Vector2(_sourceRectangle.Width, _sourceRectangle.Center.Y);
                    break;
                case ContentAlignment.BottomLeft:
                    _origin = new Vector2(0.0f, _sourceRectangle.Height);
                    break;
                case ContentAlignment.BottomCenter:
                    _origin = new Vector2(_sourceRectangle.Center.X, _sourceRectangle.Height);
                    break;
                case ContentAlignment.BottomRight:
                    _origin = new Vector2(_sourceRectangle.Right, _sourceRectangle.Height);
                    break;
                default:
                    throw new Exception("Not implemented.");
            }
        }
    }
}