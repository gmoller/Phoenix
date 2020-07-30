using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class DynamicSlots : Control
    {
        private readonly int _numberOfSlotsX;
        private readonly int _numberOfSlotsY;
        private Rectangle _slot;
        private List<Rectangle> _slots;
        private readonly float _slotPadding;
        private readonly List<LabelSized> _labels;

        public DynamicSlots(string name, Vector2 position, Alignment positionAlignment, Vector2 size, string textureAtlas, string textureName, int numberOfSlotsX, int numberOfSlotsY, float slotPadding, List<LabelSized> labels = null) :
            base(name, position, positionAlignment, size, textureAtlas, textureName, null, null, null, null)
        {
            _numberOfSlotsX = numberOfSlotsX;
            _numberOfSlotsY = numberOfSlotsY;
            _slotPadding = slotPadding;

            _labels = labels;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
            var atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);

            var frame = atlas.Frames[TextureName];
            _slot = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            var startX = TopLeft.X + _slotPadding;
            var startY = TopLeft.Y + _slotPadding;
            var slotWidth = (Size.X - _slotPadding * 2.0f) / _numberOfSlotsX;
            var slotHeight = (Size.Y - _slotPadding * 2.0f) / _numberOfSlotsY;
            _slots = CreateSlots(startX, startY, slotWidth, slotHeight, _numberOfSlotsX, _numberOfSlotsY);
        }

        protected override void Draw(SpriteBatch spriteBatch, Matrix? transform = null)
        {
            var i = 0;
            foreach (var slot in _slots)
            {
                spriteBatch.Draw(Texture, slot, _slot, Color.White);
                if (_labels != null && i < _labels.Count)
                {
                    _labels[i].Draw();
                    i++;
                }
            }
        }

        private List<Rectangle> CreateSlots(float startX, float startY, float slotWidth, float slotHeight, int numberOfSlotsX, int numberOfSlotsY)
        {
            var slots = new List<Rectangle>();

            var x = startX;
            var y = startY;

            for (var j = 0; j < numberOfSlotsY; ++j)
            {
                for (var i = 0; i < numberOfSlotsX; ++i)
                {
                    var rect = new Rectangle((int)x, (int)y, (int)slotWidth, (int)slotHeight);
                    slots.Add(rect);
                    x += slotWidth;
                }

                x = startX;
                y += slotHeight;
            }

            return slots;
        }
    }
}