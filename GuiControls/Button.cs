using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;

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

        public Button(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, string textureName, byte? textureId, string textureNormal, string textureActive, string textureDisabled, string textureHover, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, alignment, size, textureAtlas, textureName, textureId, layerDepth, parent)
        {
            _textureNormal = textureNormal;
            _textureActive = textureActive;
            _textureHover = textureHover;
            _textureDisabled = textureDisabled;

            Enabled = true;
        }

        public override void LoadContent(ContentManager content)
        {
            if (string.IsNullOrEmpty(TextureAtlas))
            {
                Texture = AssetsManager.Instance.GetTexture(TextureName);
                SourceRectangle = Texture.Bounds;
            }
            else
            {
                Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
                _atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);
                SourceRectangle = TextureId == null ? _atlas.Frames[TextureName].ToRectangle() : _atlas.Frames[(int)TextureId].ToRectangle();
            }
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            if (!Enabled)
            {
                var f = _atlas.Frames[_textureDisabled];
                SourceRectangle = new Rectangle(f.X, f.Y, f.Width, f.Height);

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
            else
            {
                if (ActualDestinationRectangle.Contains(input.MousePosition))
                {
                    var f = _atlas.Frames[_textureHover];
                    SourceRectangle = new Rectangle(f.X, f.Y, f.Width, f.Height);
                    if (input.IsLeftMouseButtonReleased)
                    {
                        OnClick(new EventArgs());
                    }
                }
                else
                {
                    var f = _atlas.Frames[_textureNormal];
                    SourceRectangle = new Rectangle(f.X, f.Y, f.Width, f.Height);
                }
            }

            Label?.Update(input, deltaTime);
        }

        public override void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transform);
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);

            EndSpriteBatch(spriteBatch);

            Label?.Draw();
        }

        private void OnClick(EventArgs e)
        {
            _cooldownTimeInMilliseconds = 50.0f;
            var f = _atlas.Frames[_textureActive];
            SourceRectangle = new Rectangle(f.X, f.Y, f.Width, f.Height);
            Click?.Invoke(this, e);
        }

        private void OnClickComplete()
        {
            _cooldownTimeInMilliseconds = 0.0f;
            var f = _atlas.Frames[_textureNormal];
            SourceRectangle = new Rectangle(f.X, f.Y, f.Width, f.Height);
        }
    }
}