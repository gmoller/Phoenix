using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Button : Control
    {
        private float _cooldownTimeInMilliseconds;
        private readonly string _textureNormal;
        private readonly string _textureActive;
        private readonly string _textureHover;
        private readonly string _textureDisabled;
        private AtlasSpec2 _atlas;

        public bool MouseOver { get; private set; }
        public bool Enabled { get; set; }

        public Label Label { get; set; }
        public event EventHandler Click;

        public bool HasAtlas => _atlas != null;

        public Button(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, string textureNormal, string textureActive, string textureDisabled, string textureHover, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, alignment, size, textureAtlas, null, null, layerDepth, parent)
        {
            _textureNormal = textureNormal;
            _textureActive = textureActive;
            _textureHover = textureHover;
            _textureDisabled = textureDisabled;

            Enabled = true;
        }

        public override void LoadContent(ContentManager content)
        {
            if (TextureAtlas.HasValue())
            {
                _atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);
                Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
            }
            else // no atlas
            {
                SetTexture(_textureNormal);
            }
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            if (!Enabled)
            {
                SetTexture(_textureDisabled);

                return;
            }

            MouseOver = ActualDestinationRectangle.Contains(input.MousePosition);
            input.Eaten = MouseOver;

            if (_cooldownTimeInMilliseconds > 0.0f)
            {
                _cooldownTimeInMilliseconds -= deltaTime;
                if (_cooldownTimeInMilliseconds <= 0.0f)
                {
                    OnClickComplete();
                }
                return;
            }

            if (ActualDestinationRectangle.Contains(input.MousePosition))
            {
                SetTexture(_textureHover);

                if (input.IsLeftMouseButtonReleased)
                {
                    OnClick(new EventArgs());
                }
            }
            else
            {
                SetTexture(_textureNormal);
            }

            Label?.Update(input, deltaTime);
        }

        public override void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);

            EndSpriteBatch(spriteBatch);

            Label?.Draw();
        }

        private void OnClick(EventArgs e)
        {
            _cooldownTimeInMilliseconds = 50.0f;

            SetTexture(_textureActive);

            Click?.Invoke(this, e);
        }

        private void OnClickComplete()
        {
            _cooldownTimeInMilliseconds = 0.0f;

            SetTexture(_textureNormal);
        }

        private void SetTexture(string textureName)
        {
            if (HasAtlas)
            {
                var f = _atlas.Frames[textureName];
                SourceRectangle = new Rectangle(f.X, f.Y, f.Width, f.Height);
            }
            else
            {
                Texture = AssetsManager.Instance.GetTexture(textureName);
                SourceRectangle = Texture.Bounds;
            }
        }
    }
}