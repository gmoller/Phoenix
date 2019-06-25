using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Input;
using Utilities;

namespace GuiControls
{
    public class Button : Control
    {
        private Texture2D _currentTexture;
        private readonly Texture2D _textureNormal;
        private readonly Texture2D _textureActive;
        private readonly Texture2D _textureHover;

        private Label _label;

        private float _cooldownTime; // in milliseconds

        public event EventHandler Click;

        public Button(Vector2 position, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, Texture2D textureNormal, Texture2D textureActive, Texture2D textureHover, Label label = null) :
            base(position, horizontalAlignment, verticalAlignment, size)
        {
            _cooldownTime = 0.0f;

            _textureNormal = textureNormal;
            _textureActive = textureActive;
            _textureHover = textureHover;
            _currentTexture = textureNormal;

            _label = label;
        }

        public void Update(GameTime gameTime)
        {
            if (_cooldownTime > 0.0f)
            {
                _cooldownTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_cooldownTime <= 0.0f)
                {
                    OnClickComplete();
                }
                return;
            }
            else
            {
                if (Area.Contains(MouseHandler.MousePosition))
                {
                    _currentTexture = _textureHover;
                    if (MouseHandler.IsLeftButtonReleased())
                    {
                        OnClick(new EventArgs());
                    }
                }
                else
                {
                    _currentTexture = _textureNormal;
                }
            }

            _label?.Update(gameTime);
        }

        public void Draw(Matrix? transform = null)
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transform);
            spriteBatch.Draw(_currentTexture, Area, Color.White);
            spriteBatch.End();

            _label?.Draw();
        }

        private void OnClick(EventArgs e)
        {
            _cooldownTime = 200.0f;
            _currentTexture = _textureActive;
            Click?.Invoke(this, e);
        }

        private void OnClickComplete()
        {
            _cooldownTime = 0.0f;
            _currentTexture = _textureNormal;
        }
    }
}