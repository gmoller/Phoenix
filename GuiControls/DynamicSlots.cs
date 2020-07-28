using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using AssetsLibrary;
using Input;

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
        private readonly List<Label> _labels;

        public DynamicSlots(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, string textureName, int numberOfSlotsX, int numberOfSlotsY, float slotPadding, List<Label> labels = null) :
            base(name, position, alignment, size, textureAtlas, textureName, null, null, null, null)
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

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            base.Update(input, deltaTime, transform);
        }

        public override void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

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

            EndSpriteBatch(spriteBatch);
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