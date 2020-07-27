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
    public class Label2 : ControlBase
    {
        private readonly string _fontName;
        private readonly Color _textColor;
        private readonly Color? _backColor;
        private readonly float _scale;

        private SpriteFont _font;
        public string Text { get; set; }

        public Label2(string name, Vector2 position, Vector2 size, string text, string fontName, Color textColor, Color backColor, float layerDepth = 0.0f, IControl parent = null) : 
            this(name, position, ContentAlignment.TopLeft, size, text, fontName, textColor, backColor, layerDepth, parent)
        {
        }

        public Label2(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string text, string fontName, Color textColor, Color? backColor = null, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, alignment, size, string.Empty, string.Empty, null, layerDepth, parent)
        {
            _fontName = fontName;
            Text = text;
            _textColor = textColor;
            _backColor = backColor;
            _scale = 1.0f;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = AssetsManager.Instance.GetSpriteFont(_fontName);
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
        }

        public override void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

            if (_backColor != null)
            {
                spriteBatch.FillRectangle(ActualDestinationRectangle, _backColor.Value, LayerDepth);
            }

            spriteBatch.DrawString(_font, Text, TopLeft.ToVector2(), _textColor, 0.0f, Vector2.Zero, _scale, SpriteEffects.None, LayerDepth);

            EndSpriteBatch(spriteBatch);
        }
    }
}