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

        public Label2 Label { get; set; }

        private float _cooldownTime; // in milliseconds

        public event EventHandler Click;

        public bool MouseOver { get; private set; }
        public bool Enabled { get; set; }

        public Button(string name, Vector2 position, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size, string guiTextures, string textureNormal, string textureActive, string textureDisabled, string textureHover) :
            base(name, position, horizontalAlignment, verticalAlignment, size)
        {
            _cooldownTime = 0.0f;

            _texture = AssetsManager.Instance.GetTexture(guiTextures);
            _spec = AssetsManager.Instance.GetAtlas(guiTextures);
            _textureNormal = textureNormal;
            _textureActive = textureActive;
            _textureHover = textureHover;
            _textureDisabled = textureDisabled;

            Enabled = true;
        }

        public void Update(InputHandler input, float deltaTime)
        {
            if (!Enabled)
            {
                var f = _spec.Frames[_textureDisabled];
                _frame = new Rectangle(f.X, f.Y, f.Width, f.Height);

                return;
            }

            MouseOver = Area.Contains(input.MousePosition);
            input.Eaten = MouseOver;

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
                if (Area.Contains(input.MousePosition))
                {
                    var f = _spec.Frames[_textureHover];
                    _frame = new Rectangle(f.X, f.Y, f.Width, f.Height);
                    if (input.IsLeftMouseButtonReleased)
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

            Label?.Update(input, deltaTime);
        }

        public void Draw(Matrix? transform = null)
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transform);
            spriteBatch.Draw(_texture, Area, _frame, Color.White);
            spriteBatch.End();

            Label?.Draw();
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