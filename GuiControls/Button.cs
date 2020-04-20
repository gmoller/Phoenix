using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;

namespace GuiControls
{
    public class Button : Control
    {
        private Texture2D _texture;
        private AtlasSpec2 _spec;
        private string _textureNormal;
        private string _textureActive;
        private string _textureHover;
        private string _textureDisabled;
        private Rectangle _frame;

        private Label _label;

        private float _cooldownTime; // in milliseconds

        public event EventHandler Click;

        public bool MouseOver { get; private set; }
        public bool Enabled { get; set; }

        public Button(string name, Vector2 position, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string guiTextures, string textureNormal, string textureActive, string textureDisabled, string textureHover, Label label = null) :
            base(name, position, horizontalAlignment, verticalAlignment, size)
        {
            _cooldownTime = 0.0f;

            _texture = AssetsManager.Instance.GetTexture(guiTextures);
            _spec = AssetsManager.Instance.GetAtlas(guiTextures);
            _textureNormal = textureNormal;
            _textureActive = textureActive;
            _textureHover = textureHover;
            _textureDisabled = textureDisabled;

            _label = label;
            Enabled = true;
        }

        public void Update(float deltaTime)
        {
            if (!Enabled)
            {
                var f = _spec.Frames[_textureDisabled];
                _frame = new Rectangle(f.X, f.Y, f.Width, f.Height);

                return;
            }

            MouseOver = Area.Contains(MouseHandler.MousePosition);

            if (_cooldownTime > 0.0f)
            {
                _cooldownTime -= deltaTime;
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
                    var f = _spec.Frames[_textureHover];
                    _frame = new Rectangle(f.X, f.Y, f.Width, f.Height);
                    if (MouseHandler.IsLeftButtonReleased())
                    {
                        OnClick(new EventArgs());
                    }
                }
                else
                {
                    var f = _spec.Frames[_textureNormal];
                    _frame = new Rectangle(f.X, f.Y, f.Width, f.Height);
                }
            }

            _label?.Update(deltaTime);
        }

        public void Draw(Matrix? transform = null)
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transform);
            spriteBatch.Draw(_texture, Area, _frame, Color.White);
            spriteBatch.End();

            _label?.Draw();
        }

        private void OnClick(EventArgs e)
        {
            _cooldownTime = 50.0f;
            var f = _spec.Frames[_textureActive];
            _frame = new Rectangle(f.X, f.Y, f.Width, f.Height);
            Click?.Invoke(this, e);
        }

        private void OnClickComplete()
        {
            _cooldownTime = 0.0f;
            var f = _spec.Frames[_textureNormal];
            _frame = new Rectangle(f.X, f.Y, f.Width, f.Height);
        }
    }
}