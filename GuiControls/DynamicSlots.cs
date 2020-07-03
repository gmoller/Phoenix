using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;

namespace GuiControls
{
    public class DynamicSlots
    {
        private readonly string _textureAtlasString;
        private readonly string _textureString;
        private readonly Vector2 _size;
        private readonly Vector2 _topLeftPosition;

        private readonly int _numberOfSlotsX;
        private readonly int _numberOfSlotsY;
        private Rectangle _slot;
        private List<Rectangle> _slots;
        private readonly float _slotPadding;

        private Texture2D _texture;

        public DynamicSlots(Vector2 topLeftPosition, Vector2 size, string textureAtlasString, string textureString, int numberOfSlotsX, int numberOfSlotsY, float slotPadding)
        {
            _topLeftPosition = topLeftPosition;
            _size = size;
            _textureAtlasString = textureAtlasString;
            _textureString = textureString;

            _numberOfSlotsX = numberOfSlotsX;
            _numberOfSlotsY = numberOfSlotsY;
            _slotPadding = slotPadding;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture(_textureAtlasString);
            var atlas = AssetsManager.Instance.GetAtlas(_textureAtlasString);

            var frame = atlas.Frames[_textureString];
            _slot = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            float startX = _topLeftPosition.X + _slotPadding;
            float startY = _topLeftPosition.Y + _slotPadding;
            float slotWidth = (_size.X - _slotPadding * 2.0f) / _numberOfSlotsX;
            float slotHeight = (_size.Y - _slotPadding * 2.0f) / _numberOfSlotsY;
            _slots = CreateSlots(startX, startY, slotWidth, slotHeight, _numberOfSlotsX, _numberOfSlotsY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var slot in _slots)
            {
                spriteBatch.Draw(_texture, slot, _slot, Color.White);
            }
        }

        public bool IsMouseOverSlot(Microsoft.Xna.Framework.Point mousePostion)
        {
            foreach (var slot in _slots)
            {
                if (slot.Contains(mousePostion))
                {
                    return true;
                }
            }

            return false;
        }

        private List<Rectangle> CreateSlots(float startX, float startY, float slotWidth, float slotHeight, int numberOfSlotsX, int numberOfSlotsY)
        {
            var slots = new List<Rectangle>();

            float x = startX;
            float y = startY;

            for (int j = 0; j < numberOfSlotsY; ++j)
            {
                for (int i = 0; i < numberOfSlotsX; ++i)
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